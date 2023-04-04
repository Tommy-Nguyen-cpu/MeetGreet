using System;
using System.Drawing;

namespace MeetGreet.Models
{
	public partial class EventImage
	{
        public IFormFile imageFileForm { get; set; } = null!;
        public byte[] imageBytes { get; set; } = null!;
    }
}

