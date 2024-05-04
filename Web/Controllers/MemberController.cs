using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Web.Services.AccountService;
using Web.Services.MemberService;
using Web.ViewModels.MemberViewModel;

namespace Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly UserContextService _userContextService;
        private readonly int _loginUserId;

        //初始化_memberService
        private readonly MemberService _memberService;
        //private readonly string loginMemberId = HttpContext.User;
        public MemberController(
            UserContextService userContextService,
            MemberService memberService)
        {
            _userContextService = userContextService;
            _memberService = memberService;
            _loginUserId = _userContextService.GetUserId();
        }

        [AllowAnonymous]
        [Route("member/index/{memberId:int}")]
        public IActionResult Index(int memberId)
        {
            //取得UserId
            var userId = _loginUserId;
            var IndexViewModel = _memberService.GetIndexViewModel(memberId);
            
            IndexViewModel.UserId = userId;

            //檢查使用者是否有追蹤這個帳號
            IndexViewModel.IsFollow = _memberService.IsFollowThisMember(userId, memberId);

            foreach(var item in IndexViewModel.PersonalCard)
            {
                //檢查活動使用者是否有按過喜歡
                item.IsLike = _memberService.CheckUserIsPickLike(item, userId);
            }


            return View(IndexViewModel);
        }
        public IActionResult MyTicket()
        {
            var myTicketViewModel = _memberService.GetMyTicketViewModel(_loginUserId);
            return View(myTicketViewModel);
        }
        public IActionResult MyEvent()
        {
            var myEventViewModel = _memberService.GetMyEventViewModel(_loginUserId);
            return View(myEventViewModel);
        }
        public IActionResult MyProfile()
        {
            var myProfileviewModel = _memberService.GetMyProfileViewModel(_loginUserId);
            return View(myProfileviewModel);
        }

        public IActionResult MyAccount()
        {
            var myAccountViewModel = _memberService.GetMyAccountViewModel(_loginUserId);
            return View(myAccountViewModel);
        }
    }
}



