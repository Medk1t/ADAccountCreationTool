using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADAccountCreationTool.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace ADAccountCreationTool.AppSettigs
{
    public class AppSettings : IAppSettings
    {
        public string Domain { get; set; } = "";
        public string NetLogonPath { get; set; } = "";
        public string? ExchangeServer { get; set; }
    }
}
