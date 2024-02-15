using Library.Data;
using Library.Models.Book;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
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
    }
}
