using ADAccountCreationTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountCreationTool.Interfaces
{
    public interface IADUserService
    {
        Task<bool> CreateUserAsync(UserModel user);
    }
}
