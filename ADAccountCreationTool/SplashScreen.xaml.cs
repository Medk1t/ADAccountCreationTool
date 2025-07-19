using ADAccountCreationTool.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ADAccountCreationTool
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var checker = App.Services.GetService<IServerChecker>();

            bool adAvailable = await checker.IsDomainAvailableAsync();
            UpdateStatus($"Контроллер домена: {(adAvailable ? "OK" : "Ошибка")}");

            bool netlogonAvailable = await checker.IsNetlogonAvailableAsync();
            UpdateStatus($"Netlogon: {(netlogonAvailable ? "OK" : "Ошибка")}");

            bool exchangeAvailable = await checker.IsExchangeAvailableAsync();
            UpdateStatus($"MS Exchange: {(exchangeAvailable ? "OK" : "Ошибка")}");

            await Task.Delay(500); // короткая задержка для визуального восприятия

            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void UpdateStatus(string message)
        {
            StatusText.Text = message;
            DoEvents();
            Task.Delay(400).Wait(); // задержка между проверками
        }

        private void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background,
                                                  new Action(delegate { }));
        }
    }
}
