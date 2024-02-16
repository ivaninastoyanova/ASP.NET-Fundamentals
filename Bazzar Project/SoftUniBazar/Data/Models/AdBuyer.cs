using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftUniBazar.Data.Models
{
    public class AdBuyer
    {
        [Required]
        [Comment("Buyer identifier")]

        public string BuyerId { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(BuyerId))]
        public IdentityUser Buyer { get; set; } = null!;

        [Required]
        [Comment("Ad identifier")]

        public int AdId { get; set; }

        [Required]
        [ForeignKey(nameof(AdId))]
        public Ad Ad { get; set; } = null!;
    }
}
