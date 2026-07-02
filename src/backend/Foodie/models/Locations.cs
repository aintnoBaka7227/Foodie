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
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        
        public User User { get; set; } = null!;
        public ICollection<FoodItem> FoodItems { get; set; } = new List<FoodItem>();
    }

}