using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.WebApiServices.RegisterServices;

namespace Web.WebApi.Register
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly RegisterApiService _registerService;
        private readonly int _memberId;
        public RegisterController(RegisterApiService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost]
        public IActionResult SelectTicket(SelectTicketRequest selectTicket)
        {
            int memberId = int.Parse(HttpContext.User.Identity?.Name ?? _memberId.ToString());
            var result = _registerService.GetSelectTicketResponse(selectTicket, memberId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDTO createOrderDTO)
        {
            var result = await _registerService.CheckOrderIsExist(createOrderDTO);
            return Ok(result);
        }

        /// <summary>
        /// 這是當綠界完成付款流程後，後端接收ReturnURL的API
        /// </summary>
        /// <param name="ecpayResult">綠界的回傳物件</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveEcpayResult([FromForm]EcpayResultDTO ecpayResult)
        {
            try
            {
                _registerService.SaveEcpayResult(ecpayResult);
                Console.WriteLine("後端API執行成功");
                return Ok(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
    }

    public class OperationResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
        public OperationResult()
        {
            IsSuccess = false;
            Message = string.Empty;
            Result = default(T);
        }
    }
}
