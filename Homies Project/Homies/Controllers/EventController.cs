using Homies.Data;
using Homies.Models.Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}
