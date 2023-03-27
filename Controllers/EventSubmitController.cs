using Azure;
using MeetGreet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using NuGet.Protocol;
using SendGrid;
using System.Net;
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
                Title = userEventName,
                Description = userDescription,
                Address = userAddress,
                City = userCity,
                ZipCode = userZipCode
            };

            HttpClient test = new HttpClient();

            test.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36");

            HttpResponseMessage resp = await test.GetAsync("https://nominatim.openstreetmap.org/search/" + userAddress + "%20" + userCity + "%20" + userZipCode + "?format=json&limit=1");
            string httpResponse = await resp.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine(httpResponse);

            string[] respArray = httpResponse.Split(',', '"');

            double latitude = Convert.ToDouble(respArray[34]);
            double longitude = Convert.ToDouble(respArray[39]);

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
