using MeetGreet.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

namespace MeetGreet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<ActionResult> Index()
        {
            HttpClient client = new HttpClient();

            // Below is the url for querying for a specific location.
            // "https://overpass-api.de/api/interpreter?data=[out:json];area[name=%22Beatty Hall%22];out%20center%20;"

            // Query for information on a city. NOTE: DIFFERENT THAN QUERYING FOR INFO ON A SPECIFIC LOCATION
            // Specific location has a "center" key where the lat and lon are stored, city queries DO NOT have "center" key.
            HttpResponseMessage response = await client.GetAsync("https://overpass-api.de/api/interpreter?data=[out:json];area[name=%22Boston%22];(node[place=%22city%22](area););out%20center%20;");
            var myResult = await response.Content.ReadFromJsonAsync<Addresses>();
/*            byte[] raw = client.DownloadData("https://overpass-api.de/api/interpreter?data=[out:json];area[name=%22Boston%22];(node[place=%22city%22](area););out%20center%20;");
            string webData = System.Text.Encoding.UTF8.GetString(raw);

            // Converts Json string into "Addresses" class. All data is filled out in the right location.
            var myResult = JsonConvert.DeserializeObject<Addresses>(webData);*/

            foreach (var address in myResult.elements)
            {
                if (address.center != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Lat, Lon: {address.center.lat}, {address.center.lon}");
                }
                System.Diagnostics.Debug.WriteLine($"Name: {address.tags.name}\n\n");
            }

            ViewData["Addresses"] = myResult;

            // Retrieves all data on all addresses with "Beatty Hall" as name.
            response = await client.GetAsync("https://overpass-api.de/api/interpreter?data=[out:json];area[name=%22Beatty Hall%22];out%20center%20;");
            var beattyHalls = await response.Content.ReadFromJsonAsync<Addresses>();
            ViewData["BeattyHalls"] = beattyHalls;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}