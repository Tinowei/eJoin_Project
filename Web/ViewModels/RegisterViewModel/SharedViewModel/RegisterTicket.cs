namespace Web.ViewModels.RegisterViewModel.SharedViewModel
{
    public class RegisterTicket
    {
        public int TicketId { get; set; }
        public string TicketName { get; set; }
        public int Amount { get; set; }
        public decimal TicketPrice { get; set; }
        public DateTime StartSellTime { get; set; }
        public DateTime EndSellTime { get; set; }
        public DateTime TicketValidTime { get; set; }
        public int Stock {  get; set; }
        public int MaxPurchase { get; set; }
    }
}
