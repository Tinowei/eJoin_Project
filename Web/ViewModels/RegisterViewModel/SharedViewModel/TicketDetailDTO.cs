namespace Web.ViewModels.RegisterViewModel.SharedViewModel
{
    public class TicketDetailDTO
    {
        public string TicketName { get; set; }

        public DateTime StartValidTime { get; set; }
        public DateTime EndValidTime { get; set; }

        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get; set; }
    }
}
