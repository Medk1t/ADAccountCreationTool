using System.Windows;
using System.Windows.Controls;
using System.Text;
using ADAccountCreationTool.Interfaces;
using ADAccountCreationTool.Models;

namespace ADAccountCreationTool
{
    public partial class MainWindow : Window
    {
        private readonly IADUserService _userService;
        private readonly IADRepository _adRepository;
        private readonly IConfigProvider _configProvider;

        public MainWindow(IUserService userService, IADRepository adRepository, IConfigProvider configProvider)
        {
            InitializeComponent();

            _userService = userService;
            _adRepository = adRepository;
            _configProvider = configProvider;

            LoadData();
            AttachEvents();
        }

        private async void LoadData()
        {
            var ous = await _adRepository.GetOrganizationalUnitsAsync();
            var groups = await _adRepository.GetGroupsAsync();

            OuComboBox.ItemsSource = ous;
            GroupComboBox.ItemsSource = groups;
        }

        private void AttachEvents()
        {
            FirstNameBox.TextChanged += UpdateLoginPreview;
            MiddleNameBox.TextChanged += UpdateLoginPreview;
            LastNameBox.TextChanged += UpdateLoginPreview;
            DepartmentBox.TextChanged += UpdateLoginPreview;
        }

        private void UpdateLoginPreview(object sender, TextChangedEventArgs e)
        {
            var first = FirstNameBox.Text.Trim();
            var middle = MiddleNameBox.Text.Trim();
            var last = LastNameBox.Text.Trim();
            var dept = DepartmentBox.Text.Trim();

            if (first.Length > 0 && last.Length > 0 && dept.Length > 0)
            {
                string translit = _userService.GenerateTransliteratedLogin(first, middle, last);
                LoginPreview.Text = $"{dept} - {translit}";
            }
            else
            {
                LoginPreview.Text = string.Empty;
            }
        }

        private async void Create_Click(object sender, RoutedEventArgs e)
        {
            var user = new UserModel
            {
                FirstName = FirstNameBox.Text,
                MiddleName = MiddleNameBox.Text,
                LastName = LastNameBox.Text,
                DepartmentNumber = DepartmentBox.Text,
                OrganizationalUnit = OuComboBox.SelectedItem?.ToString(),
                Group = GroupComboBox.SelectedItem?.ToString(),
                ScriptPath = ScriptPathBox.Text
            };

            user.Login = _userService.GenerateTransliteratedLogin(user.FirstName, user.MiddleName, user.LastName);
            user.Login = $"{user.DepartmentNumber} - {user.Login}";

            bool success = await _userService.CreateUserAsync(user);
            MessageBox.Show(success ? "Учетная запись создана." : "Ошибка создания.", "Результат");
        }
    }
}