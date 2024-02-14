﻿using System.ComponentModel.DataAnnotations;
using static Homies.Data.DataConstants;

namespace Homies.Data.Models
{
    public class Type
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(TypeNameMaxLength)]
        public string Name { get; set; } = string.Empty;

        public IList<Event> Events { get; set; } = new List<Event>();

    }
}
