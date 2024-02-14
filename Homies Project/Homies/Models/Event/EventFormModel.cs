using Homies.Models.Type;
using System.ComponentModel.DataAnnotations;
using static Homies.Data.DataConstants;


namespace Homies.Models.Event
{
    public class EventFormModel
    {
        [Required]
        [StringLength(EventNameMaxLength, MinimumLength = EventNameMinLength,
            ErrorMessage = "Event {0} must be between {2} and {1} characters.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(EventDescriptionMaxLength, MinimumLength = EventsDescriptionMinLength,
            ErrorMessage = "Event {0} must be between {2} and {1} characters.")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Start { get; set; } = string.Empty;

        [Required]
        public string End { get; set; } = string.Empty;

        [Required]
        public int TypeId { get; set; }

        public IEnumerable<TypeViewModel> Types { get; set; } = new List<TypeViewModel>();
    }
}
