using Watchlist.Data;
using Watchlist.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;
using System.Xml.Linq;
using Watchlist.Models.Movie;

namespace Watchlist.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private readonly WatchlistDbContext data;

        public MovieController(WatchlistDbContext context)
        {
            data = context;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var moviesToDisplay = await data
                .Movies
                .Select(m => new MovieInfoViewModel()
                {
                    Id = m.Id,
                    Title = m.Title,
                    Director = m.Director,
                    Rating = m.Rating,
                    Genre = m.Genre.Name,
                    ImageUrl = m.ImageUrl
                })
                .ToListAsync();

            return View(moviesToDisplay);
        }
    }
}
