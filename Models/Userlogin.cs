using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace MeetGreet.Models;

public partial class UserLogin : IdentityUserLogin<string>
{
    public override string LoginProvider { get; set; } = null!;

    public override string ProviderKey { get; set; } = null!;

    public override string? ProviderDisplayName { get; set; }

    public override string UserId { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
