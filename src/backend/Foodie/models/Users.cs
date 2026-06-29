using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Foodie.Models
{
    [Index(nameof(email), IsUnique = true)]
    public class Users
    {
        [Key]
        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [Display(Name = "Password")]
        public string password { get; set; } = string.Empty;
        
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        public ICollection<Locations> Locations { get; set; } = new List<Locations>();
        public ICollection<FoodItems> FoodItems { get; set; } = new List<FoodItems>();
    }
}