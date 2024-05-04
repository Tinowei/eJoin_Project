using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.WebApiServices.RegisterServices;
using Web.WebApiServices.SignupServices;

namespace Web.WebApi.Signup
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignupController : ControllerBase
    {
        private readonly SignupApiService _signupApiService; // 宣告
        public SignupController(SignupApiService signupApiService) // 注入
        {
            _signupApiService = signupApiService; // 指派
        }

        [HttpPost]
        public IActionResult SendVerifyNumberMail(SendVerifyNumberMailDTO sendVerifyEmail)
        {
            _signupApiService.SendVerifyEmail(sendVerifyEmail.RecipientAddress, sendVerifyEmail.VerifyNumber);
            return Ok();
        }
    }
}
