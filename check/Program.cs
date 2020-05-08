using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace check
{
    class Program
    {
        static void Main(string[] args)
        {
            var testes = GetUser();

            var teste = ToolsDataBase.SelectUsuarioByName("Lino Cesar Cutini");

            MessageBox.Show(testes);
        }

        static string GetUser()
        {
            var username = System.DirectoryServices.AccountManagement.UserPrincipal.Current.DisplayName;

            var teste = Environment.UserName;

            return username;
        }
    }
}
