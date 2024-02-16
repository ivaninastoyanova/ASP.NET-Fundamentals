using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static SoftUniBazar.Data.DataConstants;

namespace SoftUniBazar.Data.Models
{
    public class Category
    {
        [Key]
        [Required]
        [Comment("Category identifier")]

        public int Id { get; set; }

        [Required]
        [MaxLength(CategoryNameMaxLength)]
        [Comment("Category name")]

        public string Name { get; set; } = string.Empty;

        public IList<Ad> Ads { get; set; } = new List<Ad>();
    }
}
