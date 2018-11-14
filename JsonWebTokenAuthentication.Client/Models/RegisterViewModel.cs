using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JsonWebTokenAuthentication.Client.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "User Name is required")]
        [Display(Name ="User Name")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Use letters or digits only please")]
        [StringLength(10)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(12, ErrorMessage = "Password Must be between 6 and 12 characters", MinimumLength = 6)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(12, ErrorMessage = "Confirm Password Must be between 6 and 12 characters", MinimumLength = 6)]
        [Display(Name ="Confirm Password")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}