using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Sockets;

namespace check
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length > 0)
            {
                var teste = ToolsDataBase.SelectUsuarioByName(GetUser());

                if (teste == null)
                {
                    try
                    {
                        CreateFileTxt("false");
                    }
                    catch
                    {

                    }
                }

                else
                {
                    try
                    {
                        if (teste.Date.Date >= DateNow().Date && teste.UserName == GetUser() && teste.Name == GetUserAsDisplayed() && GetIP() == teste.IPAddress)
                        {
                            CreateFileTxt("true");
                        }
                    }

                    catch
                    {

                    }

                }

            }

            else
            {

            }

            
        }

        static DateTime DateNow()
        {
            return DateTime.Now;
        }

        static string GetUser()
        {
            return Environment.UserName;
        }

        static string GetUserAsDisplayed()
        {
            return System.DirectoryServices.AccountManagement.UserPrincipal.Current.DisplayName;
        }


        static string GetIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return string.Empty;
        }

        static void CreateFileTxt(string text)
        {
            File.WriteAllText(@"C:\DESENHO\EXE\check.txt", text);
        }
    }
}
