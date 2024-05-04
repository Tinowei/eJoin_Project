using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Services.AccountService;
using Web.WebApiServices.FollowServices;

namespace Web.WebApi.Follow
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private readonly UserContextService _userContextService;
        private readonly FollowApiService _followApiService;

        public FollowController(
            UserContextService userContextService,
            FollowApiService followApiService)//ctor
        {
            _userContextService = userContextService;
            _followApiService = followApiService;
        }

        [HttpPost]
        public IActionResult Add([FromBody] FollowDTO followDTO)
        {
            var userId = _userContextService.GetUserId();

            if (!_userContextService.IsAuthenticated() || userId == 0)
            {
                return Unauthorized();
            }

            //to do something
            _followApiService.Follow(userId, followDTO.BeingFollowedId);

            return Ok();
        }

        [HttpPost]
        public IActionResult Remove([FromBody] FollowDTO followDto)
        {
            var userId = _userContextService.GetUserId();

            if (!_userContextService.IsAuthenticated() || userId == 0)
            {
                return Unauthorized();
            }

            _followApiService.UnFollow(userId, followDto.BeingFollowedId);

            return Ok();
        }

        [HttpGet]
        public IActionResult GetFansCount(int memberId)
        {
            var fansCount = _followApiService.GetFansCount(memberId);

            return Ok(fansCount);
        }
    }
}
