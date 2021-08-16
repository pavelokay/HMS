using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMS.Domain.Models
{
    public class Role : IdentityRole
    {
        public Role() : base()
        { }

        public Role(string roleName)
        {
            Name = roleName;
        }
    }
}
