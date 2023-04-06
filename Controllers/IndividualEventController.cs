using MeetGreet.Data;
using MeetGreet.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Security.Claims;
using System;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Extensions.NETCore.Setup;
using static Amazon.RegionEndpoint;
using Microsoft.Extensions.Logging;
using NuGet.Protocol.Core.Types;
using MeetGreet.AmazonS3HelperClasses;
using System.Security.AccessControl;

namespace MeetGreet.Controllers
{
    public class IndividualEventController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MeetgreetContext _context;
        private readonly MySqlConnection connect;
        private readonly AmazonS3Helper s3Helper;

        public IndividualEventController(ILogger<HomeController> logger, MeetgreetContext context, MySqlConnection connection, AmazonS3Helper amazonS3Helper)
        {
            _logger = logger;
            _context = context;
            connect = connection;

            s3Helper = amazonS3Helper;
        }

        [HttpPost]
        public async Task<IActionResult> IndividualEventPage(int eventID)
        {
            await connect.OpenAsync();
            // Sends a request for email in table "user".
            MySqlCommand command = new MySqlCommand("SELECT * FROM Event WHERE ID="+ eventID, connect);

            Event userEvent = new Event();

            // Reads result.
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                userEvent.Title = reader.GetValue(1).ToString();
                userEvent.Description = reader.GetValue(4).ToString();
                userEvent.ScheduledDateTime = DateTime.Parse(reader.GetValue(10).ToString());
                userEvent.Address = reader.GetValue(9).ToString();
                userEvent.City = reader.GetValue(8).ToString();
                userEvent.ZipCode = reader.GetValue(7).ToString();
                userEvent.GeoLocationLatitude = Convert.ToDouble(reader.GetValue(6).ToString());
                userEvent.GeoLocationLongitude = Convert.ToDouble(reader.GetValue(5).ToString());
            }

            //reader.Close();

            ViewData["Event"] = userEvent;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitToSQL(DateTime eventDate, string eventTitle, string eventDescription, string eventAddress, string eventCity, string eventZipCode, double eventLatitude, double eventLongitude, string imageByteString, string eventImageName)
        {
            Event userEvent = new Event
            {
                Title = eventTitle,
                Description = eventDescription,
                ScheduledDateTime = eventDate,
                Address = eventAddress,
                City = eventCity,
                ZipCode = eventZipCode,
                GeoLocationLatitude = eventLatitude,
                GeoLocationLongitude = eventLongitude
            };

            string? id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id != null)
            {
                userEvent.CreatedByUserId = id;
                _context.Add(userEvent);
                _context.SaveChanges();
            }


            string objectID = await s3Helper.uploadImageToS3Bucket(Convert.FromBase64String(imageByteString), userEvent);

            if(objectID == "ERROR")
            {
                Console.Write("ERROR: The image could not be saved to S3. It will not be written to the MYSQL Server.");

            }
            else {
                saveImageToDatabase(userEvent, objectID);
            }

            ViewData["Event"] = userEvent;
            ViewData["EventImage"] = new EventImage()
            {
                imageBytes = Convert.FromBase64String(imageByteString),
            };

            return View("~/Views/IndividualEvent/IndividualEventPage.cshtml");
        }

        private string BUCKET_NAME = "meetgreet-image-store";


        private void saveImageToDatabase(Event userEvent, String s3Key)
        {
            Image image = new Image()
            {
                EventId = userEvent.Id,
                S3key = s3Key
            };
            _context.Add(image);
            _context.SaveChanges();
        }
    }
}
