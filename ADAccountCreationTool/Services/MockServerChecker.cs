using ADAccountCreationTool.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountCreationTool.Services
{
    public class MockServerChecker : IServerChecker
    {
        public Task<bool> IsDomainAvailableAsync() => Task.FromResult(true);
        public Task<bool> IsNetlogonAvailableAsync() => Task.FromResult(true);
        public Task<bool> IsExchangeAvailableAsync() => Task.FromResult(true);
    }

}
