using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace MeetGreet.Models;

public partial class UserToken : IdentityUserToken<string>
{
    public override string UserId { get; set; } = null!;

    public override string LoginProvider { get; set; } = null!;

    public override string Name { get; set; } = null!;

    public override string? Value { get; set; }

    public virtual User User { get; set; } = null!;
}
