using Microsoft.AspNetCore.Mvc;

namespace MeetGreet.Models
{
    // TODO: TEMP MODEL UNTIL WE GET A DATABASE TABLE FOR EVENTS
    public class Event
    {
        public string EventId { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string imageURL { get; set; }
        public string EventAddress { get; set; }
        public string EventCity { get; set; }
        public string EventZipcode { get; set; }
        public double lon { get; set; }
        public double lat { get; set; }
    }
}
