using MeetGreet.Data;
using MeetGreet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using RestSharp;
using MimeKit.Text;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;

namespace MeetGreet.Controllers
{
    // "Authorize" tag essentially says that only users who are logged in can have access to the pages associated with this controller.
    // We can bring the "Authorize" tag to any and all controller and it will do the exact same thing.
    [Authorize]
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

            // TODO: For some odd reason, when I try to send "events" as a list, it throws an error telling me to update to https.
            ViewData["Events"] = GenerateEvents();

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


        // TODO: TEMP METHOD FOR GENERATING EVENTS UNTIL WE START CREATING LEGITIMATE EVENTS.
        private List<Event> GenerateEvents()
        {
            List<Event> events = new List<Event>();

            Random random= new Random();

            for(int i = 0; i < 10; i++)
            {
                Event newEvent = new Event();
                newEvent.EventName = $"Some Event {i}";
                newEvent.EventDescription = "A Description";
                newEvent.EventLocation = "Some Where :)";
                newEvent.imageURL = "https://media.istockphoto.com/id/1181250359/photo/business-people.jpg?s=612x612&w=0&k=20&c=1DFEPJdcvlhFdQYp-hzj2CYXXRn-b6qYoPgyOptZsck=";
                newEvent.lat= random.NextDouble()*100;
                newEvent.lon = random.NextDouble()*100;
                events.Add(newEvent);
            }

            return events;
        }


        #region Not Used Methods (For reference only)
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

        /// <summary>
        /// Method illustrating how we'd query with "_context".
        /// Methods are used as examples, and may not work if:
        /// A) user does not exist in the database table
        /// B) the database does not contain the table
        /// As such, it is best to try and understand as much as you can and mess around with it.
        /// </summary>
        private async void QueryThroughContext()
        {
            #region Create Entries In a Database Table
            // Creating a new entry for a database is very easy.
            // All that is needed to be done is to instantiate a model, and add data to each fields in the model.
            // Below, I decided to instantiate the "User" model and create data for the user.

            // In this case, I decided to create the user with email "someEmail@gmail.com".
            User newUser = new User();
            newUser.Email = "someEmail@gmail.com";
            newUser.PasswordHash= "12345";
            newUser.RememberMe = true;
            newUser.UserName = newUser.Email;

            // Once you are done with the model, just pass the model into "_context.Add()" and it'll do the work of
            // finding the corresponding database table and adding the entries for you!
            _context.Add(newUser);

            // NOTE: In order for the entries to be saved in the database, you MUST always call ".SaveChanges()" or ".SaveChangesAsync()".
            // ".SaveChanges()" or ".SaveChangesAsync()" must be called after all database manipulations, regardless of the type of manipulation it is (add, update, delete).
            // ".SaveChangesAsync()" is preferred because it allows for better performance and is more responsive (i.e. runs on a separate thread).
            await _context.SaveChangesAsync();
            #endregion


            #region Update entries

            // The for loop below attempts to find the user with email "Some1@gmail.com".
            // If it finds the user with that email, it will set "findUser" to be that user.
            User? findingUser = null;
            foreach(var user in _context.Users)
            {
                if (user.Email.ToLower().Contains("Some1@gmail.com"))
                {
                    findingUser = user;
                    break;
                }
            }

            // The line below updates the email to be a different email.
            findingUser.Email = "NewEmail@gmail.com";

            // We then pass in the updated user model into "_context.Update()" which will update that specific entry in the database.
            _context.Update(findingUser);

            // Again, we save so that the database is actually updated.
            await _context.SaveChangesAsync();
            #endregion

            #region Delete entries

            // Searches for user with email "SomeEmail@gmail.com".
            User? removeUser = null;
            foreach(var user in _context.Users)
            {
                if (user.Email.ToLower().Contains("SomeEmail@gmail.com"))
                {
                    removeUser = user;
                    break;
                }
            }

            // Removes user from database table "User".
            _context.Remove(removeUser); 
            
            // Saves changes.
            await _context.SaveChangesAsync();

            #endregion
        }
        #endregion
    }
}