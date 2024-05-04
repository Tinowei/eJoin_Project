namespace ApplicationCore.Models
{
    public class SendEmailDTO
    {
        public string recipientName { get; set; }
        public string recipientAddress { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
    }
}
