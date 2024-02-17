using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Contacts.Data.DataConstants;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Data.Models
{
    public class Contact
    {
        [Key]
        [Required]
        [Comment("Contact identifier")]
        public int Id { get; set; }

        [Required]
        [MaxLength(ContactFirstNameMaxLength)]
        [Comment("Contact first name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(ContactLastNameMaxLength)]
        [Comment("Contact last name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(ContactEmailMaxLength)]
        [EmailAddress]
        [Comment("Contact last name")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(ContactPhoneMaxLength)]
        [RegularExpression(ContactPhoneRegex)]
        [Comment("Contact phone number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Comment("Contact address")]
        public string Address { get; set; } = string.Empty;

        [Required]
        [RegularExpression(ContactWebsiteRegex)]
        [Comment("Contact website")]
        public string Website { get; set; } = string.Empty;

        public IList<ApplicationUserContact> ApplicationUsersContacts { get; set; } = new List<ApplicationUserContact>();

    }
}
