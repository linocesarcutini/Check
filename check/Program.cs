using System;
using System.Management;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace check
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static void Main(string[] args)
        {
            const int SW_HIDE = 0;
            //const int SW_SHOW = 5;
            var handle = GetConsoleWindow();

            // Ocultar
            ShowWindow(handle, SW_HIDE);

            // Mostrar
            //ShowWindow(handle, SW_SHOW);

            var macAddress = GetEnderecoMAC();

            var UserMac = ToolsDataBase.SelectUsuarioByUserMac(GetUser(), macAddress);

            if (UserMac.User == null || UserMac.MacAddress == null)
            {
                try
                {
                    Usuario userNaoCadastrado = new Usuario
                    {
                        Nome = "",
                        User = GetUser(),
                        MacAddress = macAddress,
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
                    if (UserMac.User != GetUser() && UserMac.Ativacao != "True")
                        Ativacao("false");
                    else if (UserMac.User == GetUser() && UserMac.Ativacao == "True")
                        Ativacao("true");
                    else if (UserMac.User == GetUser() && UserMac.Ativacao == "False")
                        Ativacao("false");
                }
                catch
                {
                }
            }
        }

        static string GetUser()
        {
            return Environment.UserName;
        }

        //private static string Identifier(string wmiClass, string wmiProperty)
        //{
        //    string result = "";
        //    System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
        //    System.Management.ManagementObjectCollection moc = mc.GetInstances();
        //    foreach (System.Management.ManagementObject mo in moc)
        //    {
        //        //Only get the first one
        //        if (result == "")
        //        {
        //            try
        //            {
        //                result = mo[wmiProperty].ToString();
        //                break;
        //            }
        //            catch
        //            {
        //            }
        //        }
        //    }
        //    return result;
        //}

        public static string GetEnderecoMAC()
        {
            try
            {
                ManagementObjectSearcher objMOS = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection objMOC = objMOS.Get();
                string enderecoMAC = String.Empty;
                foreach (ManagementObject objMO in objMOC)
                {
                    //retorna endereço MAC do primeiro cartão
                    if (enderecoMAC == String.Empty)
                    {
                        if (objMO["MacAddress"] != null)
                        {
                            enderecoMAC = objMO["MacAddress"].ToString();
                            break;
                        }
                    }
                    objMO.Dispose();
                }
                enderecoMAC = enderecoMAC.Replace(":", "");
                return enderecoMAC;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static void Ativacao(string situacao)
        {
            if (situacao == "true")
            {
                try
                {
                    string autocad = @"HKEY_CURRENT_USER\SOFTWARE\Autodesk\AutoCAD\";
                    string autocadCurver = (string)Registry.GetValue(autocad, "CurVer", "");
                    autocad += autocadCurver + @"\";
                    string autocadLang = (string)Registry.GetValue(autocad, "CurVer", "");
                    autocad += autocadLang + @"\Applications";

                    Registry.SetValue(autocad, "1", "1");
                }
                catch (System.Exception)
                {
                }
            }

            if (situacao == "false")
            {
                try
                {
                    string autocad = @"HKEY_CURRENT_USER\SOFTWARE\Autodesk\AutoCAD\";
                    string autocadCurver = (string)Registry.GetValue(autocad, "CurVer", "");
                    autocad += autocadCurver + @"\";
                    string autocadLang = (string)Registry.GetValue(autocad, "CurVer", "");
                    autocad += autocadLang + @"\Applications";

                    Registry.SetValue(autocad, "1", "0");
                }
                catch (System.Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
