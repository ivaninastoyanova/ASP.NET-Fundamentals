using Library.Data;
using Library.Data.Models;
using Library.Models.Book;
using Library.Models.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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

        [HttpPost]
        public async Task<IActionResult> AddToCollection(int id)
        {
            var currentBook = await data
                .Books
                .FindAsync(id);

            if (currentBook == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();

            var entryToAdd = new IdentityUserBook()
            {
                BookId = currentBook.Id,
                CollectorId = currentUserId,
            };

            if (await data.IdentityUsersBooks.ContainsAsync(entryToAdd))
            {
                return RedirectToAction("All", "Book");
            }

            await data.IdentityUsersBooks.AddAsync(entryToAdd);

            await data.SaveChangesAsync();

            return RedirectToAction("All", "Book");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCollection(int id)
        {
            var bookId = id;
            var bookToRemove = await data
                .Books
                .FindAsync(bookId);

            if (bookToRemove == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();

            var entryToRemove = await data.IdentityUsersBooks
                .FirstOrDefaultAsync(ub => ub.CollectorId == currentUserId && ub.BookId == bookId);

            if (entryToRemove == null)
            {
                return BadRequest();
            }

            data.IdentityUsersBooks.Remove(entryToRemove);

            await data.SaveChangesAsync();

            return RedirectToAction("Mine", "Book");
        }


        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var bookModel = new BookFormViewModel();

            var categories = await GetCategories();

            bookModel.Categories = categories;

            return View(bookModel);
        }


        [HttpPost]
        public async Task<IActionResult> Add(BookFormViewModel bookModel)
        {
            string currentUserId = GetUserId();

            var categories = await GetCategories();

            if (!categories.Any(c => c.Id == bookModel.CategoryId))
            {
                ModelState.AddModelError(nameof(bookModel.Id), "Category does not exist!");
            }

            //In case there is a problem with the decimal value - we can take it as string from the form
            //Input value should be string in the form for creating a new book.
            //In the database the value is decimal

            //decimal rating;

            //if (!decimal.TryParse(bookModel.Rating, out rating) || rating < 0 || rating > 10)
            //{
            //    ModelState.AddModelError(nameof(bookModel.Rating), "Rating must be a number between 0 and 10.");

            //    bookModel.Categories = categories;

            //    return View(bookModel);
            //}


            if (!ModelState.IsValid)
            {
                bookModel.Categories = categories;
                return View(bookModel);
            }


            var bookToAdd = new Book()
            {
                Title = bookModel.Title,
                Author = bookModel.Author,
                Description = bookModel.Description,
                ImageUrl = bookModel.Url,
                Rating = bookModel.Rating,
                CategoryId = bookModel.CategoryId,
            };

            await data.Books.AddAsync(bookToAdd);
            await data.SaveChangesAsync();

            return RedirectToAction("All", "Book");
        }


        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        private async Task<IEnumerable<CategoryViewModel>> GetCategories()
        {
            var categories = await data
                        .Categories
                         .Select(c => new CategoryViewModel
                         {
                             Name = c.Name,
                             Id = c.Id,
                         })
                         .ToListAsync();

            return categories;
        }

    }
}
