using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Watchlist.Data.DataConstants;
using Microsoft.EntityFrameworkCore;

namespace Watchlist.Data.Models
{
    public class Movie
    {
        [Key]
        [Required]
        [Comment("Movie identifier")]
        public int Id { get; set; }

        [Required]
        [MaxLength(MovieTitleMaxLength)]
        [Comment("Movie title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(MovieDirectorMaxLength)]
        [Comment("Movie director name")]
        public string Director { get; set; } = string.Empty;

        [Required]
        [Comment("Movie image")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [Range(0.00, 10.00)]
        [Comment("Movie rating")]
        public decimal Rating { get; set; }

        [Required]
        [Comment("Genre identifier")]
        public int GenreId { get; set; } 

        [Required]
        [ForeignKey(nameof(GenreId))]
        public Genre Genre { get; set; } = null!;

        public IList<UserMovie> UsersMovies { get; set; } = new List<UserMovie>();
    }
}
