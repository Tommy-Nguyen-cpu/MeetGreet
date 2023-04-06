// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using MeetGreet.Data;
using MeetGreet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MeetGreet.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class MyEvents : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly MeetgreetContext _context;

        // Events that they are hosting.
        public List<Event> HostedEvents { get; set; } = new List<Event>();

        // Events that they'll attend.
        public List<Event> AttendingEvents { get; set; } = new List<Event>();

        // Number of attendees per event.
        public Dictionary<int, int> NumberOfAttendancePerEvents { get; set; } = new Dictionary<int, int>();

        public MyEvents(UserManager<User> userManager, SignInManager<User> signInManager, MeetgreetContext context, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;

            // I believe that we need to explicitly pass in a "IHttpContextAccessor" for classes that aren't controllers in order to access current user login info.
            string? id = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieves all events hosted by the current user.
            foreach (var myEvent in _context.Events.ToList())
            {
                if (myEvent.CreatedByUserId.ToLower().Equals(id.ToLower()))
                {
                    HostedEvents.Add(myEvent);

                    NumberOfAttendancePerEvents.Add(myEvent.Id, 0);
                    foreach(var attendance in _context.AttendingIndications)
                    {
                        if(attendance.EventId == myEvent.Id)
                        {
                            NumberOfAttendancePerEvents[myEvent.Id]++;
                        }
                    }
                }
            }

            // Retrieve all events that the user has indicated that they are going to attend.
            foreach(var attendingEvent in _context.AttendingIndications.ToList())
            {
                if (attendingEvent.UserId.ToLower().Equals(id.ToLower()))
                {
                    foreach(var eventInfo in _context.Events.ToList())
                    {
                        // If the user said they'll attend the event.
                        if(eventInfo.Id == attendingEvent.EventId && attendingEvent.Status == 0)
                        {
                            AttendingEvents.Add(eventInfo);
                            break;
                        }
                    }
                }
            }
        }
    }
}
