using Web.ViewModels.RegisterViewModel.SharedViewModel;

namespace Web.ViewModels.RegisterViewModel
{

    public class PaymentViewModel
    {
        //public EventDTO Event { get; set; }

        public int EventId { get; set; }
        public string EventName { get; set; }

        public string EventPictureUrl { get; set; }

        public string EventAddress { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<TicketDetailDTO> Tickets { get; set; }
    }
}
