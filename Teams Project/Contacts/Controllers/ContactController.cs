using Contacts.Data;
using Contacts.Models.Contact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private readonly ContactsDbContext data;

        public ContactController(ContactsDbContext context)
        {
            data = context;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var contactsToDisplay = await data
                .Contacts
                .Select(c => new AllContactViewModel()
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    Address = c.Address,
                    Website = c.Website
                })
                .ToListAsync();

            return View(contactsToDisplay);
        }
    }
}
