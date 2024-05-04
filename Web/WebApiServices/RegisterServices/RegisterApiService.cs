using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System;
using Web.Services.RegisterService;
using Web.WebApi.Register;
 

namespace Web.WebApiServices.RegisterServices
{
    public class RegisterApiService
    {
        private readonly IRepository<Cart> _cartRepo;
        private readonly IRepository<CartTicket> _cartTicketRepo;
        private readonly IRepository<TicketType> _ticketTypeRepo;
        private readonly IRepository<Member> _memberRepo;
        private readonly IRepository<Event> _eventRepo;
        private readonly IOrderRepository _orderCustomRepo;
        private readonly IRepository<EcpayLog> _ecpayLogRepo;
        private readonly RegisterService _registerService;

        public RegisterApiService(IRepository<Cart> cartRepo,
            IRepository<CartTicket> cartTicketRepo,
            IRepository<TicketType> ticketTypeRepo,
            IRepository<Member> memberRepo,
            IRepository<Event> eventRepo,
            IOrderRepository orderCustomRepo,
            IRepository<EcpayLog> ecpayLogRepo,
            RegisterService registerService)
        {
            _cartRepo = cartRepo;
            _cartTicketRepo = cartTicketRepo;
            _ticketTypeRepo = ticketTypeRepo;
            _memberRepo = memberRepo;
            _eventRepo = eventRepo;
            _orderCustomRepo = orderCustomRepo;
            _ecpayLogRepo = ecpayLogRepo;
            _registerService = registerService;
        }

        public SelectTicketResponse GetSelectTicketResponse(SelectTicketRequest request, int memberId)
        {
            try
            {
                if (memberId == 0) // 未登入狀況
                {
                    throw new ArgumentException("未登入使用者操作");
                }

                var tickets = new List<CartTicket>() { };

                foreach (var t in request.Tickets)
                {
                    var ticket = new CartTicket()
                    {
                        TicketTypeId = t.TicketTypeId,
                        Quantity = t.Count,
                    };
                    tickets.Add(ticket);
                }

                var cartTarget = new Cart()
                {
                    MemberId = memberId,
                    CreateTime = DateTime.Now,
                    ExpiredTime = DateTime.Now.AddMinutes(60),
                    EventId = request.EventId,
                    CartTickets = request.Tickets.Select(t => new CartTicket()
                    {
                        TicketTypeId = t.TicketTypeId,
                        Quantity = t.Count,
                    }).ToList() // 直接操作ICollection，由EF Core的關聯功能直接做
                };

                // 因為只有做一次資料庫讀寫，所以相當於是一個transaction，失敗會被RollBack
                var savedCart = _cartRepo.Add(cartTarget); 

                return new SelectTicketResponse()
                {
                    IsDone = true,
                    CartId = savedCart.Id
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new SelectTicketResponse();
            }

        }

        /// <summary>
        /// 檢查訂單Order是否已過期
        /// true則執行CreateNewOrder(cartId)
        /// false則回傳orderId
        /// </summary>
        /// <param name="createOrderDTO"></param>
        /// <returns></returns>
        public async Task<OperationResult<int>> CheckOrderIsExist(CreateOrderDTO createOrderDTO)
        {
            OperationResult<int> result = new OperationResult<int>();
            try
            {
                var orderTarget = _orderCustomRepo.SingleOrDefault(o => o.Id == createOrderDTO.OrderId);
                if (orderTarget == null)
                {
                    return await CreateNewOrder(createOrderDTO.CartId);
                }

                // 設定回傳值內容
                result.IsSuccess = true;
                result.Message = "當前已存在訂單";
                result.Result = createOrderDTO.OrderId;

                return result;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                result.Message = $"CheckOrderIsExist過程出現錯誤，錯誤訊息：{ex.Message}";
                result.Result = -1;
                return result;
            }
        }

        private async Task<OperationResult<int>> CreateNewOrder(int cartId)
        {
            OperationResult<int> result = new OperationResult<int>();
            try
            {
                var cartTarget = await _cartRepo.SingleOrDefaultAsync(c => c.Id == cartId) ?? throw new Exception("不存在的CartId");
                var cartTicketTargets = _cartTicketRepo.List(ct => ct.CartId == cartTarget.Id);
                var ticketTypeTargets = _ticketTypeRepo.List(tt => cartTicketTargets.Select(ct => ct.TicketTypeId).Contains(tt.Id));
                var memberTarget = await _memberRepo.SingleOrDefaultAsync(m => m.Id == cartTarget.MemberId);
                var eventTarget = await _eventRepo.SingleOrDefaultAsync(e => e.Id == cartTarget.EventId);

                // 組合OrderTickets
                var savedOrderTickets = cartTicketTargets.Select(ct => new OrderTicket()
                {
                    TicketTypeName = ticketTypeTargets.First(tt => tt.Id == ct.TicketTypeId).Name,
                    PurchaseQuantity = ct.Quantity,
                    UnitPrice = ticketTypeTargets.First(tt => tt.Id == ct.TicketTypeId).UnitPrice,
                    TicketTypeId = ct.TicketTypeId,
                });

                // 組合OrderDetail
                var savedOrderDetail = new OrderDetail()
                {
                    CreateTime = DateTime.Now,
                    ParticipantName = cartTarget.ParticipantName,
                    ParticipantEmail = cartTarget.ParticipantEmail,
                    ParticipantPhone = cartTarget.ParticipantPhone,
                    TotalMoney = savedOrderTickets.Sum(sot => sot.PurchaseQuantity * sot.UnitPrice),
                    BuyerName = memberTarget.Name,
                    EventTitle = eventTarget.Title,
                    LearnedFrom = cartTarget.LearnedFrom,
                    OrderTickets = savedOrderTickets.ToList(),
                };

                // 產出不重複的OrderNo
                string orderNo = "EJ";
                while (_orderCustomRepo.Any(o => o.OrderNo == orderNo))
                {
                    var now = DateTime.Now.ToString("yyMMddHHmmss");
                    orderNo = $"EJ{now}";
                }

                // 組合Order
                var savedOrder = new Order()
                {
                    BuyerId = memberTarget.Id,
                    EventId = eventTarget.Id,
                    Status = 1,
                    ExpiredTime = DateTime.Now.AddMinutes(5),
                    OrderNo = orderNo,
                    OrderDetail = savedOrderDetail,
                };

                // 組合庫存改變後的TicketType
                var changedticketTypeTargets = ticketTypeTargets.Select(tt =>
                {
                    tt.Stock -= cartTicketTargets.First(ct => ct.TicketTypeId == tt.Id).Quantity;
                    return tt;
                });

                // 進行交易
                var returnOrder = _orderCustomRepo.CreateOrder(savedOrder, changedticketTypeTargets).Result;

                // 設定回傳值內容
                result.IsSuccess = true;
                result.Message = "建立訂單成功";
                result.Result = returnOrder.Id;

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                result.Message = $"資料庫讀寫過程出現錯誤，錯誤訊息：{ex.Message}";
                result.Result = -1;
                return result;
            }
        }

        public void SaveEcpayResult(EcpayResultDTO ecpayResult)
        {
            var ecpayLogTarget = _ecpayLogRepo.SingleOrDefault(el => el.MerchantTradeNo == ecpayResult.MerchantTradeNo);
            if (ecpayLogTarget == null)
            {
                throw new NullReferenceException("沒有對應的MerchantTradeNo訂單");
            }

            if (ecpayLogTarget.CheckMacValue != _registerService.GetCheckMacValue(ecpayResult))
            {
                // todo
            }

            ecpayLogTarget.RtnCode = ecpayResult.RtnCode;
            ecpayLogTarget.RtnMsg = ecpayResult.RtnMsg;
            ecpayLogTarget.TradeNo = ecpayResult.TradeNo;
            ecpayLogTarget.PaymentDate = Convert.ToDateTime(ecpayResult.PaymentDate);
            ecpayLogTarget.TradeDate = Convert.ToDateTime(ecpayResult.TradeDate);

            _ecpayLogRepo.Update(ecpayLogTarget);
        }
    }
}
