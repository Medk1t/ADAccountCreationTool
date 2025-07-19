using ADAccountCreationTool.AppSettigs;
using ADAccountCreationTool.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountCreationTool.Services
{
    public class RealServerChecker : IServerChecker
    {
        private readonly AppSettings _settings;

        public RealServerChecker(AppSettings settings)
        {
            _settings = settings;
        }

        public Task<bool> IsDomainAvailableAsync() => PingServerAsync(_settings.Domain);
        public Task<bool> IsNetlogonAvailableAsync() => PingServerAsync(_settings.NetLogonPath);
        public Task<bool> IsExchangeAvailableAsync()
        {
            if (string.IsNullOrWhiteSpace(_settings.ExchangeServer))
                return Task.FromResult(true); // Exchange опционален
            return PingServerAsync(_settings.ExchangeServer);
        }

        private static async Task<bool> PingServerAsync(string host)
        {
            try
            {
                using var ping = new Ping();
                var reply = await ping.SendPingAsync(host, 1500);
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
