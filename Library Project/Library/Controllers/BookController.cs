using Library.Data;
using Library.Models.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Library.Controllers
{
    [Authorize]
    public class BookController : Controller
    {

        private readonly LibraryDbContext data;

        public BookController(LibraryDbContext context)
        {
            data = context;
        }


        [HttpGet]
        public async Task<IActionResult> All()
        {
            var booksToDisplay = await data
                .Books
                .Select(b => new BookInfoViewModel()
                {
                    Id= b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Rating = b.Rating,
                    ImageUrl = b.ImageUrl,
                    Category = b.Category.Name
                })
                .ToListAsync();

            return View(booksToDisplay);
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            string currentUserId = GetUserId();

            var mineBooks = await data
                .IdentityUsersBooks
                .Where(ub => ub.CollectorId == currentUserId)
                .Select(ub => new MineBooksViewModel()
                {
                    Id = ub.BookId,
                    Description = ub.Book.Description,
                    Title = ub.Book.Title,
                    Author = ub.Book.Author,
                    ImageUrl= ub.Book.ImageUrl,
                    Category = ub.Book.Category.Name
                })
                .ToListAsync();

            return View(mineBooks);
        }


        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

    }
}
