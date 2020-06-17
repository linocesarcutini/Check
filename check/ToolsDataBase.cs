using System;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace check
{
    public class ToolsDataBase
    {
        static string _conexaoMySQL = "server=seltteactive.mysql.dbaas.com.br;database=seltteactive;" +
                                      "uid=seltteactive;password=SeLtTe13579246";

        static MySqlConnection con = null;

        public static Usuario SelectUsuarioByUserMac(string nome, string mac)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_conexaoMySQL))
                {
                    Usuario usuario = new Usuario();

                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM `User` WHERE `Usuario` LIKE '" + nome + "'", conn))
                    {
                        conn.Open();
                        using (MySqlDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                usuario.User = dr["Usuario"].ToString();
                                usuario.MacAddress = dr["MacAddress"].ToString();
                            }
                        }
                    }

                    return usuario;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<Usuario> SelectListUsuario()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_conexaoMySQL))
                {
                    using (MySqlCommand command = new MySqlCommand("Select * from User", conn))
                    {
                        conn.Open();
                        List<Usuario> listaUsuario = new List<Usuario>();
                        using (MySqlDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Usuario usuario = new Usuario
                                {
                                    User = dr["Usuario"].ToString(),
                                    Nome = dr["Nome"].ToString(),
                                    MacAddress = dr["MacAddress"].ToString(),
                                    Ativacao = dr["Ativado"].ToString()
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

        public static void InsertUsuario(Usuario usuario)
        {
            try
            {
                string sql = "INSERT INTO User (nome,usuario,MacAddress,Ativado) VALUES (@nome,@usuario,@MacAddress,@Ativado)";
                con = new MySqlConnection(_conexaoMySQL);
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nome", usuario.Nome);
                cmd.Parameters.AddWithValue("@usuario", usuario.User);
                cmd.Parameters.AddWithValue("@MacAddress", usuario.MacAddress);
                cmd.Parameters.AddWithValue("@Ativado", usuario.Ativacao);
                con.Open();
                cmd.ExecuteNonQuery();
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

        public static void UpdateUsuario(Usuario usuario)
        {
            try
            {
                string sql = "UPDATE User SET nome= @nome, usuario = @usuario, MacAddress = @MacAddress, Ativado = @Ativado WHERE usuario = @usuario AND MacAddress = @MacAddress";
                con = new MySqlConnection(_conexaoMySQL);
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Nome", usuario.Nome);
                cmd.Parameters.AddWithValue("@Usuario", usuario.User);
                cmd.Parameters.AddWithValue("@MacAddress", usuario.MacAddress);
                if (usuario.Ativacao.ToLower() == "true")
                {
                    cmd.Parameters.AddWithValue("@Ativado", "1");
                }

                else
                {
                    cmd.Parameters.AddWithValue("@Ativado", "0");
                }

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
