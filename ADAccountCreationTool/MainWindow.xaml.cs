using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ADAccountCreationTool
{
    public partial class MainWindow : Window
    {
        private readonly string domain = "domain.local";
        private bool isAdAvailable = false;

        public MainWindow()
        {
            InitializeComponent();
            LoadOrganizationalUnits();
        }

        private void LoadOrganizationalUnits()
        {
            try
            {
                string ldapRoot = $"LDAP://DC={domain.Replace(".", ",DC=")}";
                using (DirectoryEntry root = new DirectoryEntry(ldapRoot))
                {
                    var ous = new List<string>();
                    GetOUsRecursive(root, "", ous);
                    if (ous.Count > 0)
                    {
                        OuComboBox.ItemsSource = ous;
                        OuComboBox.SelectedIndex = 0;
                        isAdAvailable = true;
                    }
                }
            }
            catch
            {
                OuComboBox.ItemsSource = new List<string> { "AD недоступен (тестовый режим)" };
                OuComboBox.SelectedIndex = 0;
                isAdAvailable = false;
            }
        }

        private void GetOUsRecursive(DirectoryEntry entry, string pathPrefix, List<string> result)
        {
            foreach (DirectoryEntry child in entry.Children)
            {
                if (child.SchemaClassName == "organizationalUnit")
                {
                    string dn = child.Properties["distinguishedName"].Value.ToString();
                    result.Add(dn);
                    GetOUsRecursive(child, pathPrefix + "  ", result);
                }
            }
        }

        private void UpdateLoginPreview(object sender, RoutedEventArgs e)
        {
            string fn = Transliterate(FirstNameBox.Text.Trim());
            string mn = Transliterate(MiddleNameBox.Text.Trim());
            string ln = Transliterate(LastNameBox.Text.Trim());

            string initials = (fn.Length > 0 ? fn[0].ToString() : "") +
                              (mn.Length > 0 ? mn[0].ToString() : "") +
                              (ln.Length > 0 ? ln[0].ToString() : "");

            string dept = DepartmentNumberBox.Text.Trim();
            LoginBox.Text = string.IsNullOrWhiteSpace(dept) ? initials.ToLower() : $"{dept}-{initials.ToLower()}";
        }

        private string Transliterate(string input)
        {
            var map = new Dictionary<char, string>()
            {
                {'а',"a"},{'б',"b"},{'в',"v"},{'г',"g"},{'д',"d"},{'е',"e"},{'ё',"e"},
                {'ж',"zh"},{'з',"z"},{'и',"i"},{'й',"y"},{'к',"k"},{'л',"l"},{'м',"m"},
                {'н',"n"},{'о',"o"},{'п',"p"},{'р',"r"},{'с',"s"},{'т',"t"},{'у',"u"},
                {'ф',"f"},{'х',"kh"},{'ц',"ts"},{'ч',"ch"},{'ш',"sh"},{'щ',"shch"},
                {'ъ',""},{'ы',"y"},{'ь',""},{'э',"e"},{'ю',"yu"},{'я',"ya"}
            };

            var sb = new StringBuilder();
            input = input.ToLower();

            foreach (char c in input)
                sb.Append(map.ContainsKey(c) ? map[c] : c.ToString());

            return sb.ToString();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameBox.Text.Trim();
            string middleName = MiddleNameBox.Text.Trim();
            string lastName = LastNameBox.Text.Trim();
            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Password;
            string scriptPath = ScriptPathBox.Text.Trim();
            string groupName = GroupBox.Text.Trim();

            if (!isAdAvailable)
            {
                ResultText.Text = "⚠️ Active Directory недоступен. Пользователь не будет создан.";
                ResultText.Foreground = System.Windows.Media.Brushes.DarkOrange;
                return;
            }

            string fullName = $"{firstName} {middleName} {lastName}";
            string upn = $"{login}@{domain}";
            string selectedOuDn = OuComboBox.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedOuDn) || selectedOuDn.Contains("недоступен"))
            {
                ResultText.Text = "❌ Выберите допустимый OU.";
                ResultText.Foreground = System.Windows.Media.Brushes.DarkRed;
                return;
            }

            try
            {
                string ouPath = "LDAP://" + selectedOuDn;
                DirectoryEntry ou = new DirectoryEntry(ouPath);
                DirectoryEntry newUser = ou.Children.Add($"CN={fullName}", "user");

                newUser.Properties["samAccountName"].Value = login;
                newUser.Properties["userPrincipalName"].Value = upn;
                newUser.Properties["givenName"].Value = firstName;
                newUser.Properties["sn"].Value = lastName;
                newUser.Properties["displayName"].Value = fullName;
                newUser.Properties["scriptPath"].Value = scriptPath;

                newUser.CommitChanges();
                newUser.Invoke("SetPassword", new object[] { password });
                newUser.Properties["userAccountControl"].Value = 0x200;
                newUser.CommitChanges();

                using (var ctx = new PrincipalContext(ContextType.Domain, domain))
                {
                    var user = UserPrincipal.FindByIdentity(ctx, login);
                    var group = GroupPrincipal.FindByIdentity(ctx, groupName);
                    if (user != null && group != null)
                    {
                        group.Members.Add(user);
                        group.Save();
                    }
                }

                ResultText.Text = $"✅ Пользователь {login} успешно создан.";
                ResultText.Foreground = System.Windows.Media.Brushes.DarkGreen;
            }
            catch (Exception ex)
            {
                ResultText.Text = "❌ Ошибка при создании: " + ex.Message;
                ResultText.Foreground = System.Windows.Media.Brushes.DarkRed;
            }
        }
    }
}
