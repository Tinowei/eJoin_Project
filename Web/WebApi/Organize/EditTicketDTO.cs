namespace Web.WebApi.Organize
{
    public class EditTicketDTO
    {
        public int EventId { get; set; }
        public List<Ticket> TicketsData { get; set; }
    }
    public class Ticket
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public DateTime StartSellDate { get; set; }
        public DateTime EndSellDate { get; set; }
        public int? MaxPurchase { get; set; }
        public int IsDeleted { get; set; }
    }
}
