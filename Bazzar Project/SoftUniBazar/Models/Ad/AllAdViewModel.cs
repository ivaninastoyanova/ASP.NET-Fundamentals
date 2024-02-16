using Microsoft.AspNetCore.Identity;
using SoftUniBazar.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoftUniBazar.Models.Ad
{
    /// <summary>
    /// Add all view model which is used when displaying the ads on pages: 
    /// 1. all ads
    /// 2. cart/my ads
    /// </summary>
    public class AllAdViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Owner { get; set; } = null!;

        public string ImageUrl { get; set; } = string.Empty;

        public string CreatedOn { get; set; } = string.Empty;

        public string Category { get; set; } = null!;
    }
}
