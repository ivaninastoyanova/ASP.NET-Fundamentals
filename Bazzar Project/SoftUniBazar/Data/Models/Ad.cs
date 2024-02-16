using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static SoftUniBazar.Data.DataConstants;
using Microsoft.EntityFrameworkCore;

namespace SoftUniBazar.Data.Models
{
    public class Ad
    {
        [Key]
        [Required]
        [Comment("Ad identifier")]
        public int Id { get; set; }

        [Required]
        [MaxLength(AdNameMaxLength)]
        [Comment("Ad name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(AdDescriptionMaxLength)]
        [Comment("Ad description")]

        public string Description { get; set; } = string.Empty;

        [Required]
        [Comment("Ad price")]

        public decimal Price { get; set; }

        [Required]
        [Comment("Owner identifier")]

        public string OwnerId { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(OwnerId))]
        public IdentityUser Owner { get; set; } = null!;

        [Required]
        [Comment("Ad image URL")]

        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [Comment("Ad creation time")]

        public DateTime CreatedOn { get; set; }

        [Required]
        [Comment("Category identifier")]

        public int CategoryId { get; set; }

        [Required]
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;
    }
}

