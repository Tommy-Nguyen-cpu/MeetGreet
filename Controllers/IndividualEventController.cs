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
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        /*This Task is meant as the method that a user will use to see an individual event page.
         * The eventID is passed into the method as a param so that we can query the SQL table.
         * As the data is read in from the SQL it is assigned to the event model so it can be passed to the 
         * individual event page.
         */
        [HttpPost]
        public async Task<IActionResult> IndividualEventPage(int eventID)
        {
            await connect.OpenAsync();
            // Sends a request for all data related to event with the same ID
            MySqlCommand command = new MySqlCommand("SELECT * FROM Event WHERE ID="+ eventID, connect);
            //Create empty event model
            Event userEvent = new Event();

            // Reads result.
            MySqlDataReader reader = command.ExecuteReader();
            //The return values from the SQL are always in the same order so grabbing specific values just means grabbing a certain value
            while (reader.Read())
            {
                userEvent.Id = Convert.ToInt32(reader.GetValue(0).ToString());
                userEvent.Title = reader.GetValue(1).ToString();
                userEvent.Description = reader.GetValue(4).ToString();
                userEvent.ScheduledDateTime = DateTime.Parse(reader.GetValue(10).ToString());
                userEvent.Address = reader.GetValue(9).ToString();
                userEvent.City = reader.GetValue(8).ToString();
                userEvent.ZipCode = reader.GetValue(7).ToString();
                userEvent.GeoLocationLatitude = Convert.ToDouble(reader.GetValue(6).ToString());
                userEvent.GeoLocationLongitude = Convert.ToDouble(reader.GetValue(5).ToString());
            }

            reader.Close();
            //pass the data as a ViewData object to the view
            ViewData["Event"] = userEvent;
            ViewData["ImageURL"] = getImageURL(userEvent.Id);
            return View();
        }

        /*
         * This is where the event that a user has created will be submitted to the SQL database. The method takes in all the needed info as 
         * params and assigns them inside a model. That model is then passed to the SQL table and the data inside is pushed. The event is also
         * passed to the individual event page so that it can display the correct information
         */
        [HttpPost]
        public async Task<IActionResult> SubmitToSQL(DateTime eventDate, string eventTitle, string eventDescription, string eventAddress, string eventCity, string eventZipCode, double eventLatitude, double eventLongitude, string imageByteString, string eventImageName)
        {
            //assign params to values in event model
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
            //pushing data to the SQL table
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
            ViewData["ImageURL"] = getImageURL(userEvent.Id);

            return View("~/Views/IndividualEvent/IndividualEventPage.cshtml");
        }

        private string getImageURL(int eventID) {
            if(connect.State != System.Data.ConnectionState.Open)
            {
                connect.Open();
            }
            MySqlCommand command = new MySqlCommand("SELECT * FROM Image WHERE EventId=" + eventID, connect);

            // Reads result.
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("READER: " + reader.GetValue(2));
                string s3Key = reader.GetValue(2).ToString() ?? "";
                return s3Helper.retrieveS3BucketImageURL(s3Key);
            }
            reader.Close();
            return "";
        }

        private void saveImageToDatabase(Event userEvent, string s3Key)
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
