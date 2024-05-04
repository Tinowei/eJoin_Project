using Microsoft.AspNetCore.Mvc;
using Web.Services.TicketService;
using Web.ViewModels.TicketViewModel;

namespace Web.Controllers
{
    public class TicketController : Controller
    {
        public IActionResult Index()
        {
            var _ticketService = new TicketTestService();
            var indexViewModel = _ticketService.GetTicketViewModel();

            return View(indexViewModel);
        }


        public IActionResult Apply()
        {
            return View();
        }
    }
}
