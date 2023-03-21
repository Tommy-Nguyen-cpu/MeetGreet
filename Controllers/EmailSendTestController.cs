﻿using MeetGreet.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace MeetGreet.Controllers
{
    public class EmailSendTestController : Controller
    {

        ApplicationDbContext _context;

        public EmailSendTestController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult EmailSendTestPage()
        {
            foreach(var email in _context.Users.Select(ex => ex.UserName))
            {
                System.Diagnostics.Debug.WriteLine("Email: " + email);
            }
            return View();
        }

        internal class Example
        {
            private static void Main()
            {
                Execute().Wait();
            }

            static async Task Execute()
            {
                var apiKey = "place holder I have api Key didn't wanna push to github";
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("MeetGreetWIT@outlook.com", "Example User");
                var subject = "MeetGreet Registration";
                var to = new EmailAddress("test@example.com", "Example User");
                var plainTextContent = "and easy to do anywhere, even with C#";
                var htmlContent = "<p>Thank you for signing up for MeetGreet.com!<p>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
            }
        }
    }
}
