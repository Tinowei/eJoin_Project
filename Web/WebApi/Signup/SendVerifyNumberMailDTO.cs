namespace Web.WebApi.Signup
{
    public class SendVerifyNumberMailDTO
    {
        public string RecipientAddress { get; set; }
        public string VerifyNumber { get; set; }
    }
}
