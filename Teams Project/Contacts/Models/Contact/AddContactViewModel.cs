using Contacts.Data.Models;
using System.ComponentModel.DataAnnotations;
using static Contacts.Data.DataConstants;

namespace Contacts.Models.Contact
{
    public class AddContactViewModel
    {
        [Required]
        [StringLength(ContactFirstNameMaxLength, MinimumLength = ContactFirstNameMinLength,
            ErrorMessage = ContactErrorMessage )]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(ContactLastNameMaxLength, MinimumLength = ContactLastNameMinLength,
            ErrorMessage = ContactErrorMessage)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(ContactEmailMaxLength, MinimumLength = ContactEmailMinLength,
            ErrorMessage = ContactErrorMessage)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(ContactPhoneMaxLength, MinimumLength = ContactPhoneMinLength, 
            ErrorMessage = ContactErrorMessage)]
        [RegularExpression(ContactPhoneRegex,
            ErrorMessage = "Contact {0} is not a valid format phone number.")]
        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        [Required]
        [RegularExpression(ContactWebsiteRegex,
            ErrorMessage = "Contact {0} is not a valid format website.")]
        public string Website { get; set; } = string.Empty;
    }
}
