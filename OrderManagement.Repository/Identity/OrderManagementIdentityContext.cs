using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Repository.Identity
{
    public class OrderManagementIdentityContext : IdentityDbContext<User>
    {
        public OrderManagementIdentityContext(DbContextOptions<OrderManagementIdentityContext> options): base(options)
        {
            
        }
    }
}
