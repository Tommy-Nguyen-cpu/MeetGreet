using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetGreet.Models;

public partial class Event
{
    public int Id { get; set; }

    /// <summary>
    /// Host will define what the event is called.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Date the event was created at.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Primary key of user who created the event.
    /// </summary>
    public string CreatedByUserId { get; set; } = null!;

    /// <summary>
    /// Description for the event.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Longitude of the event (used for event marker placing).
    /// </summary>
    public double GeoLocationLongitude { get; set; }

    /// <summary>
    /// Latitute of the event (used for event marker placing).
    /// </summary>
    public double GeoLocationLatitude { get; set; }

    /// <summary>
    /// Zipcode of event location.
    /// </summary>
    public string ZipCode { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Address { get; set; } = null!;

    /// <summary>
    /// The date at which the event is scheduled for.
    /// </summary>
    public DateTime ScheduledDateTime { get; set; }

    /// <summary>
    /// The radius that users must be within in order to see the event.
    /// </summary>
    public int? Radius { get; set; }

    public virtual ICollection<AttendingIndication> AttendingIndications { get; } = new List<AttendingIndication>();
    public virtual User CreatedByUser { get; set; } = null!;
    public virtual ICollection<Image> Images { get; } = new List<Image>();
}
