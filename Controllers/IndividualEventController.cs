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

namespace MeetGreet.Controllers
{
    public class IndividualEventController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MeetgreetContext _context;
        private readonly MySqlConnection connect;

        public IndividualEventController(ILogger<HomeController> logger, MeetgreetContext context, MySqlConnection connection)
        {
            _logger = logger;
            _context = context;
            connect = connection;
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


            uploadEventImages(imageByteString);

            // TO DO: SAVE OBJECT KEY IN SQL DATABASE. SEE CODE ON MEETGREET-INFRA TO FETCH A URL TO DISPLAY IMAGES.
            // ALSO, NEED TO QUERY DATABASE FOR AWS API KEY.


            ViewData["Event"] = userEvent;

            ViewData["EventImage"] = new EventImage()
            {
                imageBytes = Convert.FromBase64String(imageByteString),
            };
            return View("~/Views/IndividualEvent/IndividualEventPage.cshtml");
        }

        private string BUCKET_NAME = "meetgreet-image-store";


        private async void uploadEventImages(string byteString)
        {
            connect.Open();
            // GET S3 CLIENT FROM SQL

            // Sends a request for API KEYS FOR AWS
            MySqlCommand command = new MySqlCommand("SELECT * FROM AWSAPIKey WHERE ID=1", connect);

            IAmazonS3 s3Client = new AmazonS3Client();

            // Reads result.
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                s3Client = new AmazonS3Client(reader.GetValue(1).ToString(), reader.GetValue(2).ToString(), Amazon.RegionEndpoint.USEast1);
            }

            reader.Close();

            string objectName = generateObjectName();
            byte[] byteArray = Convert.FromBase64String(byteString);

            await UploadFileAsync(s3Client, BUCKET_NAME, objectName, "", byteArray);
        }

        private string generateObjectName()
        {
            return DateTime.Now.ToString("yyMMdd") + DateTime.Now.ToString("HH:mm:ss") + "-" + Guid.NewGuid().ToString();
        }

        /// <param name="client">An initialized Amazon S3 client object.</param>
        /// <param name="bucketName">The Amazon S3 bucket to which the object
        /// will be uploaded.</param>
        /// <param name="objectName">The object to upload.</param>
        /// <param name="filePath">The path, including file name, of the object
        /// on the local computer to upload.</param>
        /// <returns>A boolean value indicating the success or failure of the
        /// upload procedure.</returns>
        private static async Task<bool> UploadFileAsync(
            IAmazonS3 client,
            string bucketName,
            string objectName,
            string filePath,
            byte[] imageBytes)
        {
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = objectName,
                FilePath = filePath,
            };
            using (var ms = new MemoryStream(imageBytes))
            {
                request.InputStream = ms;

                var response = await client.PutObjectAsync(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine($"Successfully uploaded {objectName} to {bucketName}.");
                    return true;
                }
                else {
                    Console.WriteLine($"Could not upload {objectName} to {bucketName}.");
                    Console.WriteLine($"{response.ToString}");
                    return false;
                }
            }
        }

    }
}
