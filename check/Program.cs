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
using Microsoft.Win32;

namespace check
{
    class Program
    {
        static void Main(string[] args)
        {
            var serialHd = identifier("Win32_DiskDrive", "SerialNumber");

            var macAddress = identifier("Win32_NetworkAdapterConfiguration", "MacAddress");

            //string nomeCompletoUser = GetUserAsDisplayed("Win32_UserAccount", "FullName");

            //RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

            //var names = key.GetValueNames();

            //var serialWindows = key.GetValue("DigitalProductId") as byte[];

            //var serialCPU = identifier("Win32_Product", "ProductID");

            //args = new string[1];

            //var user = GetUser();
            //var name = GetUserAsDisplayed();

            var teste = ToolsDataBase.SelectUsuarioByName(GetUser());

            //MessageBox.Show(args[0]);

            if (teste.UserName == null)
            {
                try
                {
                    User userNaoCadastrado = new User
                    {
                        UserName = GetUser(),
                        Date = DateTime.Now,
                        Ativacao = "0"
                    };

                    ToolsDataBase.InsertUsuario(userNaoCadastrado);

                    CreateFileTxt("false");

                    //MessageBox.Show("Deu Ruim1");
                }
                catch
                {

                }
            }

            else
            {
                try
                {
                    //GetIP() == teste.IPAddress &&
                    if (teste.Date.Date >= DateNow().Date && teste.UserName == GetUser() && teste.Ativacao == "True")
                    {
                        CreateFileTxt("true");

                        //MessageBox.Show("Tudo Certo!");
                    }

                    else
                    {
                        CreateFileTxt("false");
                    }
                }

                catch
                {

                }

            }

            //Console.WriteLine("serialHD: " + serialHd + " " + "MAC: " + macAddress);
            //Console.ReadKey();
        }

        static DateTime DateNow()
        {
            return DateTime.Now;
        }

        static string GetUser()
        {
            return Environment.UserName;
        }

        static string GetUserAsDisplayed(string wmiClass, string wmiProperty)
        {
            string result = "";
            System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                //Only get the first one
                if (result == "")
                {
                    try
                    {
                        result = mo[wmiProperty].ToString();

                        if(result != "")
                        {
                            return result;
                        }
                        //break;
                    }
                    catch
                    {
                    }
                }
            }
            return result;
            //return System.DirectoryServices.AccountManagement.UserPrincipal.Current.DisplayName;
        }

        private static string identifier(string wmiClass, string wmiProperty)
        //Return a hardware identifier
        {
            string result = "";
            System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                //Only get the first one
                if (result == "")
                {
                    try
                    {
                        result = mo[wmiProperty].ToString();
                        break;
                    }
                    catch
                    {
                    }
                }
            }
            return result;
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
