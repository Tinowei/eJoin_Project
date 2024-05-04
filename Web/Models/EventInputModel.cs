namespace Web.Models
{
    public class EventInputModel
    {
        public string ThemeIdList { get; set; }
        public int Id { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string AddInformation { get; set; }
        public string Summary { get; set; }
        public string Introduction { get; set; }
    }
}
