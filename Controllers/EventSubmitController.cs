using MeetGreet.Models;
using Microsoft.AspNetCore.Mvc;

namespace MeetGreet.Controllers
{
    public class EventSubmitController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EventSubmitPage(String userEventName, string userDescription, string userAddress, string userCity, string userZipCode)
        {
            Event userEvent = new Event
            {
                eventName = userEventName,
                description = userDescription,
                address = userAddress,
                city = userCity,
                zipcode = userZipCode
            };
            return View(userEvent);
        }
    }
}
