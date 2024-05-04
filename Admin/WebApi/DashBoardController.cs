using Admin.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Admin.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly DashboardService _DashboardService;

        public DashBoardController(DashboardService dashboardService)
        {
            _DashboardService = dashboardService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var data = _DashboardService.GetData();

            return Ok(data);
        }
    }
}
