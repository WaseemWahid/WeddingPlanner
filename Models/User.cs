using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "is required.")]
        [MinLength(2, ErrorMessage = "must be at least 2 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "is required.")]
        [MinLength(2, ErrorMessage = "must be at least 2 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "is required.")]
        [MinLength(8, ErrorMessage= "must be atleast 8 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped] // don't add to DB
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "doesn't match password")]
        [Display(Name = "Confirm Password")]
        public string PasswordConfirm { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<Attendance> WeddingAttendees {get;set;} //The list of Weddings that a User is attending

        public List<Wedding> Weddings { get; set; }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }
    }
}