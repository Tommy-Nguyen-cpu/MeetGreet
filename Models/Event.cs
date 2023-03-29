using System;
using System.Collections.Generic;

namespace MeetGreet.Models;

public partial class Event
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string CreatedByUserId { get; set; } = null!;

    public string Description { get; set; } = null!;

    public double GeoLocationLongitude { get; set; }

    public double GeoLocationLatitude { get; set; }

    public string ZipCode { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateTime ScheduledDateTime { get; set; }
    public int Radius { get; set; }

    public virtual ICollection<AttendingIndication> AttendingIndications { get; } = new List<AttendingIndication>();
    public virtual User CreatedByUser { get; set; } = null!;
}
