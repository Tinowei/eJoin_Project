using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Admin.WebApi
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }



        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var test = _orderService.GetOrders();
            
            return Ok(test);
        }
    }
}
