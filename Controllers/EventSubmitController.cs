using MeetGreet.Models;
using Microsoft.AspNetCore.Mvc;
using SendGrid;

namespace MeetGreet.Controllers
{
    public class EventSubmitController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EventSubmitPage(string userEventName, string userDescription, string userAddress, string userCity, string userZipCode)
        {
            Event userEvent = new Event
            {
                eventName = userEventName,
                description = userDescription,
                address = userAddress,
                city = userCity,
                zipcode = userZipCode
            };
            
            HttpClient client = new HttpClient();

            //HttpResponseMessage response = await client.GetAsync("https://overpass-api.de/api/interpreter?data=[out:json];area[name=%22" + userAddress + "%22];out%20center%20;");
            //HttpResponseMessage response = await client.GetAsync("https://overpass-api.de/api/interpreter?data=[out:json];area[addr:housenumber=%22101%22, addr:street=%22Larchmont Lane%22, addr:postcode=%2202339%22, addr:city=%22Hanover%22];out%20center%20;");
            //HttpResponseMessage response = await client.GetAsync("https://overpass-api.de/api/interpreter?data=[out:json];area[addr:full=%22" + housenumber + userAddress + userZipCode + userCity + "%22];out%20center%20;");
            //HttpResponseMessage response = await client.GetAsync("https://overpass-api.de/api/interpreter?data=[out:json];node[addr:full=%22101 Larchmont Lane 02339 Hanover%22];out%20center%20;");
            HttpResponseMessage response = await client.GetAsync("https://overpass-api.de/api/interpreter?data=[out:json];area[name=%22Hanover%22]; area[addr:housenumber=%22101%22]; area[addr:street=%22Larchmont Lane%22]; area[addr:postcode=%2202339%22];out%20center%20;");
            var eventMarker = await response.Content.ReadFromJsonAsync<Addresses>();

            ViewData["EventMarker"] = eventMarker;
            ViewData["Event"] = userEvent;
            return View();
        }
    }
}
