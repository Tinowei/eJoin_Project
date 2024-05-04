using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Web.Services.AccountService;
using Web.ViewModels.AccountViewModel;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly GoogleAccountService _googleAccountService;
        private readonly ISendEmail _sendEmail;

        public AccountController(IAuthService authService, GoogleAccountService googleAccountService, ISendEmail sendEmail)
        {
            _authService = authService;
            _googleAccountService = googleAccountService;
            _sendEmail = sendEmail;
        }

        //登入

        [Route("Login")]
        public IActionResult LoginView(LoginViewModel request)
        {
            return View("Login", request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel request)
        {
            var user = await _authService.AuthenticateUser(request.Email, request.Password);
            if (user == null)
            {
                return View("Login");
            }

            await _authService.Login(user);
            _sendEmail.SendEmail(new ApplicationCore.Models.SendEmailDTO()
            {
                recipientName = user.Name,
                recipientAddress = user.Email,
                subject = "eJoin登入成功",
                body = "</b>您已成功登入eJoin網站",
            });

            if (string.IsNullOrEmpty(request.ReturnUrl))
            {
                return Redirect("/Home");
            }

            return Redirect(request.ReturnUrl);
        }

        //註冊

        public IActionResult SignupView()
        {
            return View("Signup");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(SignupViewModel request)
        {
            try
            {
                // 確保你的方法是標記為async的，並且返回Task或Task<T>
                var member = await _authService.RegisterLocalLoginAsync(request.Email, request.Fullname, request.Phonenumber, request.Password);
                if (member.Id == 0)
                {
                    request.Status = 1;
                    return View(request);
                }
                await _authService.Login(member);

                // 處理註冊成功後的邏輯
                return Redirect("/Home");
            }
            catch (Exception ex)
            {
                // 處理註冊失敗的情況
                Console.WriteLine(ex);
                request.Status = -1;
                return View(request);
            }
        }

        private void ShowErrorMessage(string message)
        {
            //TODO
            throw new NotImplementedException(message);
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl }; //Items = { { "MemberId", "1" } }
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await _googleAccountService.SaveGoogleResponse();

            switch (result)
            {
                case -1:
                    // 出現EX
                    throw new NullReferenceException("處理出錯後的頁面");
                    break;
                case 1:

                    //todo 新增一個讓客戶去完善資料的view
                    //throw new NullReferenceException("處理跳轉道完善資料的View");
                    return Redirect("/Account/CompletedInfo");

                    break;
                case 0:
                    //return View();
                    break;
                case 2:
                    return Redirect("/Member/MyProfile");
                    break;
                    //綁定失敗 return 個人profile;
            }
            return Redirect("/Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            foreach (var cookieKey in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookieKey);
            }

            return Redirect("/Home");


        }

        [HttpGet]
        public async Task<IActionResult> CompletedInfo()
        {
            var vm = await _googleAccountService.GetCompletedInfoViewModelAsync();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CompletedInfo(FormDTO form)
        {
            // TODO處理建立Member和Relation表的資料的Service
            Member user = await _googleAccountService.RegisterGoogleLoginAsync(form.Name, form.Phone, form.Password);
            await _authService.Login(user);
            return Redirect("/Home");
        }
        
        public class FormDTO
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Password { get; set; }
        }
    }
}
