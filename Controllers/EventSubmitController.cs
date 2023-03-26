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
            //HttpResponseMessage response = await client.GetAsync("https://overpass-api.de/api/interpreter?data=[out:json];area[name=%22Hanover%22]; area[addr:housenumber=%22101%22]; area[addr:street=%22Larchmont Lane%22]; area[addr:postcode=%2202339%22];out%20center%20;");
            //var eventMarker = await response.Content.ReadFromJsonAsync<Addresses>();

            HttpResponseMessage response = await client.GetAsync("http://localhost:8080/search/%20" + userAddress + "%20" + userCity + "%20" + userZipCode + "?format=json&limit=1");
            string httpResponse = await response.Content.ReadAsStringAsync();
            string[] testing = httpResponse.Split(',', '"');
            
            double bruh = Convert.ToDouble(testing[34]);
            double bruhh = Convert.ToDouble(testing[39]);
            System.Diagnostics.Debug.WriteLine(bruh);
            System.Diagnostics.Debug.WriteLine(bruhh);

            MapInfo eventMarker = new MapInfo
            {
                lat = bruh,
                lon = bruhh
            };

            //foreach (var huh in testing)
            //{
                //System.Diagnostics.Debug.WriteLine(huh);
            //}

            //MapInfo please = new MapInfo
            //{
                //lat = string[]
                //lon = string[]
            //}
            //JsonArray testing = JsonConvert.DeserializeObject<JsonArray>(httpResponse);
            //var testing2 = testing.ToJson;
            //var eventMarker = await testing2.
            //System.Diagnostics.Debug.WriteLine(httpResponse);
            //var eventMarker = await response.Content.ReadFromJsonAsync<MapInfo>();

            //var eventMarker = await response.Content.ReadFromJsonAsync();
            //var test = new StringContent(response.Content.ToString());
            //System.Diagnostics.Debug.WriteLine(test);

            

            ViewData["MapInfo"] = eventMarker;
            ViewData["Event"] = userEvent;
            return View();
        }
    }
}
