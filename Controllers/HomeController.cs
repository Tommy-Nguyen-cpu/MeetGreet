using MeetGreet.Data;
using MeetGreet.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

namespace MeetGreet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MeetgreetContext _context;
        private readonly MySqlConnection connect;
        public HomeController(ILogger<HomeController> logger, MeetgreetContext context, MySqlConnection connection)
        {
            _logger = logger;
            _context = context;
            connect = connection;
        }

        public async Task<ActionResult> Index()
        {


            HttpClient client = new HttpClient();

            // Below is the url for querying for a specific location.
            // "https://overpass-api.de/api/interpreter?data=[out:json];area[name=%22Beatty Hall%22];out%20center%20;"

            // Query for information on a city. NOTE: DIFFERENT THAN QUERYING FOR INFO ON A SPECIFIC LOCATION
            // Specific location has a "center" key where the lat and lon are stored, city queries DO NOT have "center" key.
            HttpResponseMessage response = await client.GetAsync("https://overpass-api.de/api/interpreter?data=[out:json];area[name=%22Boston%22];(node[place=%22city%22](area););out%20center%20;");
            
            // TODO: Sometimes this throws an exception. Fix it so that it never throws an exception.
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


        /// <summary>
        /// Method for storing the code for alternative methods of querying from a MYSQL database.
        /// Not used for anything, just here for reference.
        /// </summary>
        private void AltDatabaseMethods()
        {
            #region MySQLConnector
/*            // MYSqlConnector Alternative to NodeJS

            // Opens connection to MYSQL database.
            await connect.OpenAsync();

            // Sends a request for email in table "user".
            MySqlCommand command = new MySqlCommand("SELECT email from user;", connect);

            // Reads result.
            MySqlDataReader reader = command.ExecuteReader();

            // Iterates through all results and prints out the email address.
            while (reader.Read())
            {
                System.Diagnostics.Debug.WriteLine("Test Email Retrieve: " + reader.GetValue(0).ToString());
            }
            reader.Close();*/
            #endregion

            /*            #region NodeJS
            HttpClient client2 = new HttpClient();

            // Goes to the website and requests for data on that site (GET request).
            HttpResponseMessage response2 = await client2.GetAsync("http://localhost:5500/sign-in-user/baffog@wit.edu/test123");
            
            // Retrieves the response for the site as a string (if we plan on going with NodeJS from now on, we'll have to create a model for the data and call "ReadAsJsonAsync()" instead of "ReadAsStringAsync()").
            var result = await response2.Content.ReadAsStringAsync();

            // Prints out result (json string with status, message, and userInfo).
            System.Diagnostics.Debug.WriteLine($"NODEJS Result: {result}");
            #endregion*/
        }
    }
}