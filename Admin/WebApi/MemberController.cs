using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.WebApi.Member;
using static QRCoder.PayloadGenerator;


namespace Admin.WebApi
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MemberController : ControllerBase
    {

        private readonly IRepository<Member> _memberRepo; //宣告一個唯讀的欄位

        public MemberController(IRepository<Member> memberRepo) //初始化建構式(透過注入的方式，像這邊是注入資料庫)
        {
            _memberRepo = memberRepo; //指派給欄位
        }


        [HttpGet]
        public IActionResult GetMemberData()
        {
            //NEW DTO
            List<MemberData> result = new List<MemberData>();

            //抓資料庫的所有內容
            List<Member> members = _memberRepo.List(x => true);

            //將資料庫的內容，重新組合成DTO的樣子
            result = members.Select(m => new MemberData()
            {
                MemberId = m.Id,
                Email = m.Email,
                Name = m.Name,
                DisplayName = m.DisplayName ?? string.Empty,
                Phone = m.Phone,
                RegisterTime = m.RegisterTime,
                LastEditTime = m.LastEditTime,
            }).ToList();

            return Ok(result);
        }
    }
}
