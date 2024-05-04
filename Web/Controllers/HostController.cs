using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Services.AccountService;
using Web.Services.HostService;
using Web.Services.OrganizeService;
//using CloudinaryDotNet;
//using CloudinaryDotNet.Actions;

namespace Web.Controllers
{
    [Authorize]
    public class HostController : Controller
    {
        private readonly HostService _service;
        public HostController(HostService hostService, UserContextService userContextService)
        {
            _service = hostService;
        }
        /// <summary>
        /// 新增活動步驟1
        /// 勾選規範同意書
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> EditEvent()
        {
            try
            {
                var vm = await _service.GetThemeSelectViewModel();

                if (vm == null)
                {
                    return NotFound();
                }

                return View(vm);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return NotFound();
            }
        }
    }
}
