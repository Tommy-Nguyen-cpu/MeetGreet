using MeetGreet.Models;
using Microsoft.AspNetCore.Mvc;


namespace MeetGreet.Controllers
{
    public class EventSubmitController : Controller
    {
        /*
         * This allows for the data that the user entered in the event creation page to be displayed on last time before it is submitted 
         * to the SQL table. This method takes in the values entered by the user in the event creation page as params. These values are assigned
         * to an empty event model to hold the data. We call the Nominatim API to get the geolocation of the event and display it on the
         * map for the user. 
         */
        [HttpPost]
        public async Task<IActionResult> EventSubmitPage(string userEventName, string userDescription, DateTime userDateTime, string userAddress, string userCity, string userZipCode)
        {
            //takes params and assigns it to event model
            Event userEvent = new Event
            {
                Title = userEventName,
                Description = userDescription,
                ScheduledDateTime = userDateTime,
                Address = userAddress,
                City = userCity,
                ZipCode = userZipCode
            };
            //create variables to hold longitude and latitude once API returns them
            double latitude = 0;
            double longitude = 0;

            /*
             * Put the API call inside a try catch block.
             * This will handel any errors in case the user entered the address incorrectly. If there is an error with the API call
             * it is caught and the user is redirected to an Event Creation Page with an error. 
             */
            try
            {
                HttpClient httpClient = new HttpClient();
                //Need to include a user agent in order to query Nominatim API without it request is blocked
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36");
                //Gets response from Nominatim API
                HttpResponseMessage resp = await httpClient.GetAsync("https://nominatim.openstreetmap.org/search/" + userAddress + "%20" + userCity + "%20" + userZipCode + "?format=json&limit=1");
                //Put response into a string
                string httpResponse = await resp.Content.ReadAsStringAsync();
                //Seperate string into array by commas and quotation marks.
                string[] respArray = httpResponse.Split(',', '"');
                //Because of the way nominatim API returns the longitude and latitude are always in the same posistion in the array
                latitude = Convert.ToDouble(respArray[34]);
                longitude = Convert.ToDouble(respArray[39]);
            }
            //If any error redirect to error page
            catch(Exception err)
            {
                return View("~/Views/EventCreation/EventCreationPageError.cshtml");
            }
            //fill in longitude and latitude in model
            userEvent.GeoLocationLatitude = latitude;
            userEvent.GeoLocationLongitude = longitude;
            //pass longitude and latitude into their own model
            MapInfo eventMarker = new MapInfo
            {
            lat = latitude,
            lon = longitude
            };
            //pass data onto the EventSubmitView
            ViewData["MapInfo"] = eventMarker;
            ViewData["Event"] = userEvent;
            return View();
        }
    }
}
