using ApplicationCore.Interfaces;

namespace Web.WebApiServices.SignupServices
{
    public class SignupApiService
    {
        private readonly ISendEmail _sendEmail;

        public SignupApiService(ISendEmail sendEmail) 
        {
            _sendEmail = sendEmail;
        }

        public void SendVerifyEmail(string recipientAddress, string verifyNumber)
        {
            _sendEmail.SendEmail(new ApplicationCore.Models.SendEmailDTO()
            {
                recipientName = "親愛的eJoin用戶",
                recipientAddress = recipientAddress,
                subject = "eJoin註冊驗證碼",
                body = $"您的eJoin驗證碼為{verifyNumber}，請盡快回填至註冊頁面驗證碼輸入框。",
            });
        }
    }
}
