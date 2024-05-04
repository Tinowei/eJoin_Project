using ApplicationCore.Entities;
using ApplicationCore.Extensions;
using ApplicationCore.Interfaces;
using Web.WebApi.Ticket;
using Infrastructure.Services;
using Web.Services.AccountService;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using ReleaseTicketSummaryDTO = ApplicationCore.Models.ReleaseTicketSummaryDTO;

namespace Web.WebApiServices.TicketServices
{
    public class TicketApiService
    {
        private readonly UserContextService _userContextService;
        private readonly IQrCodeHelper _qrcodeHelper;
        private readonly IRepository<Event> _eventRepo;
        private readonly IRepository<TicketType> _ticketTypeRepo;
        private readonly IReleaseTicketQueryService _releaseTicketQueryService;
        private readonly IReleaseTicketRepository _releaseTicketRepo;
        private readonly IOrderDetailQueryService _orderDetailQueryService;

        public TicketApiService(IQrCodeHelper qrcodeHelper,
                                IReleaseTicketQueryService releaseTicketQueryService,
                                IRepository<Event> eventRepo,
                                IRepository<TicketType> ticketTypeRepo,
                                UserContextService userContextService, 
                                IReleaseTicketRepository releaseTicketRepo,
                                IOrderDetailQueryService orderDetailQueryService)
        {
            _qrcodeHelper = qrcodeHelper;
            _releaseTicketQueryService = releaseTicketQueryService;
            _eventRepo = eventRepo;
            _ticketTypeRepo = ticketTypeRepo;
            _userContextService = userContextService;
            _releaseTicketRepo = releaseTicketRepo;
            _orderDetailQueryService = orderDetailQueryService;
        }
        
        /// <summary>
        /// 取得當前用戶的所有已發行票券
        /// </summary>
        /// <returns></returns>
        public List<ReleaseTicketSummaryDTO> GetTicketsFromDB(int page,int pageSize)
        {
            //取得當前登錄者Id
            var userId =_userContextService.GetUserId();
            //取得eventId
            var releaseTicketTargets = _releaseTicketQueryService.List(rt => rt.MemberId == userId && rt.Status==0);
            
            var result = _releaseTicketQueryService.GetReleaseTicketSummaries(userId,page,pageSize);
            
            Console.WriteLine(result);
            return result;
        }
        
        /// <summary>
        /// 產生包含相對應票券編號的qrcode
        /// </summary>
        /// <param name="selectedNumber"></param>
        /// <param name="eventId"></param>
        /// <param name="ticketTypeId"></param>
        /// <returns></returns>
        public QrCodeResult CreateQrCode(int selectedNumber,int eventId,int ticketTypeId)
        {
            if (IsEnough(selectedNumber,ticketTypeId,eventId))
            {
                var memberId = _userContextService.GetUserId();
                var targetResult = _releaseTicketQueryService
                    .GetTicketsByNumber(selectedNumber, eventId, ticketTypeId,memberId)
                    .Select(rt => rt.ReleaseTicketNumber);
                
                var qrcodeTestImg = _qrcodeHelper.GenerateQrCodeByQuantity(targetResult);
                string imageBase64Data = Convert.ToBase64String(qrcodeTestImg);
                string imageDataUrl = $"data:image/png;base64,{imageBase64Data}";
                
                return new QrCodeResult { Success = true, ImageDataUrl = imageDataUrl };
            }
            return new QrCodeResult { Success = false, ErrorMessage = "庫存不足，無法兌換！" };
        }
        
        /// <summary>
        /// 驗證欲兌換數量是否有小於等於當前庫存
        /// </summary>
        /// <param name="selectedNumber"></param>
        /// <param name="ticketTypeId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        private bool IsEnough(int selectedNumber,int ticketTypeId,int eventId)
        {
            //todo: 進資料庫查詢該用戶總共有幾張相同的票券，而且選擇數量要小於等於他的總庫存
            var memberId = _userContextService.GetUserId();
            var ticketInStock = _releaseTicketQueryService.GetTicketCount(memberId,eventId, ticketTypeId);
            if (ticketInStock >= selectedNumber)
            {
                return true;
            }
            return false;
        }
        
        
        /// <summary>
        /// 找到該票券編號所屬的活動主辦方id
        /// </summary>
        /// <param name="StringTicketNumbers"></param>
        /// <returns></returns>
        public bool IsValidHostMemberId(string StringTicketNumbers)
        {
            int eventHostId;

            var ticketNumber = GetFirstTicketNumber(StringTicketNumbers);
            // 無有效票券編號，返回false
            if (string.IsNullOrEmpty(ticketNumber))
            {
                return false;
            }
            eventHostId = _releaseTicketQueryService.GetTicketHostId(ticketNumber);
            
            //從cookie，取得目前登錄者的id
            var userId = _userContextService.GetUserId();
            return userId == eventHostId ; //條件是登錄者必須等於活動主辦方
        }
        
        /// <summary>
        /// 拆出第一個票券編號，之後會拿來查詢該票券的活動主辦方id
        /// </summary>
        /// <param name="StringTicketNumbers"></param>
        /// <returns></returns>
        private string GetFirstTicketNumber(string StringTicketNumbers)
        {
            //若票券編號含有$字符號，就將它拆分
            if (StringTicketNumbers.Contains("$"))
            {
                var ticketNumberList = StringTicketNumbers.Split("$");
                if (ticketNumberList.Length > 0)
                {
                    return ticketNumberList[0];
                }
            }
            return StringTicketNumbers;
        }

        
        /// <summary>
        /// 核銷傳進來的票券編號
        /// </summary>
        /// <param name="stringTicketNumbers"></param>
        /// <returns></returns>
        public async Task<(List<ReleaseTicket>,List<string> ErrorMessages)> VerifyTargetTickets(string stringTicketNumbers)
        {
            var ticketNumberList = SplitTicketNumbers(stringTicketNumbers);
            var verifiedTickets = new List<ReleaseTicket>();
            var errorMessages = new List<string>();
            
            try
            {
                var result = await _releaseTicketRepo.VerifyTickets(ticketNumberList);
                verifiedTickets = result.Item1;
                errorMessages = result.Item2;
            }
            catch (Exception ex)
            {
                errorMessages.Add($"票券驗證過程中出現錯誤：{ex.Message}");
            }
            
            return (verifiedTickets,errorMessages);
        }

        /// <summary>
        /// 若傳進來的stringTicketNumbers含有$，將票券編號做拆分
        /// </summary>
        /// <param name="stringTicketNumbers"></param>
        /// <returns></returns>
        private List<string> SplitTicketNumbers(string stringTicketNumbers)
        {
            //若票券編號含有$字符號，就將它拆分
            if (stringTicketNumbers.Contains("$"))
            {
                var ticketNumberList = stringTicketNumbers.Split("$",StringSplitOptions.RemoveEmptyEntries);
                return ticketNumberList.ToList();
            }
            return new List<string> { stringTicketNumbers };
        }

        public async Task<ActionResult<IEnumerable<UsedTicketsSummaryDTO>>> GetUsedTicketsFromDB(int page,int pageSize)
        {
            var userId = _userContextService.GetUserId();
            var result =  _releaseTicketQueryService.GetUsedTickets(userId,page,pageSize);
            return result;

        }
        
        /// <summary>
        /// 取得未使用的已發行票券總數
        /// </summary>
        /// <returns></returns>
        public int GetMemberTicketCounts()
        {
            var userId = _userContextService.GetUserId();
            var result = _releaseTicketQueryService.GetTicketsCount(userId);
            return result;
        }
        
        /// <summary>
        /// 取得已使用的已發行票券總數
        /// </summary>
        /// <returns></returns>
        public int GetUsedTicketCounts()
        {
            var userId = _userContextService.GetUserId();
            var result = _releaseTicketQueryService.GetUsedTicketsCount(userId);
            return result;
        }
        
        /// <summary>
        /// 依據分頁分批拿取購買記錄
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public async Task<PageOrder> GetOrderDetailsByPagedList(int currentPage)
        {
            var userId = _userContextService.GetUserId();
            
            var orderParameters = new OrderDetailParameters()
            {
                MemberId = userId,
                PageNumber = currentPage,
                PageSize = 3
            };
            var pageOrderDetailSummaries =await _orderDetailQueryService.GetOrderListByPageAsync(orderParameters);

            var result = new PageOrder()
            {
                Orders = pageOrderDetailSummaries.Items,
                TotalPage = pageOrderDetailSummaries.TotalPages,
                CurrentPage = pageOrderDetailSummaries.CurrentPage
            };

            return result;
        }
        
        
        //test用
        // public async Task<IPagedList<ReleaseTicketSummaryDTO>> GetReleaseTicketByPagedList(int currentPage)
        // {
        //     var userId = _userContextService.GetUserId();
        //     var releaseTicketParameters = new ReleaseTicketParameters()
        //     {
        //         MemberId = userId,
        //         PageNumber = currentPage,
        //         PageSize = 3
        //     };
        //     //取得當前分頁的筆數
        //     var pagedReleaseTickets = await _releaseTicketQueryService.GetReleaseTicketListByPageAsync(releaseTicketParameters);
        //
        //     return pagedReleaseTickets;
        // }
        
    }

    public class QrCodeResult
    {
        public bool Success { get; set; }
        public string? ImageDataUrl { get; set; }
        public string? ErrorMessage { get; set; }
    }


    public class PageOrder
    {
        public List<OrderDetailSummaryDTO> Orders { get; set; }

        public int TotalPage { get; set; }
        
        public int CurrentPage { get; set; }
        
    }
}


