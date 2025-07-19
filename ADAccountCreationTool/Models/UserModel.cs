using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAccountCreationTool.Models
{
    public class UserModel
    {
        public string FirstName { get; set; }           // Имя
        public string MiddleName { get; set; }          // Отчество
        public string LastName { get; set; }            // Фамилия
        public string DepartmentNumber { get; set; }    // Номер отдела

        public string Login => $"{DepartmentNumber} - {LoginInitials}";
        public string LoginInitials => Transliterate($"{FirstName[0]}{MiddleName[0]}{LastName[0]}");

        public string Password { get; set; }            // Пароль
        public string ScriptPath { get; set; }          // Путь сценария входа
        public string GroupName { get; set; }           // Имя группы (AD Group)

        public string Email { get; set; }               // Почта, если используется MS Exchange
        public string OUPath { get; set; }              // Полный путь в AD (например, OU=отдел 288,...)

        private static string Transliterate(string input)
        {
            var map = new Dictionary<char, string>
            {
                {'а',"a"},{'б',"b"},{'в',"v"},{'г',"g"},{'д',"d"},{'е',"e"},{'ё',"e"},
                {'ж',"zh"},{'з',"z"},{'и',"i"},{'й',"i"},{'к',"k"},{'л',"l"},{'м',"m"},
                {'н',"n"},{'о',"o"},{'п',"p"},{'р',"r"},{'с',"s"},{'т',"t"},{'у',"u"},
                {'ф',"f"},{'х',"kh"},{'ц',"ts"},{'ч',"ch"},{'ш',"sh"},{'щ',"shch"},
                {'ы',"y"},{'э',"e"},{'ю',"yu"},{'я',"ya"}, {'ь', ""}, {'ъ', ""}
            };

            return string.Concat(input.ToLower().Select(c => map.ContainsKey(c) ? map[c] : c.ToString()));
        }
    }
}
