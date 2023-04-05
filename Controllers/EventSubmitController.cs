using MeetGreet.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;


namespace MeetGreet.Controllers
{
    public class EventSubmitController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EventSubmitPage(string userEventName, string userDescription, DateTime userDateTime, IFormFile imageFileForm, string userAddress, string userCity, string userZipCode)
        {
            EventImage eventImage = new EventImage()
            {
                imageFileForm = imageFileForm
            };

            using (var memoryStream = new MemoryStream())
            {
                await imageFileForm.CopyToAsync(memoryStream);
                eventImage.imageBytes = memoryStream.GetBuffer();
                
            }


            Event userEvent = new Event
            {
                Title = userEventName,
                Description = userDescription,
                ScheduledDateTime = userDateTime,
                Address = userAddress,
                City = userCity,
                ZipCode = userZipCode
            };

            double latitude = 0;
            double longitude = 0;


            //my version of error handling kinda klunky and not the best but very open to change just something quick I came up with
            try
            {
                HttpClient test = new HttpClient();

                test.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36");

                HttpResponseMessage resp = await test.GetAsync("https://nominatim.openstreetmap.org/search/" + userAddress + "%20" + userCity + "%20" + userZipCode + "?format=json&limit=1");
                string httpResponse = await resp.Content.ReadAsStringAsync();
                string[] respArray = httpResponse.Split(',', '"');

                latitude = Convert.ToDouble(respArray[34]);
                longitude = Convert.ToDouble(respArray[39]);
            }
            catch (Exception err)
            {
                return View("~/Views/EventCreation/EventCreationPageError.cshtml");
            }

            userEvent.GeoLocationLatitude = latitude;
            userEvent.GeoLocationLongitude = longitude;

            MapInfo eventMarker = new MapInfo
            {
                lat = latitude,
                lon = longitude
            };

            ViewData["MapInfo"] = eventMarker;
            ViewData["Event"] = userEvent;
            ViewData["EventImage"] = eventImage;
            return View();
        }
    }
}
