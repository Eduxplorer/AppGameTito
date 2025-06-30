using AppGameTito.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace AppGameTito.Services
{
    public class UsuarioService
    {
        private string _connectionString;

        public UsuarioService()
        {
            // Lê a connection string do App.config
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public Usuario GetUsuario(string usuarioOuEmail)
        {
            Usuario usuario = null;
            string query = "SELECT * FROM tb_Usuario WHERE nickName = @nickName OR email = @email";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nickName", usuarioOuEmail);
                    command.Parameters.AddWithValue("@email", usuarioOuEmail);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            IdUsuario = (int)reader["idUsuario"],
                            IdAcl = (int)reader["idAcl"],
                            IdStatus = (int)reader["idStatus"],
                            NickName = reader["nickName"].ToString(),
                            Email = reader["email"].ToString(),
                            Senha = reader["senha"].ToString()
                        };
                    }
                }
            }
            return usuario;
        }

        public bool CriarUsuario(Usuario novoUsuario)
        {
            // Lógica para verificar se usuário/email já existe...

            // Simplificando a criação da senha 
            string senhaHash = BCrypt.Net.BCrypt.HashPassword(novoUsuario.Senha);

            string insertQuery = @"INSERT INTO tb_Usuario (nickName, email, senha, idStatus, idAcl, dataCriacao, dataAlteracao)
                                   VALUES (@nickName, @email, @senha, @idStatus, @idAcl, @dataCriacao, @dataAlteracao)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmdInsert = new SqlCommand(insertQuery, connection))
                {
                    cmdInsert.Parameters.AddWithValue("@nickName", novoUsuario.NickName);
                    cmdInsert.Parameters.AddWithValue("@email", novoUsuario.Email);
                    cmdInsert.Parameters.AddWithValue("@senha", senhaHash); // Salva o hash gerado pelo BCrypt
                    cmdInsert.Parameters.AddWithValue("@idStatus", 2); // Ativo
                    cmdInsert.Parameters.AddWithValue("@idAcl", 2); // Comum
                    cmdInsert.Parameters.AddWithValue("@dataCriacao", DateTime.Now);
                    cmdInsert.Parameters.AddWithValue("@dataAlteracao", DateTime.Now);

                    int rowsAffected = cmdInsert.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Adicionar aqui os outros métodos (Update, Delete, etc.)
    }
}