using System;
using System.Collections.Generic;

namespace MeetGreet.Models;

public partial class Awsapikey
{
    public int Id { get; set; }

    public string? AccessKey { get; set; }

    public string? SecretAccessKey { get; set; }
}
