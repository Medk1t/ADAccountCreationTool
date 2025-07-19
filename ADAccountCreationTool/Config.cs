using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace ADAccountCreationTool
{
    public class ADSettings
    {
        public string Domain { get; set; }
        public string LdapRoot { get; set; }
        public string NetLogonServer { get; set; }
        public string ExchangeServer { get; set; } // можно не использовать
    }

    public static class Config
    {
        public static ADSettings Settings { get; }

        static Config()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false);

            var config = builder.Build();
            Settings = config.GetSection("ADSettings").Get<ADSettings>();
        }
    }
}
