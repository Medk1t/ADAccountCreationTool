using ADAccountCreationTool.Interfaces;
using ADAccountCreationTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountCreationTool.Services
{
    public class RealADUserService : IADUserService
    {
        private readonly IAppSettings _settings;

        public RealADUserService(IAppSettings settings)
        {
            _settings = settings;
        }

        public async Task<bool> CreateUserAsync(UserModel user)
        {
            // Реальная логика взаимодействия с AD через DirectoryEntry
            // ...
            return await Task.FromResult(true);
        }
    }
}
