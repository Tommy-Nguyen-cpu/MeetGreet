using SendGrid.Helpers.Mail;
using SendGrid;
using MeetGreet.Models;
using MeetGreet.Data;

namespace MeetGreet.EmailClasses
{
    public class EmailClass
    {
        public async Task SendEmail(string receipientEmail, string htmlBody, MeetgreetContext context)
        {
            var client = new SendGridClient(context.EmailApikeys.First().Apikey);
            var from = new EmailAddress("MeetGreetWIT@outlook.com", "MeetGreetNotification");
            var subject = "MeetGreet Email Confirmation";
            var to = new EmailAddress(receipientEmail);
            var plainTextContent = htmlBody;
            var htmlContent = htmlBody;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
