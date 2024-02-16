using System.ComponentModel.DataAnnotations;
using static Watchlist.Data.DataConstants;

namespace Watchlist.Data.Models
{
    public class Genre
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(GenreNameMaxLength)]
        public string Name { get; set; } = string.Empty;

        public IList<Movie> Movies { get; set; } = new List<Movie>();
    }
}
