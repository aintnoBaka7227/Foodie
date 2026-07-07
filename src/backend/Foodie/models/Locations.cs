using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    public class Location
    {
        // Data annotations for the Location model
        [Key]
        [Display(Name = "Location ID")]
        public int LocationId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Location Name")]
        public string LocationName { get; set; } = string.Empty;

        [Required]
        public Guid UserId { get; set; }
        
        
        public ICollection<FoodItem> FoodItems { get; set; } = new List<FoodItem>();
    }

}