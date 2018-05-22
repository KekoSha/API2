using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API2.Data
{
    public class ApplicationUser:IdentityUser 
    {
        [Required]
        [StringLength(150)]
        public string Address { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime DateofBirth { get; set; }

        [Url] //validate the url 
        public string Url { get; set; }
    }
}
