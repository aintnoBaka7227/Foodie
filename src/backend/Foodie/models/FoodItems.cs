using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;        

namespace Foodie.Models
{
    public class FoodItems
    {
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

        
        [ForeignKey(nameof(Locations))]
        public int? LocationId { get; set; }
        
        public Locations? Location { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Users))]
        public int UserId { get; set; }
        
        public Users User { get; set; } = null!;
    }
}