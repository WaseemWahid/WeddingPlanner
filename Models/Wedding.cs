using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId { get; set; }

        [Required(ErrorMessage = "is required.")]
        [MinLength(2, ErrorMessage = "must be atleast 2 characters.")]
        [Display(Name = "Wedder One")]
        public string WedderOne { get;set; }

        [Required(ErrorMessage = "is required.")]
        [MinLength(2, ErrorMessage = "must be atleast 2 characters.")]
        [Display(Name = "Wedder Two")]
        public string WedderTwo { get;set; }

        [Required(ErrorMessage = "is required.")]
        [FutureDate]
        [Display(Name = "Date")]
        
        public DateTime WeddingDate { get;set; }

        [Required(ErrorMessage = "is required")]
        [MinLength(5, ErrorMessage = "Must be a valid address")]
        [Display(Name = "Wedding Address")]

        public string WeddingAddress { get;set; }



        public DateTime CreatedAt { get;set; } = DateTime.Now;

        public DateTime UpdatedAt { get;set; } = DateTime.Now;

        /* Relationships */

        // Foreign Keys
        public int UserId { get;set; } 


        // // Navigation Properties (related class instances) - MUST use .Include to access.
        public User Author { get; set; }
        public List<Attendance> WeddingAttendees { get;set; } //The list of Users who are attending this Wedding



        public class FutureDateAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (((DateTime)value) <= DateTime.Today)
                {
                    return new ValidationResult("Only dates in the future are allowed!");
                }
                return ValidationResult.Success;
            }
        }
    }
}