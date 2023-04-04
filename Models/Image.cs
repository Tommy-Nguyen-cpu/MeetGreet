using System;
using System.Collections.Generic;

namespace MeetGreet.Models;

public partial class Image
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public string S3key { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;
}
