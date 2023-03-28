using MeetGreet.Data;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

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

        public async Task<IActionResult> IndividualEventPage()
        {
            //working on querying database
            //await connect.OpenAsync();
            //MySqlCommand command = new MySqlCommand("SELECT * FROM ");
            return View();
        }
    }
}
