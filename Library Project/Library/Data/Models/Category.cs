﻿using System.ComponentModel.DataAnnotations;
using static Library.Data.DataConstants;

namespace Library.Data.Models
{
    public class Category
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; } = string.Empty;

        public List<Book> Books { get; set; } = new List<Book>();
    }
}
