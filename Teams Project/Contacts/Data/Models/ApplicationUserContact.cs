using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contacts.Data.Models
{
    public class ApplicationUserContact
    {
        [Required]
        public string ApplicationUserId  { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(ApplicationUserId))]
        public IdentityUser ApplicationUser  { get; set; } = null!;

        [Required]
        public int ContactId  { get; set; }

        [Required]
        [ForeignKey(nameof(ContactId))]
        public Contact Contact { get; set; } = null!;
    }
}
