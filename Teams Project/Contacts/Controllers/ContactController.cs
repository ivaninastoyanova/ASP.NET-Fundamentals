﻿using Contacts.Data;
using Contacts.Data.Models;
using Contacts.Models.Contact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;

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
                    Id = c.Id,
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

        [HttpGet]
        public async Task<IActionResult> Team()
        {
            string currentUserId = GetUserId();

            var userContacts = await data
                .ApplicationUsersContacts
                .Where(uc => uc.ApplicationUserId == currentUserId)
                .Select(uc => new AllContactViewModel()
                {
                    Id = uc.Contact.Id,
                    FirstName = uc.Contact.FirstName,
                    LastName = uc.Contact.LastName,
                    Email = uc.Contact.Email,
                    PhoneNumber = uc.Contact.PhoneNumber,
                    Address = uc.Contact.Address,
                    Website = uc.Contact.Website
                })
                .ToListAsync();

            return View(userContacts);
        }


        [HttpPost]
        public async Task<IActionResult> AddToTeam(int id)
        {
            var currentContact = await data
                .Contacts
                .FindAsync(id);

            if (currentContact == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();

            var entryToAdd = new ApplicationUserContact()
            {
                ContactId = currentContact.Id,
                ApplicationUserId = currentUserId,
            };

            if (await data.ApplicationUsersContacts.ContainsAsync(entryToAdd))
            {
                return RedirectToAction("All", "Contact");
            }

            await data.ApplicationUsersContacts.AddAsync(entryToAdd);

            await data.SaveChangesAsync();

            return RedirectToAction("All", "Contact");
        }


        [HttpPost]
        public async Task<IActionResult> RemoveFromTeam(int id)
        {
            var contactId = id;
            var contactToRemove = await data
                .Contacts
                .FindAsync(contactId);

            if (contactToRemove == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();

            var entryToRemove = await data.ApplicationUsersContacts
                .FirstOrDefaultAsync(uc => uc.ApplicationUserId == currentUserId && uc.ContactId == contactId);

            if (entryToRemove == null)
            {
                return BadRequest();
            }

            data.ApplicationUsersContacts.Remove(entryToRemove);

            await data.SaveChangesAsync();

            return RedirectToAction("Team", "Contact");
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var contactModel = new AddContactViewModel();

            return View(contactModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddContactViewModel contactModel)
        {   
            if (!ModelState.IsValid)
            {
                return View(contactModel);
            }

            var contactToAdd = new Contact()
            {
               FirstName = contactModel.FirstName,
               LastName = contactModel.LastName,    
               Email = contactModel.Email,
               PhoneNumber = contactModel.PhoneNumber,
               Address = contactModel.Address,
               Website = contactModel.Website
            };

            await data.Contacts.AddAsync(contactToAdd);
            await data.SaveChangesAsync();

            return RedirectToAction("All", "Contact");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var contactToEdit = await data.Contacts.FindAsync(id);

            if (contactToEdit == null)
            {
                return BadRequest();
            }

            var contactModel = new AddContactViewModel()
            {
                FirstName = contactToEdit.FirstName,
                LastName = contactToEdit.LastName,
                Email = contactToEdit.Email,
                PhoneNumber = contactToEdit.PhoneNumber,
                Address = contactToEdit.Address,
                Website= contactToEdit.Website
            };

            return View(contactModel);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddContactViewModel contactModel)
        {
            var contactToEdit = await data.Contacts.FindAsync(id);

            if (contactToEdit == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(contactModel);
            }

            contactToEdit.FirstName = contactModel.FirstName;
            contactToEdit.LastName = contactModel.LastName;
            contactToEdit.Email = contactModel.Email;
            contactToEdit.PhoneNumber = contactModel.PhoneNumber;
            contactToEdit.Address = contactModel.Address;
            contactToEdit.Website = contactModel.Website;

            await data.SaveChangesAsync();

            return RedirectToAction("All", "Contact");
        }


        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }
    }
}
