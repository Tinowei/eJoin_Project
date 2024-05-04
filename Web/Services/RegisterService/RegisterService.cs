using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System.Text;
using System.Web;
using Web.Services.AccountService;
using Web.ViewModels.RegisterViewModel;
using Web.ViewModels.RegisterViewModel.SharedViewModel;

namespace Web.Services.RegisterService
{
#nullable disable
    public class RegisterService : IRegisterService
    {
        private readonly IRepository<Event> _eventRepo;
        private readonly IRepository<TicketType> _ticketTypeRepo;
        private readonly IRepository<Cart> _cartRepo;
        private readonly IRepository<CartTicket> _cartTicketRepo;
        private readonly IRepository<Order> _orderRepo;
        private readonly IRepository<OrderDetail> _orderDetailRepo;
        private readonly IRepository<OrderTicket> _orderTicketRepo;
        private readonly IRepository<EcpayLog> _ecpayLogRepo;
        private readonly IConfiguration _configuration;
        private readonly IRepository<ReleaseTicket> _releaseTicketRepo;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterService(IRepository<Event> eventRepo,
            IRepository<TicketType> ticketTypeRepo,
            IRepository<Cart> cartRepo,
            IRepository<CartTicket> cartTicketRepo,
            IRepository<Order> orderRepo,
            IRepository<OrderDetail> orderDetailRepo,
            IRepository<OrderTicket> orderTicketRepo,
            IRepository<EcpayLog> ecpayLogRepo,
            IConfiguration configuration,
            IRepository<ReleaseTicket> releaseTicketRepo,
            IUnitOfWork unitOfWork)
        {
            _eventRepo = eventRepo;
            _ticketTypeRepo = ticketTypeRepo;
            _cartRepo = cartRepo;
            _cartTicketRepo = cartTicketRepo;
            _orderRepo = orderRepo;
            _orderDetailRepo = orderDetailRepo;
            _orderTicketRepo = orderTicketRepo;
            _ecpayLogRepo = ecpayLogRepo;
            _configuration = configuration;
            _releaseTicketRepo = releaseTicketRepo;
            _unitOfWork = unitOfWork;
        }

        public SelectTicketViewModel GetSelectTicketViewModel(int eventId)
        {
            try
            {
                Event eventTarget = _eventRepo.GetById(eventId);
                IEnumerable<TicketType> ticketTypeTarget = _ticketTypeRepo.List(t => t.EventId == eventId);

                var ticketTypes = ticketTypeTarget.Select(t => new RegisterTicket
                {
                    TicketId = t.Id,
                    TicketName = t.Name,
                    TicketPrice = t.UnitPrice,
                    Amount = t.ReleaseAmount,
                    StartSellTime = t.StartSellTime,
                    EndSellTime = t.EndSellTime,
                    TicketValidTime = t.StartSellTime,
                    Stock = t.Stock,
                    MaxPurchase = t.MaxPurchase ?? 0,
                }).ToList();


                return new SelectTicketViewModel
                {
                    EventId = eventTarget.Id,
                    EventName = eventTarget.Title,
                    EventAddress = eventTarget.Address,
                    EventPictureUrl = eventTarget.CoverUrl,
                    StartTime = eventTarget.StartTime,
                    EndTime = eventTarget.EndTime,
                    Tickets = ticketTypes
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new SelectTicketViewModel()
                {
                    EventId = 1,
                    EventName = "【熱銷】欸等的一小時MVC魔鬼訓練營 ",
                    EventAddress = "台北市大安區忠孝東路三段96號11號樓之1",
                    EventPictureUrl = "class.gif",
                    StartTime = new DateTime(2024, 2, 27, 11, 00, 00),
                    EndTime = new DateTime(2024, 2, 27, 12, 00, 00),
                    Tickets = new List<RegisterTicket>
                                    {
                                        new RegisterTicket
                                        {
                                            TicketId = 1,
                                            TicketName = "2/27一般學員班",
                                            TicketPrice = 599,
                                            Amount = 5,
                                            TicketValidTime = new DateTime(2024, 2, 27, 12, 00, 00),
                                            StartSellTime = new DateTime(2024, 2, 21),
                                            EndSellTime = new DateTime(2024, 2, 25)
                                        },
                                        new RegisterTicket
                                        {
                                            TicketId = 2,
                                            TicketName = "2/27Vip海景第一排",
                                            TicketPrice = 699,
                                            Amount = 3,
                                            TicketValidTime = new DateTime(2024, 2, 27, 12, 00, 00),
                                            StartSellTime = new DateTime(2024, 2, 21),
                                            EndSellTime = new DateTime(2024, 2, 25)
                                        },
                                        new RegisterTicket
                                        {
                                            TicketId = 3,
                                            TicketName = "免費票",
                                            TicketPrice = 0,
                                            Amount = 3,
                                            TicketValidTime = new DateTime(2024, 2, 27, 12, 00, 00),
                                            StartSellTime = new DateTime(2024, 2, 21),
                                            EndSellTime = new DateTime(2024, 2, 25)
                                        }
                                    }
                };
            }
        }

        public FillFormViewModel GetFillFormViewModel(int cartId)
        {
            try
            {
                if (cartId == 0)
                {
                    throw new Exception("cartId為0");
                }
                var cartTarget = _cartRepo.GetById(cartId);
                var eventTarget = _eventRepo.GetById(cartTarget.EventId);

                return new FillFormViewModel()
                {
                    EventId = eventTarget.Id,
                    CartId = cartTarget.Id,
                    EventName = eventTarget.Title,
                    EventAddress = eventTarget.Address,
                    EventPictureUrl = eventTarget.CoverUrl,
                    StartTime = eventTarget.StartTime,
                    EndTime = eventTarget.EndTime,
                    ParticipantName = cartTarget.ParticipantName ?? string.Empty,
                    Email = cartTarget.ParticipantEmail ?? string.Empty,
                    PhoneNumber = cartTarget.ParticipantPhone ?? string.Empty,
                    LearnFrom = cartTarget.LearnedFrom ?? string.Empty,
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //DB無資料的情況下帶入假資料，防止報錯
                return new FillFormViewModel()
                {
                    EventId = 1,
                    EventName = "【熱銷】欸等的一小時MVC魔鬼訓練營 ",
                    EventAddress = "台北市大安區忠孝東路三段96號11號樓之1",
                    EventPictureUrl = "class.gif",
                    StartTime = new DateTime(2024, 2, 27, 11, 00, 00),
                    EndTime = new DateTime(2024, 2, 27, 12, 00, 00),
                };
            }
        }

        public PaymentViewModel GetPaymentViewModel(int cartId)
        {
            try
            {
                var cartTicketTargets = _cartTicketRepo.List(ct => ct.CartId == cartId);
                var ticketTypeTargets = _ticketTypeRepo.List(tt => cartTicketTargets.Select(ct => ct.TicketTypeId).Contains(tt.Id));
                var eventTarget = _eventRepo.SingleOrDefault(e => e.Id == ticketTypeTargets[0].EventId);

                var result = new PaymentViewModel()
                {
                    EventId = eventTarget.Id,
                    EventName = eventTarget.Title,
                    EventAddress = eventTarget.Address,
                    EventPictureUrl = eventTarget.CoverUrl,
                    StartTime = eventTarget.StartTime,
                    EndTime = eventTarget.EndTime,
                    Tickets = cartTicketTargets.Select(ct => new TicketDetailDTO()
                    {
                        TicketName = ticketTypeTargets.Single(tt => tt.Id == ct.TicketTypeId).Name,
                        Price = ticketTypeTargets.Single(tt => tt.Id == ct.TicketTypeId).UnitPrice,
                        Count = ct.Quantity,
                        StartValidTime = eventTarget.StartTime,
                        EndValidTime = eventTarget.EndTime,
                        SubTotal = ct.Quantity * ticketTypeTargets.Single(tt => tt.Id == ct.TicketTypeId).UnitPrice,
                    }).ToList(),
                };

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"出現錯誤:{e}");
                return new PaymentViewModel()
                {
                    EventId = 1,
                    EventName = "【熱銷】欸等的一小時MVC魔鬼訓練營 ",
                    EventAddress = "台北市大安區忠孝東路三段96號11號樓之1",
                    EventPictureUrl = "class.gif",
                    StartTime = new DateTime(2024, 2, 27, 11, 00, 00),
                    EndTime = new DateTime(2024, 2, 27, 12, 00, 00),
                    Tickets = new List<TicketDetailDTO>
                                    {
                                        new TicketDetailDTO
                                        {

                                            TicketName = "2/27一般學員班",
                                            Price = 599,
                                            Count = 5,
                                            StartValidTime = new DateTime(2024, 2, 21),
                                            EndValidTime = new DateTime(2024, 2, 25),
                                            SubTotal=2995
                                        },
                                        new TicketDetailDTO
                                        {
                                            TicketName = "2/27Vip海景第一排",
                                            Price = 699,
                                            Count = 5,
                                            StartValidTime = new DateTime(2024, 2, 21),
                                            EndValidTime = new DateTime(2024, 2, 25),
                                            SubTotal=3495
                                        }
                                    }
                };
            }
        }

        public DirectToEcpayViewModel GetDirectToECPayViewModel(int orderId)
        {
            try
            {
                var orderTarget = _orderRepo.GetById(orderId);
                var orderDetailTarget = _orderDetailRepo.GetById(orderId);
                var orderTicketTargets = _orderTicketRepo.List(ot => ot.OrderDetailId == orderId);

                var merchantTradeNo = GetMerchantTradeNo(orderTarget);
                var itemName = GetItemName(orderTicketTargets);

                var result = new DirectToEcpayViewModel()
                {
                    MerchantID = _configuration.GetValue<string>("Ecpay:MerchantID") ?? "3002607",
                    MerchantTradeNo = merchantTradeNo,
                    MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                    PaymentType = "aio",
                    TotalAmount = (int)orderDetailTarget.TotalMoney,
                    TradeDesc = "eJoin交易",
                    ItemName = itemName,
                    ReturnURL = _configuration.GetValue<string>("Ecpay:ReturnURL"),
                    ChoosePayment = "ALL",
                    CheckMacValue = string.Empty,
                    EncryptType = 1,
                    OrderResultURL = _configuration.GetValue<string>("Ecpay:OrderResultURL"),
                    CustomField1 = orderId.ToString(),
                };

                var checkMacValue = GetCheckMacValue(result);
                result.CheckMacValue = checkMacValue;

                var savedEcpayLog = new EcpayLog()
                {
                    RelateOrderId = orderId,
                    MerchantTradeNo = merchantTradeNo,
                    TotalAmount = (int)orderDetailTarget.TotalMoney,
                    ItemName = itemName,
                    MerchantTradeDate = DateTime.Now,
                    CheckMacValue = checkMacValue,
                };

                _ecpayLogRepo.Add(savedEcpayLog);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public int GetVaildCart(int memberId)
        {
            try
            {
                var cart = _cartRepo.SingleOrDefault(c => c.MemberId == memberId);
                if (cart == null)
                {
                    return 0;
                }

                if (cart.ExpiredTime < DateTime.Now)
                {
                    var cartTickets = _cartTicketRepo.List(t => t.CartId == cart.Id);

                    //cart.CartTickets = cartTickets; // 這樣做會報錯
                    //_cartRepo.Delete(cart); 

                    _cartTicketRepo.DeleteRange(cartTickets);
                    _cartRepo.Delete(cart); // 這樣沒有transaction可以RollBack

                    return 0;
                }
                else
                {
                    return cart.Id;
                }
            }
            catch (InvalidOperationException ex)
            {
                // 如果超過一筆符合會進入這個EX，這代表有刻意的操作出現了造成不合規的狀況發生
                Console.WriteLine($"Cart資料表中有Member出現超過一個購物車!!!以下為錯誤訊息：\n {ex}");
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }

        }

        public int SaveFillFormInput(Cart cart)
        {
            try
            {
                Console.WriteLine(cart.Id);
                var cartTarget = _cartRepo.GetById(cart.Id);
                cartTarget.ParticipantName = cart.ParticipantName;
                cartTarget.ParticipantEmail = cart.ParticipantEmail;
                cartTarget.ParticipantPhone = cart.ParticipantPhone;
                cartTarget.LearnedFrom = cart.LearnedFrom;

                return _cartRepo.Update(cartTarget).Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }

        public string GetCheckMacValue<T>(T input)
        {
            // Step 1: Sort the properties by their names
            var sortedProperties = input.GetType().GetProperties()
                .Where(p => p.Name != "CheckMacValue")
                .Where(p => p.GetValue(input) != null)
                .OrderBy(p => p.Name)
                .Select(p => $"{p.Name}={p.GetValue(input)?.ToString() ?? string.Empty}")
                .ToList();

            // Step 2: Concatenate the properties
            var concatenatedString = string.Join("&", sortedProperties);

            // Step 3: Prepend and append HashKey and HashIV
            string hashKey = _configuration.GetValue<string>("Ecpay:HashKey");
            string hashIV = _configuration.GetValue<string>("Ecpay:HashIV");
            concatenatedString = $"HashKey={hashKey}&{concatenatedString}&HashIV={hashIV}";

            // Step 4: URL encode the string
            concatenatedString = HttpUtility.UrlEncode(concatenatedString);

            // Step 5: Convert the string to lowercase
            concatenatedString = concatenatedString.ToLower();

            // Step 6: Hash the string using SHA256
            using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(concatenatedString));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                // Step 7: Convert the hash to uppercase
                return sb.ToString().ToUpper();
            }
        }

        private string GetItemName(IEnumerable<OrderTicket> orderTicketTargets)
        {
            return string.Join("#", orderTicketTargets.Select(ot => ot.TicketTypeName + $"*{ot.PurchaseQuantity}"));
        }

        private string GetMerchantTradeNo(Order order)
        {
            return Guid.NewGuid().ToString("").Replace("-", string.Empty).Substring(15);
        }

        public int GetCartIdByOrderId(int orderId)
        {
            
            var cartTarget = _cartRepo.SingleOrDefault(c => c.MemberId == (_orderRepo.GetById(orderId).BuyerId));
            return cartTarget?.Id ?? 0;
        }

        public void CompleteOrder(string merchantTradeNo)
        {
            try
            {
                _unitOfWork.Begin();

                var ecpayLogTarget = _unitOfWork.GetRepository<EcpayLog>().SingleOrDefault(el => el.MerchantTradeNo == merchantTradeNo) ?? throw new NullReferenceException("找不到對應的EcpayLog");
                var orderTarget = _unitOfWork.GetRepository<Order>().SingleOrDefault(o => o.Id == ecpayLogTarget.RelateOrderId) ?? throw new NullReferenceException("找不到對應的Order");

                var savedOrder = ChangeOrderStatus(orderTarget);

                var orderDetailTarget = _orderDetailRepo.SingleOrDefault(od => od.Id == savedOrder.Id) ?? throw new NullReferenceException("找不到對應的OrderDetial");
                var orderTicketTargets = _orderTicketRepo.List(ot => ot.OrderDetailId == orderDetailTarget.Id) ?? throw new NullReferenceException("找不到對應的OrderTickets");

                CreateReleaseTicket(savedOrder, orderDetailTarget, orderTicketTargets);
                DeleteCart(savedOrder.BuyerId);

                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                Console.WriteLine(ex);
            }
        }

        private Order ChangeOrderStatus(Order order)
        {
            order.Status = 2;
            return _unitOfWork.GetRepository<Order>().Update(order);
        }

        private void CreateReleaseTicket(Order order, OrderDetail orderDetail, List<OrderTicket> orderTickets)
        {
            var result = new List<ReleaseTicket>();
            var e = _unitOfWork.GetRepository<Event>().GetById(order.EventId).EndTime;

            foreach (var ot in orderTickets)
            {
                for (var i = 0; i < ot.PurchaseQuantity; i++)
                {
                    var rt = new ReleaseTicket()
                    {
                        TicketTypeId = ot.TicketTypeId ?? 0,
                        EventId = order.EventId,
                        MemberId = order.BuyerId,
                        ReleaseTicketNumber = Guid.NewGuid().ToString(),
                        OrderId = order.Id,
                        ParticipantName = orderDetail.ParticipantName,
                        ParticipantEmail = orderDetail.ParticipantEmail,
                        ParticipanPhone = orderDetail.ParticipantPhone,
                        Status = 0,
                        ExpireTime = e,
                    };
                    result.Add(rt);
                }
            }

            _unitOfWork.GetRepository<ReleaseTicket>().AddRange(result);
        }

        private void DeleteCart(int memberId)
        {
            var repo = _unitOfWork.GetRepository<Cart>();
            var cartTarget = repo.SingleOrDefault(c => c.MemberId == memberId);
            var cartTicketTargets = _unitOfWork.GetRepository<CartTicket>().List(ct => ct.CartId == cartTarget.Id);

            _unitOfWork.GetRepository<CartTicket>().DeleteRange(cartTicketTargets);
            _unitOfWork.GetRepository<Cart>().Delete(cartTarget);
        }
    }
}
