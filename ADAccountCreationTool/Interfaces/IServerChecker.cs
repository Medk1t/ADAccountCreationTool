using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountCreationTool.Interfaces
{
    public interface IServerChecker
    {
        Task<bool> IsDomainAvailableAsync();
        Task<bool> IsNetlogonAvailableAsync();
        Task<bool> IsExchangeAvailableAsync();
    }
}
