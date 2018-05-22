using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace API2_MVC_Interface.Models
{
    public class RoleViewModel:IdentityRole 
    {
        public string Id { get; set; }
        public string Name { get; set; }

    }
}
