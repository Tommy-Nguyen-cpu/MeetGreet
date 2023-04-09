using SendGrid.Helpers.Mail;
using SendGrid;
using MeetGreet.Models;
using MeetGreet.Data;
using System.Diagnostics;

namespace MeetGreet.EmailClasses
{
    public class EmailClass
    {
        /// <summary>
        /// Sends emails to recipients. This method is currently only used for email confirmation. Could be used for more things in the future.
        /// </summary>
        /// <param name="receipientEmail"> Who this email will be sent to.</param>
        /// <param name="htmlBody"> Body of email.</param>
        /// <param name="context"> Access to database.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Sends messages to all individuals who made attendance indications for a specific event.
        /// Hosts can use this method to send out mass emails.
        /// </summary>
        /// <param name="emailSender"> Event host.</param>
        /// <param name="emailHeader"> Title of email.</param>
        /// <param name="emailBody"> Email body.</param>
        /// <param name="eventID"> Id of the current event.</param>
        /// <param name="context"> Access to database.</param>
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
