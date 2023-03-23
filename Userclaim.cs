using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace MeetGreet;

public partial class Userclaim :IdentityUserClaim<string>
{
    public override int Id { get; set; }

    public override string? UserId { get; set; }

    public override string? ClaimType { get; set; }

    public override string? ClaimValue { get; set; }

    public virtual User User { get; set; } = null!;
}
