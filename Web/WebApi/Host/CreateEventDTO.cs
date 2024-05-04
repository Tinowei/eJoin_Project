namespace Web.WebApi.Host
{
    public class CreateEventDTO
    {
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string City { get; set; } 
        public string Address { get; set; }
        public string? AddressDetail { get; set; }
        public string Summary { get; set; }
        public string Intro { get; set; } 
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public List<int> SelectedThemes { get; set; }
        public List<Ticket> TicketsData { get; set; }
    }
    public class Ticket
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public DateTime StartSellDate { get; set; }
        public DateTime EndSellDate { get; set; }
        public int? MaxPurchase { get; set; }
        public int IsDeleted { get; set; }
    }
}
