using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Web.Services.AccountService;
using Web.Services.OrganizeService;
using Web.ViewModels.OrganizeViewModel;
using Web.WebApiServices.OrganizeServices;

namespace Web.WebApi.Organize
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrganizeEditController : ControllerBase
    {
        private readonly OrganizeService _organizeService;
        private readonly OrganizeApiService _organizeApiService;
        private readonly UserContextService _userContextService;

        public OrganizeEditController(OrganizeApiService organizeApiService, 
                                      UserContextService userContextService, 
                                      OrganizeService organizeService)
        {
            _organizeApiService = organizeApiService;
            _userContextService = userContextService;
            _organizeService = organizeService;
        }

        [HttpPost]
        public async Task<IActionResult> GetEventData([FromBody] int eventId)
        {
            if (eventId == 0)
            {
                return NotFound();
            }

            try
            {
                var userId = _userContextService.GetUserId();

                var vm = await _organizeService.GetEditViewModel(eventId, userId);

                if (vm == null)
                {
                    return NotFound();
                }
                
                return Ok(vm);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"An error occurred while editing the event. {ex}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditTheme([FromBody] EventThemeDTO eventQuery)
        {
            try
            {
                var userId = _userContextService.GetUserId();

                await _organizeApiService.EditEventTheme(userId, eventQuery);

                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"An error occurred while editing the event theme. {ex}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditBasicInfo([FromBody] EditBasicInfoDTO eventQuery)
        {
            try
            {
                var userId = _userContextService.GetUserId();

                await _organizeApiService.EditBasicInfo(userId, eventQuery);

                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"An error occurred while editing the event. {ex}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditDescInfo([FromBody] EditDescInfoDTO eventQuery)
        {
            try
            {
                var userId = _userContextService.GetUserId();

                await _organizeApiService.EditDescInfo(userId, eventQuery);

                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"An error occurred while editing the event. {ex}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditTicket([FromBody] EditTicketDTO eventQuery)
        {
            try
            {
                var userId = _userContextService.GetUserId();

                await _organizeApiService.EditTicket(userId, eventQuery);

                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"An error occurred while editing the event. {ex}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditStatus([FromBody] EditStatusDTO eventQuery)
        {
            try
            {
                var userId = _userContextService.GetUserId();

                await _organizeApiService.EditStatus(userId, eventQuery);

                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"An error occurred while editing the event. {ex}" });
            }
        }
    }
}
