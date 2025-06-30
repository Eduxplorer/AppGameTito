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

        // Lembrar de adicionar aqui os outros métodos (Update, Delete, etc.)

        // Este método retorna uma string para sabermos o resultado da operação
        public string CriarUsuario(string nickName, string email, string senha)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // 1. VERIFICAR SE O USUÁRIO OU EMAIL JÁ EXISTE
                string checkQuery = "SELECT COUNT(*) FROM tb_Usuario WHERE nickName = @nickName OR email = @email";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection))
                {
                    checkCmd.Parameters.AddWithValue("@nickName", nickName);
                    checkCmd.Parameters.AddWithValue("@email", email);

                    int userCount = (int)checkCmd.ExecuteScalar();
                    if (userCount > 0)
                    {
                        // Retorna uma mensagem de erro específica
                        return "Usuário ou email já cadastrado!";
                    }
                }

                // 2. CRIAR O HASH DA SENHA (DA FORMA CORRETA E SEGURA)
                // Simples, direto e seguro. Adeus MD5 e concatenações!
                string senhaHash = BCrypt.Net.BCrypt.HashPassword(senha);

                // 3. INSERIR O NOVO USUÁRIO NO BANCO
                string insertQuery = @"INSERT INTO tb_Usuario (nickName, email, senha, idStatus, idAcl, dataCriacao, dataAlteracao)
                                   VALUES (@nickName, @email, @senha, @idStatus, @idAcl, @dataCriacao, @dataAlteracao)";

                using (SqlCommand insertCmd = new SqlCommand(insertQuery, connection))
                {
                    insertCmd.Parameters.AddWithValue("@nickName", nickName);
                    insertCmd.Parameters.AddWithValue("@email", email);
                    insertCmd.Parameters.AddWithValue("@senha", senhaHash); // Salva o hash seguro
                    insertCmd.Parameters.AddWithValue("@idStatus", 2);      // Ativo
                    insertCmd.Parameters.AddWithValue("@idAcl", 2);         // Usuário Comum
                    insertCmd.Parameters.AddWithValue("@dataCriacao", DateTime.Now);
                    insertCmd.Parameters.AddWithValue("@dataAlteracao", DateTime.Now);

                    int rowsAffected = insertCmd.ExecuteNonQuery();

                    return rowsAffected > 0 ? "sucesso" : "Erro ao cadastrar usuário.";
                }
            }
        }


    }
}