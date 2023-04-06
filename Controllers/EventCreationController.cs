using Microsoft.AspNetCore.Mvc;

namespace MeetGreet.Controllers
{
    public class EventCreationController : Controller
    {
        /*
         * Easiest controller just returns the event creation page.
         */
        public IActionResult EventCreationPage()
        {
            return View();
        }
    }
}
