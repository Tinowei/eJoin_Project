using ApplicationCore.Models;

namespace ApplicationCore.Interfaces
{
    public interface ISendEmail
    {
        void SendEmail(SendEmailDTO sendEmail);
    }
}
