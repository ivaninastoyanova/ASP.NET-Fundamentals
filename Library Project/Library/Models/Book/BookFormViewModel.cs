using Library.Models.Category;
using System.ComponentModel.DataAnnotations;
using static Library.Data.DataConstants;

namespace Library.Models.Book
{
    public class BookFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(BookTitleMaxLength, MinimumLength = BookTitleMinLength,
            ErrorMessage = "Book {0} must be between {2} and {1} characters.")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(BookAuthorMaxLength, MinimumLength = BookAuthorMinLength,
            ErrorMessage = "Book {0} must be between {2} and {1} characters.")]
        public string Author { get; set; } = string.Empty;

        [Required]
        [Range(BookRatingMinValue, BookRatingMaxValue, 
            ErrorMessage = "The {0} must be between 0.00 and 10.00")]
        public decimal Rating { get; set; }


        [Required(AllowEmptyStrings = false)]
        public string Url { get; set; } = string.Empty;

        [Required]
        [StringLength(BookDescriptionMaxLength, MinimumLength = BookDescriptionMinLength,
            ErrorMessage = "Book {0} must be between {2} and {1} characters.")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }
}
