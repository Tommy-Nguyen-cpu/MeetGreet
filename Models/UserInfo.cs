using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeetGreet.Models
{
    public class UserInfo
    {

        public string Email { get; set; }
        public string HashedPassword { get; set; }

        public string Id => throw new System.NotImplementedException();

        public string UserName { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}