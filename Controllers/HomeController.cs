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
using System.Security.Claims;
using System;

namespace MeetGreet.Controllers
{
    // "Authorize" tag essentially says that only users who are logged in can have access to the pages associated with this controller.
    // We can bring the "Authorize" tag to any and all controller and it will do the exact same thing.
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // _context gives us direct access to the database. So we can use _context to query for data in the database.
        private readonly MeetgreetContext _context;

        // "connect" isn't used for any functionality at the moment. It is here mainly for running one of the examples below.
        private readonly MySqlConnection connect;

        public HomeController(ILogger<HomeController> logger, MeetgreetContext context, MySqlConnection connection)
        {
            _logger = logger;
            _context = context;
            connect = connection;
        }

        public async Task<ActionResult> Attendance(int? Attending = null, int? Interested = null, int? NotAttending = null)
        {
            Debug.WriteLine("Attending?: " + Attending);
            Debug.WriteLine("Interested?: " + Interested);
            Debug.WriteLine("Not Attending?: " + NotAttending);

            // Retrieves the ID of the current logged in user.
            string? id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // ID shouldn't be null, but may be null in some exceptional cases.
            if(!string.IsNullOrEmpty(id) )
            {

                AttendingIndication attendance = new AttendingIndication();
                attendance.UserId = id;

                // Depending on which icon the user clicks on, the corresponding "attending", "interested", or "not attending" will be stored in the DB.
                if (Attending != null)
                {
                    attendance.EventId = (int)Attending;
                    attendance.Status = 0;
                }
                else if (Interested != null)
                {
                    attendance.EventId = (int)Interested;
                    attendance.Status = 1;
                }
                else if(NotAttending != null)
                {
                    attendance.EventId = (int)NotAttending;
                    attendance.Status = 2;
                }

                _context.Add(attendance);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Something went wrong! Cannot get User ID!!!");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Index(string? searchString)
        {
            // If the user the search bar to search for an event by name, we will query in the context object.
            if (!String.IsNullOrEmpty(searchString))
            {
                // Searches for all events with the matching search string.
                List<Event> searchedEvents = new List<Event>();
                foreach(var myEvent in _context.Events)
                {
                    if(myEvent.Title.ToLower().Contains(searchString.ToLower()))
                        searchedEvents.Add(myEvent);
                }

                // Returns list of all events containing the same search string.
                return View(searchedEvents);
            }

            // Sends the list of events to the View.
            return View(RetrieveEvents());
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

        private List<Event> RetrieveEvents()
        {
            Debug.WriteLine("There are " + _context.Events.Count() + " events in the database table");

            // Retrieves the ID of the current user.
            string? currentUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Instantiates a list of events.
            List<Event> events = new List<Event>();

            // Iterates through all events in the database.
            foreach(var myEvent in _context.Events)
            {
                events.Add(myEvent);
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
            #region Retrieve Entries In A Database Table

            // Retrieving data from a database is incredibly straightforward.
            // _context (our local instance of MeetgreetContext) is our portal to the database.
            // _context contains everything we need to retrieve, create, update, and remove entries.

            // in the example below, we iterate through all entries in the table "Users".
            foreach(var user in _context.Users)
            {
                // We then check to see if an entry contains the email we want.
                // Note that checking is not limited to just emails, you can check to see if a user contains a certain id, password, etc.
                if(user.Email == "SomeEmail@gmail.com")
                {
                    // After this point, we can do whatever we want. We can return the current "user" variable, add it to a list of users, etc.
                    Debug.WriteLine("Found SomeEmail@gmail.com email!");
                }
            }


            #endregion


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