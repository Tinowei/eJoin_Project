using Web.ViewModels.RegisterViewModel.SharedViewModel;

namespace Web.ViewModels.RegisterViewModel
{
    public class SelectTicketViewModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; }

        public string EventPictureUrl { get; set; }

        public string EventAddress { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        //可能賣兩種票價
        public List<RegisterTicket> Tickets { get; set; }
    }


    //public class Event
    //{
    //    public int EventId { get; set; }
    //    public string EventName { get; set; }

    //    public string EventPictureUrl { get; set; }

    //    public string EventAddress { get; set; }

    //    public DateTime StartTime { get; set; }
    //    public DateTime EndTime { get; set; }
    //    public List<Ticket> Tickets { get; set; }
    //}

    //public class Ticket
    //{
    //    public int TicketId { get; set; }
    //    public string TicketName { get; set; }
    //    public int Amount { get; set; }
    //    public decimal TicketPrice { get; set; }
    //    public DateTime StartSellDate { get; set; }
    //    public DateTime EndSellDate { get; set; }
    //    public DateTime TicketValidTime { get; set; }
    //}
}
