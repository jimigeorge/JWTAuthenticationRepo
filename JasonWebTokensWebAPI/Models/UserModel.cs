using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace JsonWebTokenAuthentication.API.Models
{
    public class UserModel
    {
        [Required]
        [Display(Name ="User Name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100,ErrorMessage ="The {0} must be atleast {2} characters long",MinimumLength =6)]
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name ="Confirm Password")]
        [Compare("Password", ErrorMessage ="The Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}