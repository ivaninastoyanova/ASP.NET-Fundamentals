using System.ComponentModel.DataAnnotations;
using Watchlist.Models.Genre;
using static Watchlist.Data.DataConstants;

namespace Watchlist.Models.Movie
{
    /// <summary>
    /// Form model for adding new movies to the app
    /// </summary>
    public class AddMovieViewModel
    {
        [Required]
        [StringLength(MovieTitleMaxLength, MinimumLength = MovieTitleMinLength,
            ErrorMessage = "Event {0} must be between {2} and {1} characters.")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(MovieDirectorMaxLength, MinimumLength = MovieDirectorMinLength,
            ErrorMessage = "Event {0} must be between {2} and {1} characters.")]
        public string Director { get; set; } = string.Empty;

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [Range(0.00,10.00)]
        public decimal Rating { get; set; }

        [Required]
        public int GenreId { get; set; }

        public IEnumerable<GenreViewModel> Genres { get; set; } = new List<GenreViewModel>();


    }
}
