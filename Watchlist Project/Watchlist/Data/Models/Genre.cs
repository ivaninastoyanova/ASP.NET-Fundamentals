using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Watchlist.Data.DataConstants;

namespace Watchlist.Data.Models
{
    public class Genre
    {
        [Key]
        [Required]
        [Comment("Genre identifier")]
        public int Id { get; set; }

        [Required]
        [MaxLength(GenreNameMaxLength)]
        [Comment("Genre name")]
        public string Name { get; set; } = string.Empty;

        public IList<Movie> Movies { get; set; } = new List<Movie>();
    }
}
