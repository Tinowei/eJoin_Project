using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Web.ViewModels.AccountViewModel;
using ApplicationCore.Extensions;
using static QRCoder.PayloadGenerator;

namespace Web.Services.AccountService
{
    public class GoogleAccountService
    {
        private readonly IHttpContextAccessor _contextAccessor; // 宣告
        private readonly IRepository<GoogleLoginInfo> _googleLoginInfoRepo;
        private readonly IRepository<GoogleMemberRelation> _googleMemberRelationRepo;
        private readonly IRepository<Member> _memberRepo;
        private readonly IAuthService _authService;

        public GoogleAccountService(IHttpContextAccessor contextAccessor,
            IRepository<GoogleLoginInfo> googleLoginInfoRepo,
            IRepository<GoogleMemberRelation> googleMemberRelationRepo,
            IRepository<Member> memberRepo,
            IAuthService authService) // 注入
        {
            _contextAccessor = contextAccessor; // 賦值
            _googleLoginInfoRepo = googleLoginInfoRepo;
            _googleMemberRelationRepo = googleMemberRelationRepo;
            _memberRepo = memberRepo;
            _authService = authService;
        }

        public async Task<int> SaveGoogleResponse()
        {
            int result = -1;
            try
            {
                var cookie = await _contextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme) ?? throw new NullReferenceException("你沒有拿到Cookie回傳值");

                // 取得Google掛在Cookie上的登入狀態，並取得需要的資訊
                var claim = cookie.Principal.Claims;
                string name = claim.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                string gmail = claim.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                string nameIdentifier = claim.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                // 檢查資料庫中是否已經存在該用戶的GoogleLoginInfo，若沒有的話，新增一筆資料進資料庫
                var googleLoginInfoTarget = await _googleLoginInfoRepo.SingleOrDefaultAsync(gli => gli.NameIdentifier == nameIdentifier);
                if (googleLoginInfoTarget == null)
                {
                    googleLoginInfoTarget = new GoogleLoginInfo()
                    {
                        Name = name,
                        Gamil = gmail,
                        NameIdentifier = nameIdentifier,
                        CreateTime = DateTime.Now,
                    };

                    googleLoginInfoTarget = _googleLoginInfoRepo.Add(googleLoginInfoTarget);
                }

                var googleMemberRelationTarget = _googleMemberRelationRepo.SingleOrDefault(gmr => gmr.GoogleLoginInfoId == googleLoginInfoTarget.Id);

                //判斷cookie內的NameIdentify如果不存在GoogleMemberChain中，return至完善資料的頁面
                if (googleMemberRelationTarget == null)
                {
                    result = 1;
                    return result;
                    
                }

                //判斷cookie內的NameIdentify存在於GoogleMemberChian中，那就
                if (cookie.Properties.Items.Any(i => i.Key == "MemberId"))
                {
                    // TODO綁Google
                    // New一個for GoogleInfo的一筆資料
                    // 資料內容從cookie中取得
                    if(!int.TryParse(cookie.Properties.Items["MemberId"], out var memberId))
                    {
                        return 2;//
                    }
                    var relation = new GoogleMemberRelation()
                    {
                        GoogleLoginInfoId = googleLoginInfoTarget.Id,
                        MemberId = memberId
                    };
                    
                    
                    //TODO 將MemberID 連結至GoogleMemberRelation

                }

                // TODO根據取得的cookie內容把Member取出，並更新cookie
                var googleInfoTarget = _googleLoginInfoRepo.SingleOrDefault(gli => gli.NameIdentifier == nameIdentifier);
                var relationTartget = _googleMemberRelationRepo.SingleOrDefault(gmr => gmr.GoogleLoginInfoId == googleInfoTarget.Id);
                var memberTarget = _memberRepo.GetById(relationTartget.MemberId);

                await _authService.Login(memberTarget);
                // 從result中獲取用戶的身份訊息
                // var claims = cookie.Principal.Claims;

                // 創建一個新的ClaimsIdentity
                // var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // 使用SignInAsync方法將用戶的身份訊息存儲到session中
                //await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                result = 0;
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return result;
            }
        }

        public async Task<CompletedInfoViewModel> GetCompletedInfoViewModelAsync()
        {
            var cookie = await _contextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme) ?? throw new NullReferenceException("你沒有拿到Cookie回傳值");
            var claim = cookie.Principal.Claims;
            string name = claim.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            string gmail = claim.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            return new CompletedInfoViewModel()
            {
                Gmail = gmail,
                Name = name,
            };
        }

        public async Task<Member> RegisterGoogleLoginAsync(string fullname, string phone, string password)
        {
            //處理叫進來的成員資料，
            var cookie = await _contextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme) ?? throw new NullReferenceException("你沒有拿到Cookie回傳值");
            var claim = cookie.Principal.Claims;
            string name = claim.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            string gmail = claim.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            string nameIdentifier = claim.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;



            var member = new Member()
            {
               
                Name = fullname,
                Email = gmail,
                DisplayName = fullname,
                CoverUrl = "/images/cardUploadImg.png",
                AvatarUrl = "/images/profilePicture.png",
                Phone = phone,
                Description = string.Empty,
                Birthday = DateTime.Now,
                Gender = 0,
                Relationship = 0,
                City = string.Empty,
                Address = string.Empty,
                Password = password.HashPassword(), // 假設你有一個方法來將密碼進行雜湊
                RegisterTime = DateTime.Now,
                IsDelete = false,
            
        };

            var googleLoginInfoTarget = _googleLoginInfoRepo.SingleOrDefault(gli => gli.NameIdentifier == nameIdentifier);

            var relation = new GoogleMemberRelation()
            {
                Member = member,
                GoogleLoginInfoId = googleLoginInfoTarget.Id,
            };

            var savedRelation = _googleMemberRelationRepo.Add(relation);
            var memberTarget = _memberRepo.SingleOrDefault(m => m.Id == savedRelation.MemberId);

            return memberTarget;
        }
    }
}
