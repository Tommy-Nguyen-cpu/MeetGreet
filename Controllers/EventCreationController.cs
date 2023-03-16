using Microsoft.AspNetCore.Mvc;

namespace MeetGreet.Controllers
{
    public class EventCreationController : Controller
    {
        public IActionResult EventCreationPage()
        {
            return View();
        }
    }
}
