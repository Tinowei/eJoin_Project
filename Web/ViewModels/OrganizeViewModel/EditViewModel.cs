namespace Web.ViewModels.OrganizeViewModel
{
    public class EditViewModel
    {
        public int Id { get; set; }
        public List<Themes> Theme { get; set; }
        public List<int> SelectedTheme { get; set; }
        public string PictureUrl { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string? AddressDetail { get; set; }
        public string Summary { get; set; }
        public string Intro { get; set; }
        public List<Ticket> Tickets { get; set; }
        public int Status { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
    public class Ticket
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public int Stock {  get; set; }
        public decimal Price { get; set; }
        public DateTime StartSellDate { get; set; }
        public DateTime EndSellDate { get; set; }
        public int? MaxPurchase { get; set; }
    }
    public class Themes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }
}
