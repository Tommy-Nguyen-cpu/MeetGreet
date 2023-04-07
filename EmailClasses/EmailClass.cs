using SendGrid.Helpers.Mail;
using SendGrid;
using MeetGreet.Models;
using MeetGreet.Data;
using System.Diagnostics;

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

        public async void SendMassEmail(string emailSender, string emailHeader, string emailBody, int eventID, MeetgreetContext context)
        {
            List<string> recipients = new List<string>();
            foreach(var attendance in context.AttendingIndications)
            {
                if(attendance.EventId == eventID)
                {
                    recipients.Add(attendance.UserId);
                }
            }

            string apiKey = context.EmailApikeys.First().Apikey;
            foreach (var user in context.Users.ToList())
            {
                if (recipients.Contains(user.Id))
                {
                    var client = new SendGridClient(apiKey);
                    var from = new EmailAddress("MeetGreetWIT@outlook.com", emailSender);
                    var subject = emailHeader;
                    var to = new EmailAddress(user.Email);
                    var plainTextContent = emailBody;
                    var htmlContent = emailBody;
                    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                    var response = await client.SendEmailAsync(msg);
                }
            }
        }
    }
}
