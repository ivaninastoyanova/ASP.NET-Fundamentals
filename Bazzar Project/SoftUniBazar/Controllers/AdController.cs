using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftUniBazar.Data;
using SoftUniBazar.Data.Models;
using SoftUniBazar.Models.Ad;
using System.Security.Claims;

namespace SoftUniBazar.Controllers
{
    [Authorize]
    public class AdController : Controller
    {
        private readonly BazarDbContext data;

        public AdController(BazarDbContext context)
        {
            data = context;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var adsToDisplay = await data
                .Ads
                .Select(a => new AllAdViewModel()
                { 
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Price = a.Price,
                    ImageUrl = a.ImageUrl,
                    CreatedOn = a.CreatedOn.ToString(DataConstants.DateFormat),
                    Category = a.Category.Name,
                    Owner = a.Owner.UserName,
                })
                .ToListAsync();

            return View(adsToDisplay);
        }

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            string currentUserId = GetUserId();

            var buyerAds = await data
                .AdsBuyers
                .Where(ab => ab.BuyerId == currentUserId)
                .Select(ab => new AllAdViewModel()
                {
                    Id = ab.Ad.Id,
                    Name = ab.Ad.Name,
                    Description = ab.Ad.Description,
                    Price = ab.Ad.Price,
                    ImageUrl = ab.Ad.ImageUrl,
                    CreatedOn = ab.Ad.CreatedOn.ToString(DataConstants.DateFormat),
                    Category = ab.Ad.Category.Name,
                    Owner = ab.Ad.Owner.UserName,
                })
                .ToListAsync();

            return View(buyerAds);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            var currentEvent = await data
                .Ads
                .FindAsync(id);

            if (currentEvent == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();

            var entryToAdd = new AdBuyer()
            {
                AdId = currentEvent.Id,
                BuyerId = currentUserId,
            };

            if (await data.AdsBuyers.ContainsAsync(entryToAdd))
            {
                return RedirectToAction("All", "Ad");
            }

            await data.AdsBuyers.AddAsync(entryToAdd);

            await data.SaveChangesAsync();

            return RedirectToAction("Cart", "Ad");
        }


        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var adId = id;
            var adToRemove = await data
                .Ads
                .FindAsync(adId);

            if (adToRemove == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();

            var entryToRemove = await data.AdsBuyers
                .FirstOrDefaultAsync(ab => ab.BuyerId == currentUserId && ab.AdId == adId);

            if (entryToRemove == null)
            {
                return BadRequest();
            }

            data.AdsBuyers.Remove(entryToRemove);

            await data.SaveChangesAsync();

            return RedirectToAction("All", "Ad");
        }


        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

    }
}
