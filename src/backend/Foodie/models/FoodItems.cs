using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;       

namespace Foodie.Models
{
    public class FoodItem
    {
        // Data annotations for the FoodItem model
        [Key]
        [Display(Name = "Food Item ID")]
        public int FoodItemId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Food Item Name")]
        public string FoodItemName { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Quantity")]
        public decimal Quantity { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Unit")]
        public string Unit { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; }

        
        [ForeignKey(nameof(Location))]
        public int? LocationId { get; set; }
        
        public Location? Location { get; set; } = null!;

        public Guid UserId { get; set; }
    }
}