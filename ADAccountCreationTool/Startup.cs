using ADAccountCreationTool.AppSettigs;
using ADAccountCreationTool.Interfaces;
using ADAccountCreationTool.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountCreationTool
{
    public static class Startup
    {
        public static ServiceProvider ConfigureServices(bool useMocks)
        {
            var services = new ServiceCollection();

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var settings = config.GetSection("ADSettings").Get<AppSettings>();
            services.AddSingleton<IAppSettings>(settings);

            if (useMocks)
            {
                services.AddSingleton<IADUserService, MockADUserService>();
                services.AddSingleton<IServerChecker, MockServerChecker>();
            }
            else
            {
                services.AddSingleton<IADUserService, RealADUserService>();
                services.AddSingleton<IServerChecker, RealServerChecker>();
            }

            return services.BuildServiceProvider();
        }
    }
}
