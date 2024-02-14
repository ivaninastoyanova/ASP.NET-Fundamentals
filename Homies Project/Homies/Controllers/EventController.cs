using Homies.Data;
using Homies.Data.Models;
using Homies.Models.Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Xml.Linq;

namespace Homies.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly HomiesDbContext data;

        public EventController(HomiesDbContext context)
        {
            data = context;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var eventsToDisplay = await data
                .Events
                .Select(e => new EventInfoViewModel()
                {
                   Name = e.Name,
                   Id = e.Id,
                   Start = e.Start.ToString(DataConstants.DateFormat),
                   Type = e.Type.Name,
                   Organiser = e.Organiser.UserName
                })
                .ToListAsync();

            return View(eventsToDisplay);
        }

        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            string currentUserId = GetUserId();

            var userEvents = await data
                .EventParticipants
                .Where(ep => ep.HelperId == currentUserId)
                .Select(ep => new EventInfoViewModel()
                {
                    Name = ep.Event.Name,
                    Id = ep.Event.Id,
                    Start = ep.Event.Start.ToString(DataConstants.DateFormat),
                    Type = ep.Event.Type.Name,
                })
                .ToListAsync();

            return View(userEvents);
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            var currentEvent = await data
                .Events
                .FindAsync(id);

            if (currentEvent == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();

            var entryToAdd = new EventParticipant()
            {
                EventId = currentEvent.Id,
                HelperId = currentUserId,
            };

            if (await data.EventParticipants.ContainsAsync(entryToAdd))
            {
                return RedirectToAction("Joined", "Event");
            }

            await data.EventParticipants.AddAsync(entryToAdd);

            await data.SaveChangesAsync();

            return RedirectToAction("Joined", "Event");
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            var eventId = id;
            var eventToLeave = await data
                .Events
                .FindAsync(eventId);

            if (eventToLeave == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();

            var entryToRemove = await data.EventParticipants
                .FirstOrDefaultAsync(ep => ep.HelperId == currentUserId && ep.EventId == eventId);
            
            if (entryToRemove == null)
            {
                return BadRequest();
            }

            data.EventParticipants.Remove(entryToRemove);

            await data.SaveChangesAsync();

            return RedirectToAction("All", "Event");
        }

        private string GetUserId()
          => User.FindFirstValue(ClaimTypes.NameIdentifier);

    }
}
