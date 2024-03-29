﻿using Watchlist.Data;
using Watchlist.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;
using System.Xml.Linq;
using Watchlist.Models.Movie;
using Watchlist.Models.Genre;

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

        [HttpGet]
        public async Task<IActionResult> Watched()
        {
            string currentUserId = GetUserId();

            var userMovies = await data
                .UsersMovies
                .Where(um => um.UserId == currentUserId)
                .Select(um => new MovieInfoViewModel()
                {
                    Id = um.MovieId,
                    Title = um.Movie.Title,
                    Director = um.Movie.Director,
                    Rating = um.Movie.Rating,
                    Genre = um.Movie.Genre.Name,
                    ImageUrl = um.Movie.ImageUrl
                })
                .ToListAsync();

            return View(userMovies);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCollection(int id)
        {
            var currentMovie = await data
                .Movies
                .FindAsync(id);

            if (currentMovie == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();

            var entryToAdd = new UserMovie()
            {
                MovieId = currentMovie.Id,
                UserId = currentUserId,
            };

            if (await data.UsersMovies.ContainsAsync(entryToAdd))
            {
                return RedirectToAction("All", "Movie");
            }

            await data.UsersMovies.AddAsync(entryToAdd);
           
            await data.SaveChangesAsync();

            return RedirectToAction("All", "Movie");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCollection(int id)
        {
            var movieId = id;
            var movieToRemove = await data
                .Movies
                .FindAsync(movieId);

            if (movieToRemove == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();

            var entryToRemove = await data.UsersMovies
                .FirstOrDefaultAsync(um => um.UserId == currentUserId && um.MovieId == movieId);

            if (entryToRemove == null)
            {
                return BadRequest();
            }

            data.UsersMovies.Remove(entryToRemove);

            await data.SaveChangesAsync();

            return RedirectToAction("Watched", "Movie");
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var movieModel = new AddMovieViewModel();

            var genres = await GetGenres();

            movieModel.Genres = genres;

            return View(movieModel);
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddMovieViewModel movieModel)
        {
            string currentUserId = GetUserId();

            var genres = await GetGenres();

            if (!genres.Any(t => t.Id == movieModel.GenreId))
            {
                ModelState.AddModelError(nameof(movieModel.GenreId), "Genre does not exist!");
            }

            if (!ModelState.IsValid)
            {
                movieModel.Genres = genres;
                return View(movieModel);
            }

            var movieToAdd = new Movie()
            {
               Title = movieModel.Title,
               Director = movieModel.Director,
               ImageUrl = movieModel.ImageUrl,
               GenreId = movieModel.GenreId,
               Rating = movieModel.Rating,
            };

            await data.Movies.AddAsync(movieToAdd);
            await data.SaveChangesAsync();

            return RedirectToAction("All", "Movie");
        }


        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        private async Task<IEnumerable<GenreViewModel>> GetGenres()
        {
            var genres = await data
                        .Genres
                         .Select(t => new GenreViewModel
                         {
                             Name = t.Name,
                             Id = t.Id,
                         })
                         .ToListAsync();

            return genres;
        }
    }
}
