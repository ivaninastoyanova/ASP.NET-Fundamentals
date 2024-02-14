using Homies.Data;
using Homies.Data.Models;
using Homies.Models.Event;
using Homies.Models.Type;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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


        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var eventModel = new EventFormModel();

            var types = await GetTypes();

            eventModel.Types = types;

            return View(eventModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(EventFormModel eventModel)
        {
            string currentUserId = GetUserId();

            var types = await GetTypes();

            if (!types.Any(t => t.Id == eventModel.TypeId))
            {
                ModelState.AddModelError(nameof(eventModel.TypeId), "Type does not exist!");
            }

            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;

            if (!DateTime.TryParseExact(
               eventModel.Start,
               DataConstants.DateFormat,
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out start))
            {
                ModelState
                    .AddModelError(nameof(eventModel.Start), $"Invalid date! Format must be: {DataConstants.DateFormat}");
            }

            if (!DateTime.TryParseExact(
                eventModel.End,
                DataConstants.DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out end))
            {
                ModelState
                    .AddModelError(nameof(eventModel.End), $"Invalid date! Format must be: {DataConstants.DateFormat}");
            }

            if (!ModelState.IsValid)
            {
                eventModel.Types = types;
                return View(eventModel);
            }

            var eventToAdd = new Event()
            {
                Name = eventModel.Name,
                Description = eventModel.Description,
                CreatedOn = DateTime.Now,
                TypeId = eventModel.TypeId,
                OrganiserId = currentUserId,
                Start = start,
                End = end
            };

            await data.Events.AddAsync(eventToAdd);
            await data.SaveChangesAsync();

            return RedirectToAction("All", "Event");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var eventToEdit = await data.Events.FindAsync(id);

            if (eventToEdit == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();

            if (currentUserId != eventToEdit.OrganiserId)
            {
                return Unauthorized();
            }

            var  eventModel = new EventFormModel()
            {
                Name = eventToEdit.Name,
                Description = eventToEdit.Description,
                Start = eventToEdit.Start.ToString(DataConstants.DateFormat),
                End = eventToEdit.End.ToString(DataConstants.DateFormat),
                TypeId = eventToEdit.TypeId,
                Types = await GetTypes()
            };

            return View(eventModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EventFormModel eventModel)
        {
            string currentUser = GetUserId();

            var eventToEdit = await data.Events.FindAsync(id);

            if (eventToEdit == null)
            {
                return BadRequest();
            }

            if (currentUser != eventToEdit.OrganiserId)
            {
                return Unauthorized();
            }

            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;

            if (!DateTime.TryParseExact(
                eventModel.Start,
                DataConstants.DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out start))
            {
                ModelState
                    .AddModelError(nameof(eventModel.Start), $"Invalid date! Format must be: {DataConstants.DateFormat}");
            }

            if (!DateTime.TryParseExact(
                eventModel.End,
                DataConstants.DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out end))
            {
                ModelState
                    .AddModelError(nameof(eventModel.End), $"Invalid date! Format must be: {DataConstants.DateFormat}");
            }

            var types = await GetTypes();

            if (!types.Any(t => t.Id == eventModel.TypeId))
            {
                ModelState.AddModelError(nameof(eventModel.TypeId), "Type does not exist!");
            }

            if (!ModelState.IsValid)
            {
                eventModel.Types = types;

                return View(eventModel);
            }

            eventToEdit.Name = eventModel.Name;
            eventToEdit.Description = eventModel.Description;
            eventToEdit.Start = start;
            eventToEdit.End = end;
            eventToEdit.TypeId = eventModel.TypeId;

            await data.SaveChangesAsync();

            return RedirectToAction("All", "Event");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var eventToDisplay = await data
               .Events
               .Where(e => e.Id == id)
               .Select(e => new EventDetailsViewModel()
               {
                   Id = e.Id,
                   Name = e.Name,
                   Start = e.Start.ToString(DataConstants.DateFormat),
                   End = e.End.ToString(DataConstants.DateFormat),
                   Organiser = e.Organiser.UserName,
                   Type = e.Type.Name,
                   Description = e.Description,
                   CreatedOn = e.CreatedOn.ToString(DataConstants.DateFormat)
               })
               .FirstOrDefaultAsync();

            if (eventToDisplay == null)
            {
                return BadRequest();
            }

            return View(eventToDisplay);
        }



        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        private async Task<IEnumerable<TypeViewModel>> GetTypes()
        {
            var types = await data
                        .Types
                         .Select(t => new TypeViewModel
                         {
                             Name = t.Name,
                             Id = t.Id,
                         })
                         .ToListAsync();

            return types;
        }

    }
}
