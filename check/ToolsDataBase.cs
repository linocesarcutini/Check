using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace check
{
    public class ToolsDataBase
    {
        static string _conexaoMySQL = "server=seltteactive.mysql.dbaas.com.br;database=seltteactive;" +
                                      "uid=seltteactive;password=SeLtTe13579246";

        static MySqlConnection con = null;

        public static User SelectUsuarioByName(string nome)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_conexaoMySQL))
                {
                    User usuario = new User();

                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM `User` WHERE `Usuario` LIKE " + "'" + nome + "'", conn))
                    {
                        conn.Open();
                        using (MySqlDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                usuario.UserName = dr["Usuario"].ToString();
                                usuario.Name = dr["Nome"].ToString();
                                //usuario.SerialAutocad = dr["Serial_Autocad"].ToString();
                                //usuario.IPAddress = dr["IP"].ToString();
                                usuario.Date = (DateTime)dr["Data"];
                                usuario.Ativacao = dr["Ativado"].ToString();
                            }
                        }
                    }

                    return usuario;
                }
            }
            catch (Exception ex)
            {

                return null;
                //throw new Exception("Erro ao buscar Usuários pelo Nome: " + ex.Message);
            }
        }

        public static List<User> SelectListUsuario()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_conexaoMySQL))
                {
                    using (MySqlCommand command = new MySqlCommand("Select * from User", conn))
                    {
                        conn.Open();
                        List<User> listaUsuario = new List<User>();
                        using (MySqlDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                User usuario = new User
                                {
                                    UserName = dr["Usuario"].ToString(),
                                    Name = dr["Nome"].ToString(),
                                    SerialAutocad = dr["Serial_Autocad"].ToString(),
                                    IPAddress = dr["IP"].ToString(),
                                    Date = (DateTime)dr["Data"]
                                };

                                listaUsuario.Add(usuario);
                            }
                        }
                        return listaUsuario;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao acessar lista de Usuários: " + ex.Message);
            }
        }

        public static void InsertUsuario(User usuario)
        {
            try
            {
                string sql = "INSERT INTO User (nome,usuario,Serial_Autocad,ip,data) VALUES (@nome,@usuario,@Serial_Autocad,@ip,@data)";
                con = new MySqlConnection(_conexaoMySQL);
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nome", usuario.Name);
                cmd.Parameters.AddWithValue("@usuario", usuario.UserName);
                cmd.Parameters.AddWithValue("@Serial_Autocad", usuario.SerialAutocad);
                cmd.Parameters.AddWithValue("@ip", usuario.IPAddress);
                cmd.Parameters.AddWithValue("@data", usuario.Date);
                cmd.Parameters.AddWithValue("@ativacao", usuario.Ativacao);
                con.Open();
                cmd.ExecuteNonQuery();
                //MessageBox.Show("Usuário adicionado com sucesso!");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao inserir novo Usuário: " + ex.Message);
            }
            finally
            {
                con.Close();
                SelectListUsuario();
            }
        }


        public static void UpdateUsuario(User usuario)
        {
            try
            {
                string sql = "UPDATE User SET nome= @nome, usuario = @usuario, Serial_Autocad = @Serial_Autocad, ip = @ip, data = @data WHERE nome LIKE @nome";
                con = new MySqlConnection(_conexaoMySQL);
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nome", usuario.Name);
                cmd.Parameters.AddWithValue("@usuario", usuario.UserName);
                cmd.Parameters.AddWithValue("@Serial_Autocad", usuario.SerialAutocad);
                cmd.Parameters.AddWithValue("@ip", usuario.IPAddress);
                cmd.Parameters.AddWithValue("@data", usuario.Date);
                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Usuário alterado com sucesso!");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar Usuário: " + ex.Message);
            }
            finally
            {
                con.Close();
                SelectListUsuario();
            }
        }

        public static void DeleteUsuario(string usuario)
        {
            MessageBox.Show("Confirma a exclusão de " + usuario + "?");
            try
            {
                string sql = "DELETE FROM User WHERE nome LIKE @nome";
                con = new MySqlConnection(_conexaoMySQL);
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nome", usuario);
                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Usuário '" + usuario + "' deletado com sucesso!");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar Usuário: " + ex.Message);
            }
            finally
            {
                con.Close();
                SelectListUsuario();
            }
        }


    }
}
