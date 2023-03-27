﻿using SendGrid.Helpers.Mail;
using SendGrid;

namespace MeetGreet.EmailClasses
{
    public class EmailClass
    {

        private readonly string apiKey = "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";
        public async Task SendEmail(string receipientEmail, string htmlBody)
        {
            var client = new SendGridClient(apiKey);
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