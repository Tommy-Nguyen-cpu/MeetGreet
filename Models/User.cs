using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeetGreet.Models;

public partial class User : IdentityUser
{
    public override string Id { get; set; } = null!;

    // IdentityUser requires this column. Currently, it is different from email, but if we ever decide to allow usernames other than email, we can implement it.
    public override string? UserName { get; set; }

    public override string? NormalizedUserName { get; set; }

    [Required]
    [EmailAddress]
    public override string? Email { get; set; }

    public override string? NormalizedEmail { get; set; }

    public override bool EmailConfirmed { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public override string? PasswordHash { get; set; }

    [Display(Name = "Remember Me?")]
    public bool RememberMe { get; set; }

    public virtual ICollection<UserClaim> UserClaims { get; } = new List<UserClaim>();

    public virtual ICollection<UserLogin> UserLogins { get; } = new List<UserLogin>();

    public virtual ICollection<UserToken> UserTokens { get; } = new List<UserToken>();
}
