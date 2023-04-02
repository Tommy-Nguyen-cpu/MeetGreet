using MeetGreet.Data;
using MeetGreet.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Security.Claims;

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

            // Reads result.
            MySqlDataReader reader = command.ExecuteReader();

            // Iterates through all results and prints out the email address.
            //while (reader.Read())
            //{
                //System.Diagnostics.Debug.WriteLine("Test Email Retrieve: " + reader.GetValue(0).ToString());
            //}

            //Event userEvent = new Event
            //{
                //Title = reader.GetValue(1).ToString(),
                //Description = reader.GetValue(4).ToString(),
               // ScheduledDateTime = reader.GetValue(6)
            //}
            reader.Close();


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitToSQL(DateTime eventDate, string eventTitle, string eventDescription, string eventAddress, string eventCity, string eventZipCode, double eventLatitude, double eventLongitude)
        {
            Event userEvent = new Event
            {
                Title = eventTitle,
                Description = eventDescription,
                ScheduledDateTime = eventDate,
                Address = eventAddress,
                City = eventCity,
                ZipCode = eventZipCode,
                GeoLocationLatitude= eventLatitude,
                GeoLocationLongitude= eventLongitude
            };

            string? id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(id != null)
            {
                userEvent.CreatedByUserId = id;
                _context.Add(userEvent);
                _context.SaveChanges();
            }

            ViewData["Event"] = userEvent;
            return View("~/Views/IndividualEvent/IndividualEventPage.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> StatusOfUser(string userInterest)
        {
            if(userInterest == "Interested")
            {
                System.Diagnostics.Debug.WriteLine("You are interested in attending");
            }else if(userInterest == "Attending")
            {
                System.Diagnostics.Debug.WriteLine("You are attending the event");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("You are not attending the event");
            }
            return View("~/Views/EventCreation/EventCreationPageError.cshtml");
        }
    }
}
