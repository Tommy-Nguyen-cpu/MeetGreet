using System.Collections.Generic;

namespace MeetGreet2.Models
{
    public class Addresses
    {
        public List<AddressInfo> elements { get; set; }
    }
    public class AddressInfo
    {
        public double lat { get; set; }
        public double lon { get;set; }
        public center center { get; set; }
        public tags tags { get; set; }
    }

    public class center
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }

    public class tags
    {
        public string name { get; set; }
    }
}