using System;
using Microsoft.Win32;

namespace check
{
    class Program
    {
        static void Main(string[] args)
        {
            var macAddress = Identifier("Win32_NetworkAdapterConfiguration", "MacAddress");

            var teste = ToolsDataBase.SelectUsuarioByName(GetUser());

            if (teste.Nome == null)
            {
                try
                {
                    Usuario userNaoCadastrado = new Usuario
                    {
                        Nome = "",
                        User = GetUser(),
                        MacAddress = macAddress.Replace(":", ""),
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
                    if (!(teste.Nome == GetUser() || teste.Ativacao == "True"))
                    {
                        Ativacao("false");
                    }
                    else if (teste.Nome == GetUser() || teste.Ativacao == "True")
                    {
                        Ativacao("true");
                    }
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

        static void Ativacao(string situacao)
        {
            if (situacao == "true")
            {
                try
                {
                    string autocad = @"HKEY_CURRENT_USER\SOFTWARE\Autodesk\AutoCAD\";
                    string autocadCurver = (string)Registry.GetValue(autocad, "CurVer", "");
                    autocad += autocad + @"\" + autocadCurver + @"\";
                    string autocadLang = (string)Registry.GetValue(autocad, "CurVer", "");
                    autocad += autocad + @"\" + autocadCurver + @"\" + autocadLang + @"\Applications\";

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
                    autocad += @"\" + autocadCurver + @"\";
                    string autocadLang = (string)Registry.GetValue(autocad, "CurVer", "");
                    autocad += autocadLang + @"\Applications\";

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
