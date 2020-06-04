using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VotingUI.Models.Account
{
    public class ModelUserView
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(256)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        public string Gender { get; set; } = "Male";
        public string[] Genders = new[] { "Male", "Female" };

        [Range(10, 100, ErrorMessage = "Age allowed minimum must be {1} and maximum {2}")]
        public int Age { get; set; }

        public int RoleId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password must be a combination of upper case, lower case, number, and at least 8 characters long")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
