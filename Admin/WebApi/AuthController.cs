using Admin.Helpers;
using Admin.Models;
using Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.WebApi
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtHelper _jwt;
        private readonly UserMangerService _userMangerService;

        public AuthController(JwtHelper jwt, UserMangerService userMangerService)
        {
            _jwt = jwt;
            _userMangerService = userMangerService;
        }

        /// <summary>
        /// 故意要求每個頁面都要經過驗證，未來要和Layout上的httpGet一起刪除
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult CheckAuthentication()
        {
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginInDTO request)
        {
            if (!_userMangerService.IsUserValid(request))
            {
                return Ok(new BaseApiResponse());
            }
            // 在此設定jwt授權時間
            int jwtExpireTime = 10;
            return Ok(new BaseApiResponse(_jwt.GenerateToken(request.UserName, jwtExpireTime)));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUserName()
        {
            return Ok(new BaseApiResponse(_userMangerService.GetUserName()));
        }

        [HttpGet]
        // [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetClaims()
        {
            return Ok(new BaseApiResponse(User.Claims.Select(x => new { x.Type, x.Value })));
        }

    }

    public class LoginInDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
