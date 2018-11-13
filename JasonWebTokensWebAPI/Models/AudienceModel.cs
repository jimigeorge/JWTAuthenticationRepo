using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace JsonWebTokenAuthentication.API.Models
{
    public class AudienceModel
    {
        [MaxLength(100)]
        [Required]
        public string name { get; set; }
    }
}