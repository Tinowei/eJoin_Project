using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Services.AccountService;
using Web.WebApiServices.HostServices;

namespace Web.WebApi.Host
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HostApiController : ControllerBase
    {
        private readonly UserContextService _userContextService;
        private readonly int _userId;
        private readonly HostApiService _hostApiService;

        public HostApiController(UserContextService userContextService, HostApiService hostApiService)
        {
            _userContextService = userContextService;
            _userId = _userContextService.GetUserId();
            _hostApiService = hostApiService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDTO eventQuery)
        {
            if (_userId == 0)
            {
                return BadRequest();
            }
            try
            {
                await _hostApiService.CreateEvent(_userId, eventQuery);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"An error occurred while editing the event. {ex}" });
            }
        }
    }
}
