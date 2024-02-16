using SoftUniBazar.Models.Category;
using System.ComponentModel.DataAnnotations;
using static SoftUniBazar.Data.DataConstants;

namespace SoftUniBazar.Models.Ad
{
    public class AdFormViewModel
    {
        [Required]
        [StringLength(AdNameMaxLength, MinimumLength = AdNameMinLength,
           ErrorMessage = "Ad {0} must be between {2} and {1} characters.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(AdDescriptionMaxLength, MinimumLength = AdDescriptionMinLength,
            ErrorMessage = "Ad {0} must be between {2} and {1} characters.")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string ImageUrl { get; set; } = string.Empty;


        [Required]
        public int CategoryId { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }
}
