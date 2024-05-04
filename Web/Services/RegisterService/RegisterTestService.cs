using ApplicationCore.Entities;
using Web.ViewModels.RegisterViewModel;
using Web.ViewModels.RegisterViewModel.SharedViewModel;

namespace Web.Services.RegisterService
{
    public class RegisterTestService  : IRegisterService
    //處理所有Register的Action ViewModel
    {
        public SelectTicketViewModel GetSelectTicketViewModel(int EventId)
        {
            return new SelectTicketViewModel()
            {
                EventId = 1,
                EventName = "【熱銷】欸等的一小時MVC魔鬼訓練營 ",
                EventAddress = "台北市大安區忠孝東路三段96號11號樓之1",
                EventPictureUrl = "class.gif",
                StartTime = new DateTime(2024, 2, 27, 11, 00, 00),
                EndTime = new DateTime(2024, 2, 27, 12, 00, 00),
                Tickets=  new List<RegisterTicket>
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

        public FillFormViewModel GetFillFormViewModel(int EventId)
        {
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

        public PaymentViewModel GetPaymentViewModel(int EventId)
        {
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

        public int GetVaildCart(int memberId)
        {
            return 10;
        }

        public int SaveFillFormInput(Cart cart)
        {
            return 1;
        }

        public DirectToEcpayViewModel GetDirectToECPayViewModel(int orderId)
        {
            throw new NotImplementedException();
        }

        public int GetCartIdByOrderId(int userId)
        {
            throw new NotImplementedException();
        }

        public void CompleteOrder(string merchantTradeNo)
        {
            throw new NotImplementedException();
        }





        //public bool IsModelBindingSuccessful(TicketDetails tickets)
        //{
        //    if (tickets == null)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        //private User GetHost()
        //{
        //    return new User { UserId = 1, UserName = "Adam" };
        //}

        //private  Event GetEvent() //一筆資料
        //{
        //    return new Event ()
        //            {
        //                EventId = 1,
        //                EventName = "【熱銷】欸等的一小時MVC魔鬼訓練營 ",
        //                EventAddress = "台北市大安區忠孝東路三段96號11號樓之1",
        //                EventPictureUrl = "class.gif",
        //                StartTime = new DateTime(2024, 2, 27, 11, 00, 00),
        //                EndTime = new DateTime(2024, 2, 27, 12, 00, 00),
        //                Tickets = new List<Ticket>
        //                                    {
        //                                        new Ticket
        //                                        {
        //                                            TicketId = 1,
        //                                            TicketName = "2/27一般學員班",
        //                                            TicketPrice = 599,
        //                                            Amount = 5,
        //                                            TicketValidTime = new DateTime(2024, 2, 27, 12, 00, 00),
        //                                            StartSellTime = new DateTime(2024, 2, 21),
        //                                            EndSellTime = new DateTime(2024, 2, 25)
        //                                        },
        //                                        new Ticket
        //                                        {
        //                                            TicketId = 2,
        //                                            TicketName = "2/27Vip海景第一排",
        //                                            TicketPrice = 699,
        //                                            Amount = 3,
        //                                            TicketValidTime = new DateTime(2024, 2, 27, 12, 00, 00),
        //                                            StartSellTime = new DateTime(2024, 2, 21),
        //                                            EndSellTime = new DateTime(2024, 2, 25)
        //                                        }
        //                                    }

        //            };
        //}


    }
}
