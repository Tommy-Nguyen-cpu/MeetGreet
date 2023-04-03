using MeetGreet.Data;
using MeetGreet.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Security.Claims;
using System;

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
                userEvent.Title = reader.GetValue(10).ToString();
                userEvent.Description = reader.GetValue(5).ToString();
                userEvent.ScheduledDateTime = DateTime.Parse(reader.GetValue(9).ToString());
                userEvent.Address = reader.GetValue(1).ToString();
                userEvent.City = reader.GetValue(2).ToString();
                userEvent.ZipCode = reader.GetValue(11).ToString();
                userEvent.GeoLocationLatitude = Convert.ToDouble(reader.GetValue(6).ToString());
                userEvent.GeoLocationLongitude = Convert.ToDouble(reader.GetValue(7).ToString());
            }

            reader.Close();

            ViewData["Event"] = userEvent;
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
    }
}
