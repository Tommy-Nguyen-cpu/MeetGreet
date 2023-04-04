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
            return View();
        }

        /*
         * This is where the event that a user has created will be submitted to the SQL database. The method takes in all the needed info as 
         * params and assigns them inside a model. That model is then passed to the SQL table and the data inside is pushed. The event is also
         * passed to the individual event page so that it can display the correct information
         */
        [HttpPost]
        public async Task<IActionResult> SubmitToSQL(DateTime eventDate, string eventTitle, string eventDescription, string eventAddress, string eventCity, string eventZipCode, double eventLatitude, double eventLongitude)
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
                GeoLocationLatitude= eventLatitude,
                GeoLocationLongitude= eventLongitude
            };
            //pushing data to the SQL table
            string? id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(id != null)
            {
                userEvent.CreatedByUserId = id;
                _context.Add(userEvent);
                _context.SaveChanges();
            }
            //Pass data to Individual Event Page
            ViewData["Event"] = userEvent;
            return View("~/Views/IndividualEvent/IndividualEventPage.cshtml");
        }
    }
}
