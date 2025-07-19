using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ADAccountCreationTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider Services;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool useMocks = true; // ← ставить false для прод
            Services = ADAccountCreationTool.Startup.ConfigureServices(useMocks);

            new SplashScreen().Show();
        }
    }
}
