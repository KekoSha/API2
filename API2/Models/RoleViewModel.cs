using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API2.Models
{
    public class RoleViewModel : IdentityRole 
    {
        private string id;

        public string GetId()
        {
            return id;
        }

        public void SetId(string value)
        {
            id = value;
        }

        private string name;

        public string GetName()
        {
            return name;
        }

        public void SetName(string value)
        {
            name = value;
        }
    }
}
