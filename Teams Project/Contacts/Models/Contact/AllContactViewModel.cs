using Contacts.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Contacts.Models.Contact
{
    public class AllContactViewModel
    {
       
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Website { get; set; } = string.Empty;

    }
}