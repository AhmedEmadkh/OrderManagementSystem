using Microsoft.AspNetCore.Identity;
using OrderManagement.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Entities.Identity
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}
