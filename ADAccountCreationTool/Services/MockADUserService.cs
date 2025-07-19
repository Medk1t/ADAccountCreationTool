using ADAccountCreationTool.Interfaces;
using ADAccountCreationTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountCreationTool.Services
{
    public class MockADUserService : IADUserService
    {
        public Task<bool> CreateUserAsync(UserModel user)
        {
            Console.WriteLine($"[MOCK] Создание пользователя: {user.Login}");
            return Task.FromResult(true);
        }
    }
}
