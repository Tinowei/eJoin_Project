using ApplicationCore.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Services.AccountService;
using Web.Services.HomeService;

namespace Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHomeService _homeService;
        private readonly UserContextService _userContextService;
        private readonly int _userId;

        public HomeController(IHomeService service, UserContextService userContextService)
        {
            _homeService = service;
            _userContextService = userContextService;
            _userId = _userContextService.GetUserId();
        }

        //參考17~21行的寫法去初始化Service
        //private readonly HomeTestService _homeService;
        //public HomeController()
        //{
        //    _homeService = new HomeTestService();
        //}

        public async Task<IActionResult> Index()
        {
            //參考這樣的寫法把每個Action的ViewModel假資料建立抽成變成方法

            if (_userId == 0)
            {
                var vm = await _homeService.GetIndexViewModel();
                return View(vm);
            }
            else
            {
                var vm = await _homeService.GetIndexViewModel(_userId);
                return View(vm);
            }
        }


        public async Task<IActionResult> Search()
        {
            var vm = await _homeService.GetSearchViewModel();

            return View(vm);
        }

        [Route("home/event/{eventId:int}")]
        public async Task<IActionResult> Event(int eventId)
        {
            try
            {

                var vm = (_userId == 0)
                         ? await _homeService.GetEventViewModel(eventId)
                         : await _homeService.GetEventViewModel(eventId, _userId);

                return View(vm);
            }
            catch (KeyNotFoundException)
            {
                return View("Error", new ErrorViewModel());
            }
            catch (EventIsRemovedException)
            {
                return View("RemoveEvent");
            }
            catch (Exception)
            {
                return View("Error");
            }

        }

    }
}
