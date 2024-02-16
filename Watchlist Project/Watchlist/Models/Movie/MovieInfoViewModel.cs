namespace Watchlist.Models.Movie
{
    /// <summary>
    /// Movie view model that is used in the following pages:
    /// All movies
    /// Watched movies
    /// </summary>
    public class MovieInfoViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Director { get; set; } = string.Empty;

        public decimal Rating { get; set; }

        public string Genre { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
