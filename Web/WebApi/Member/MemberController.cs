using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Services.MemberService;
using Web.WebApiServices.MemberServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web.WebApi.Member
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly MemberApiServices _memberService; //宣告一個唯讀的欄位

        public MemberController(MemberApiServices memberService) //初始化建構式(透過注入的方式，像這邊是注入Services)
        {
            _memberService = memberService; //指派給欄位
        }

        [HttpPost]
        public IActionResult GetMemberPassword(MemberPassword password)
        {
            MemberPassword result = _memberService.GetMemberApiServices(password);

            return Ok(result);
        }


        [HttpPost]
        public IActionResult GetMemberProfile(MemberProfile profile)
        {
            MemberProfile result = _memberService.GetMemberApiServices(profile);

            return Ok(result);
        }


        [HttpPost]
        public IActionResult GetMemberProfileImage(MemberProfileImage Image)
        {
            MemberProfileImage result = _memberService.GetMemberProfileImage(Image);
            return Ok(result);
        }



        //[HttpPost]
        //public IActionResult Index2(MemberPassword password)
        //{
        //    var result = new MemberPassword()
        //    {
        //        MemberId = 1,
        //        OriginalPassword = "1111111",
        //        NewPassword = "2222222",
        //        ConfirmPassword = "2222222"
        //    };
        //    return Ok(result);
        //}

    }
}
