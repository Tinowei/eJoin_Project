namespace Web.WebApi.Organize
{
    public class EditBasicInfoDTO
    {
        public int EventId { get; set; }
        public string PictureUrl { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string AddressDetail { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
