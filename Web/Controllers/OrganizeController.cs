using Microsoft.AspNetCore.Mvc;
using Web.ViewModels.OrganizeViewModel;
using Web.Services.OrganizeService;
using static NuGet.Packaging.PackagingConstants;
using Microsoft.AspNetCore.Authorization;
using Web.Services.AccountService;
using System.ComponentModel;

namespace Web.Controllers
{
    [Authorize]
    public class OrganizeController : Controller
    {
        private readonly OrganizeService _organizeService;
        private readonly UserContextService _userContextService;
        public OrganizeController(OrganizeService organizeService, UserContextService userContextService)
        {
            _organizeService = organizeService;
            _userContextService = userContextService;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                int userId = _userContextService.GetUserId();

                var vm = await _organizeService.GetIndexViewModel(userId, page);

                return View(vm);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return NotFound();
            }
        }

        [Route("Organize/Overview/{eventId:int}")]
        public async Task<IActionResult> Overview(int eventId)
        {
            try
            {
                int userId = _userContextService.GetUserId();
                var vm = await _organizeService.GetOverView(eventId, userId);

                return View(vm);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex);
                return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return NotFound();
            }
        }

        [Route("Organize/Edit/{eventId:int}")]
        public async Task<IActionResult> Edit(int eventId)
        {
            if (eventId == 0)
            {
                return NotFound();
            }

            try
            {
                int userId = _userContextService.GetUserId();
                var vm = await _organizeService.GetEditViewModel(eventId, userId);

                if (vm == null)
                {
                    return NotFound();
                }

                return View(vm);
            }
            catch (UnauthorizedAccessException ex) 
            {
                Console.WriteLine(ex);
                return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return NotFound();
            }
        }

        public IActionResult Validate()
        {
            return View();
        }
    }
}
