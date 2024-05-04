using ApplicationCore.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Services.AccountService;
using Web.Services.RegisterService;
using Web.WebApi.Register;

namespace Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IRegisterService _registerService;
        private readonly UserContextService _userContextService;
        private readonly int _memberId;

        public RegisterController(IRegisterService registerService, UserContextService userContextService)
        {
            _registerService = registerService;
            _userContextService = userContextService;
            _memberId = _userContextService.GetUserId();
        }

        [Authorize]
        public IActionResult Index(int eventId)
        {
            // 以下為停用網頁快取，讓使用者不會因為按下上一頁而跳過控制器
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            int vaildCart = _registerService.GetVaildCart(_memberId);
            if (vaildCart == -1)
            {
                throw new Exception("請檢視資料表Cart是否異常");
            }
            else if(vaildCart !=  0)
            {
                return View("/Views/Register/GoToFillForm.cshtml", vaildCart);
            }
            var vm = _registerService.GetSelectTicketViewModel(eventId);
            return View(vm);
        }

        public IActionResult FillForm(int cartId)
        {   
            // 以下為停用網頁快取，讓使用者不會因為按下上一頁而跳過控制器
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            
            var vm = _registerService.GetFillFormViewModel(cartId);
            return View(vm);
        }

        [HttpPost]
        public IActionResult FillForm(Cart cart)
        {
            var cartId = _registerService.SaveFillFormInput(cart);
            if (cartId == 0)
            {
                // TODO 系統出現異常頁面
            }
            return Redirect($"/Register/Payment?cartId={cartId}");
        }

        [HttpGet]
        public IActionResult Payment(int cartId, int orderId)
        {
            var vm = _registerService.GetPaymentViewModel(cartId);
            ViewData["CartId"] = cartId;
            ViewData["OrderId"] = orderId;
            return View(vm);
        }

        public IActionResult Complete()
        {
            //_registerService.CompleteOrder("da0dc8507828caabd");
            return View();
        }

        [HttpGet]
        public IActionResult DirectToECPay(int orderId)
        {
            var vm = _registerService.GetDirectToECPayViewModel(orderId);
            if (vm == null)
            {
                // TODO
                throw new Exception("轉向綠界過程出現錯誤，回傳頁面待處理");
            }
            return View(vm);
        }

        [HttpPost]
        public IActionResult Complete(EcpayResultDTO ecpayResult)
        {
            if (ecpayResult.RtnCode != 1)
            {
                // 綠界交易失敗
                _ = int.TryParse(ecpayResult.CustomField1, out int orderId);
                var cartId = _registerService.GetCartIdByOrderId(orderId);
                if (cartId == 0) return Redirect("/Home");
                return View("/Views/Register/GoToPayment.cshtml", new List<int>() { cartId, orderId });
            }

            _registerService.CompleteOrder(ecpayResult.MerchantTradeNo);
            return View("/Views/Register/GoToComplete.cshtml");
        }


        ////FillForm表單提交
        //[HttpPost]
        //public IActionResult Payment(int eventId,ParticipantDTO participantDTO)
        //{
        //    var paymentViewModel = _service.GetPaymentViewModel(eventId);

        //    var vm = _registerService.GetPaymentViewModel(eventId);

        //    //test用
        //    var participant = GetFormInfo(participantDTO);
        //    //var test = $"{participantDTO.participantName},{participantDTO.Email},{participantDTO.PhoneNumber}";

        //    return View(vm);
        //}

        //private ParticipantDTO GetFormInfo(ParticipantDTO participant)
        //{
        //    TempData["Participant"] = JsonConvert.SerializeObject(participant);
        //    var participantJson = (string)TempData["Participant"];
        //    var participantObj = JsonConvert.DeserializeObject<ParticipantDTO>(participantJson);
        //    return participantObj;
        //}




        //[HttpPost]
        //當參數具有[FromBody]時，Web API 會使用 Content-Type 標頭來選取格式器。 
        //    在此範例中，內容類型為 「application / json」，而要求本文是原始 JSON 字串， (不是 JSON 物件) 。
        //public async Task<IActionResult> Complete([FromBody] List<Ticket> tickets)
        //{
        //    if (tickets != null)
        //    {
        //        return Json(new { success = true, message = "資料傳回後端成功!" });
        //    }

        //    return Content("資料傳回失敗");
        //}
    }
}
