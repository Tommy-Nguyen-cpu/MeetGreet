using System;
using System.Collections.Generic;

namespace MeetGreet.Models;

public partial class AttendingIndication
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public int EventId { get; set; }

    public int Status { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
