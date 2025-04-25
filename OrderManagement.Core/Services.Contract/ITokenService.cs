using Microsoft.AspNetCore.Identity;
using OrderManagement.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Services.Contract
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(User user, UserManager<User> userManager);
    }
}
