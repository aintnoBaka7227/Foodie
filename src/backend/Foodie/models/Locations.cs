using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Foodie.Models
{
    public class Locations
    {
        [Key]
        [Display(Name = "Location ID")]
        public int LocationId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Location Name")]
        public string LocationName { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(Users))]
        public int UserId { get; set; }
        
        public Users User { get; set; } = null!;
        public ICollection<FoodItems> FoodItems { get; set; } = new List<FoodItems>();
    }

}