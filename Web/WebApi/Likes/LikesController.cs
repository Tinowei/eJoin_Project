using Microsoft.AspNetCore.Mvc;
using Web.Services.AccountService;
using Web.WebApiServices.LikesApiService;

namespace Web.WebApi.Likes
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly UserContextService _userContextService;
        private readonly LikesApiService _likeApiService;

        public LikesController(
            UserContextService userContextService,
            LikesApiService likeApiService)//ctor
        {
            _userContextService = userContextService;
            _likeApiService = likeApiService;
        }

        [HttpPost]
        public IActionResult Add([FromBody] LikesDTO likeDTO)
        {
            var userId = _userContextService.GetUserId();

            if (!_userContextService.IsAuthenticated() || userId == 0)
            {
                return Unauthorized();
            }

            //to do something
            _likeApiService.Like(userId, likeDTO.EventId);

            return Ok(_likeApiService.GetLikeCount(likeDTO.EventId));
        }

        [HttpPost]
        public IActionResult Remove([FromBody] LikesDTO likeDTO)
        {
            var userId = _userContextService.GetUserId();

            if (!_userContextService.IsAuthenticated() || userId == 0)
            {
                return Unauthorized();
            }

            _likeApiService.UnLike(userId, likeDTO.EventId);

            return Ok(_likeApiService.GetLikeCount(likeDTO.EventId));
        }
    }
}
