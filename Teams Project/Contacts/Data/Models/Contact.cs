using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Contacts.Data.DataConstants;

namespace Contacts.Data.Models
{
    public class Contact
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(ContactFirstNameMaxLength)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(ContactLastNameMaxLength)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(ContactEmailMaxLength)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(ContactPhoneMaxLength)]
        [RegularExpression(ContactPhoneRegex)]
        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        [Required]
        [RegularExpression(ContactWebsiteRegex)]
        public string Website { get; set; } = string.Empty;

        public IList<ApplicationUserContact> ApplicationUsersContacts { get; set; } = new List<ApplicationUserContact>();

    }
}
