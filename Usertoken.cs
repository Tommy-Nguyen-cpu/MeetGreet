using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace MeetGreet;

public partial class Usertoken : IdentityUserToken<string>
{
    public override string UserId { get; set; } = null!;

    public override string LoginProvider { get; set; } = null!;

    public override string Name { get; set; } = null!;

    public override string? Value { get; set; }

    public virtual User User { get; set; } = null!;
}
