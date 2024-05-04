using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace Infrastructure.Services
{
    public class SendEmailMailKit : ISendEmail
    {
        public void SendEmail(SendEmailDTO sendMail)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("eJoinOffical", "ejoinbs@gmail.com"));
            message.To.Add(new MailboxAddress(sendMail.recipientName, sendMail.recipientAddress));
            message.Subject = sendMail.subject;
            message.Body = new TextPart("plain") { Text = sendMail.body };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate("ejoinbs@gmail.com", "bszmlxmjvbiqhoel");
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
