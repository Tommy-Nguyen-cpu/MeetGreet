using MeetGreet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using NuGet.Protocol;
using SendGrid;
using System.Runtime.InteropServices;
using System.Text.Json.Nodes;
using static System.Net.Mime.MediaTypeNames;

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
                EventName = userEventName,
                EventDescription = userDescription,
                EventAddress = userAddress,
                EventCity = userCity,
                EventZipcode = userZipCode
            };
            
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync("http://localhost:8080/search/%20" + userAddress + "%20" + userCity + "%20" + userZipCode + "?format=json&limit=1");
            string httpResponse = await response.Content.ReadAsStringAsync();
            //thow an expception for improper query to api to prevent improper data being displayed to user in EventSubmitView
            if(httpResponse == null)
            {
                throw new Exception();
            }
            string[] testing = httpResponse.Split(',', '"');
            
            double latitude = Convert.ToDouble(testing[34]);
            double longitude = Convert.ToDouble(testing[39]);

            MapInfo eventMarker = new MapInfo
            {
                lat = latitude,
                lon = longitude
            };
          
            ViewData["MapInfo"] = eventMarker;
            ViewData["Event"] = userEvent;
            return View();
        }
    }
}
