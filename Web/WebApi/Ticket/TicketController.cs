using ApplicationCore.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Web.WebApiServices.TicketServices;

namespace Web.WebApi.Ticket
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketApiService _ticketApiService;

        public TicketController(TicketApiService ticketApiService)
        {
            _ticketApiService = ticketApiService;
        }
        
        /// <summary>
        /// 前端打POST進來，回傳帶著Byte[]形式的QRCODE的response回去，Qrcode不存下來
        /// </summary>
        /// <param name="selectedTicket"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UseTicket([FromBody] UserSelectedTicketDTO selectedTicket)
        {
            //todo:api打回來應該會帶著票數數量,票種id,活動id，然後將參數帶進CreateQrCode，呼叫QrCodeHelper.GenerateQrCodeByQuantity方法產生一張指定的qrcode
            
            var qrCodeResultByService = _ticketApiService.CreateQrCode(selectedTicket.SelectedNumber,selectedTicket.EventId,selectedTicket.TicketTypeId);
            if (qrCodeResultByService.Success)
            {
                return new JsonResult(new {Success = true,ImageDataUrl = qrCodeResultByService.ImageDataUrl });
            }
            
            return new JsonResult(new {  Success = false,ErrorMessage = qrCodeResultByService.ErrorMessage  });
        }

        [HttpGet]
        public async Task<int> GetMemberTicketsCount()
        {
            var counts = _ticketApiService.GetMemberTicketCounts();
            return counts;
        }
        
        /// <summary>
        /// 將資料庫該用戶的已發行票券傳至前端
        /// </summary>
        /// <param name="memberid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMemberTickets(int page,int pageSize)
        {

                var tickets = _ticketApiService.GetTicketsFromDB(page,pageSize);
                //var ticketsJson = JsonConvert.SerializeObject(tickets);
                return Ok(tickets);
        }
        
        /// <summary>
        /// 核銷票券
        /// </summary>
        /// <param name="ticketNumbers"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ValidateTickets([FromQuery] string ticketNumbers )
        {
            //從query提取qrcode的票券編號
            if (string.IsNullOrEmpty(ticketNumbers))
            {
                return BadRequest(new { message = "無效的QrCode" });
            }
            
            // 如果不是主辦方，返回提示的訊息
            if (!_ticketApiService.IsValidHostMemberId(ticketNumbers))
            {
                return Unauthorized(new { message = "你不是主辦方，沒有權限可核銷票券!" });
            }
            
            //進行核銷的動作
            var (verifiedTickets,errorMessages)=await _ticketApiService.VerifyTargetTickets(ticketNumbers);
            if (errorMessages.Any())
            {
                // 一旦核銷過程有錯誤，就會返回錯誤訊息給前端
                return BadRequest(new { message = string.Join(", ", errorMessages) });
            }

            var result = verifiedTickets.Select(vt => new VerifiedTicketDTO()
            {
                TicketNumber = vt.ReleaseTicketNumber,
                Status = vt.Status,
                ExpireTime = vt.ExpireTime
            }).ToList();
            
            return Ok(new { message = "票券核銷成功。" ,data=result});
        }

        
        [HttpGet]
        public async Task<int> GetUsedTicketsCount()
        {
            var counts = _ticketApiService.GetUsedTicketCounts();
            return counts;
        }
        
        /// <summary>
        /// 將已使用過的票券回傳至前端
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMemberUsedTickets(int page,int pageSize)
        {
            var usedTickets = _ticketApiService.GetUsedTicketsFromDB(page,pageSize);
            
            return Ok(usedTickets);
        }
        
        
        //將購買記錄Orders傳至前端做渲染
        //目前打通了
        // [HttpGet]
        // public async Task<IActionResult> GetMemberOrders(int page,int pageSize)
        // {
        //     var orders = _ticketApiService.GetOrderDetails(page, pageSize);
        //     
        //     return Ok(orders);
        // }
        
        //將購買記錄Orders傳至前端做渲染
        //test
        [HttpGet]
        public async Task<IActionResult> GetMemberOrdersByPagedList(int page=1)
        {
            
            var orders = await _ticketApiService.GetOrderDetailsByPagedList(page);
            
            return Ok(orders);
        }
        
    }
}