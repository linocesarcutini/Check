using System;
using System.Net;
using Microsoft.Win32;
using System.Net.Sockets;

namespace check
{
    class Program
    {
        static void Main(string[] args)
        {
            var serialHd = Identifier("Win32_DiskDrive", "SerialNumber");

            var macAddress = Identifier("Win32_NetworkAdapterConfiguration", "MacAddress");

            var teste = ToolsDataBase.SelectUsuarioByName(GetUser());

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

                    Ativacao("false");
                }
                catch
                {
                }
            }

            else
            {
                try
                {
                    if (!(teste.Date.Date >= DateNow().Date && teste.UserName == GetUser() && teste.Ativacao == "True"))
                    {
                        Ativacao("false");
                    }
                }

                catch
                {
                }

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

                        if (result != "")
                        {
                            return result;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            return result;
        }

        private static string Identifier(string wmiClass, string wmiProperty)
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

        static void Ativacao(string situacao)
        {
            string autocadAtiv;

            if (situacao == "true")
            {
                try
                {
                    string autocad = @"HKEY_CURRENT_USER\SOFTWARE\Autodesk\AutoCAD\";
                    string autocadCurver = (string) Registry.GetValue(autocad, "CurVer", "");
                    autocad += autocad + @"\" + autocadCurver + @"\";
                    string autocadLang = (string) Registry.GetValue(autocad, "CurVer", "");
                    autocad += autocad + @"\" + autocadCurver + @"\" + autocadLang + @"\Applications\";
                    autocadAtiv = (string) Registry.GetValue(autocad, "1", "");

                    Registry.SetValue(autocad, autocadAtiv, "1");
                }
                catch (System.Exception)
                {
                    string autocad = @"HKEY_CURRENT_USER\SOFTWARE\Autodesk\AutoCAD\";
                    string autocadCurver = (string)Registry.GetValue(autocad, "CurVer", "");
                    autocad += autocad + @"\" + autocadCurver + @"\";
                    string autocadLang = (string)Registry.GetValue(autocad, "CurVer", "");
                    autocad += autocad + @"\" + autocadCurver + @"\" + autocadLang + @"\Applications\";
                    autocadAtiv = (string)Registry.GetValue(autocad, "1", "");

                    Registry.SetValue(autocad, autocadAtiv, "0");
                }
            }

            if (situacao == "false")
            {
                try
                {
                    string autocad = @"HKEY_CURRENT_USER\SOFTWARE\Autodesk\AutoCAD\";
                    string autocadCurver = (string)Registry.GetValue(autocad, "CurVer", "");
                    autocad += autocad + @"\" + autocadCurver + @"\";
                    string autocadLang = (string)Registry.GetValue(autocad, "CurVer", "");
                    autocad += autocad + @"\" + autocadCurver + @"\" + autocadLang + @"\Applications\";
                    autocadAtiv = (string)Registry.GetValue(autocad, "1", "");

                    Registry.SetValue(autocad, autocadAtiv, "0");
                }
                catch (System.Exception)
                {
                }
            }
        }
    }
}
