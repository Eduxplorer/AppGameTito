using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static AppGameTito.MainWindow;

namespace AppGameTito
{
    /// <summary>
    /// Lógica interna para AlteraSenhaWindow.xaml
    /// </summary>
    public partial class AlteraSenhaWindow : Window
    {
        private string hashs;
        public AlteraSenhaWindow(string hashs)
        {
            InitializeComponent();
            this.hashs = hashs; // Atribui a hash recebida ao campo de classe
            MessageBox.Show("A hash recuperada é: " + hashs, "Hash"); // Exibe a hash recebida
        }

        private void btnAlterarSenha(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("A hash passada que chegou aqui foi: " + hashes, "Hash");
            string nickName = txtEmailOuUsuario.Text.Trim().ToLower();
            string senha = txtNovaSenha.Password.Trim();
            string confSenha = txtConfirmarSenha.Password.Trim();

            // Validações de campos preenchidos
            if (string.IsNullOrWhiteSpace(nickName) ||
                string.IsNullOrWhiteSpace(senha) ||
                string.IsNullOrWhiteSpace(confSenha))
            {
                MessageBox.Show("Preencha todos os campos!", "Erro!", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmailOuUsuario.Focus(); // Aqui o foco volta para txtUsuario
                return;
            }

            // Valida as senhas
            if (senha != confSenha)
            {
                MessageBox.Show("As senhas não combinam!", "Erro!", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNovaSenha.Focus(); // Aqui o foco volta para txtSenha
                txtNovaSenha.SelectAll(); // Seleciona todo o conteúdo do txtSenha
                return;
            }

            string conn = "Server=localhost\\SQLEXPRESS;Database=games_tito;Trusted_Connection=True;TrustServerCertificate=True";
            string query = "SELECT * FROM tb_Usuario WHERE nickName = @nickName OR email = @nickName";

            List<Usuario> usuarios = new List<Usuario>();

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nickName", nickName);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        usuarios.Add(new Usuario
                        {
                            idUsuario = (int)reader["idUsuario"],
                            nickName = reader["nickName"].ToString(),
                            email = reader["email"].ToString(),
                            hashs = reader["hashs"].ToString(),
                        });
                    }
                    reader.Close();
                }
            }

            if (usuarios.Count == 0)
            {
                MessageBox.Show("Usuário inválido!", "Erro");
                return;
            }

            int idUsuarioDB = usuarios[0].idUsuario;
            string nickNameDB = usuarios[0].nickName;
            string emailDB = usuarios[0].email;
            string hashesDB = usuarios[0].hashs;

            if (hashs != hashesDB) // Assuming 'hashes' is a variable from earlier in the code, likely representing the input password hash.
            {
                MessageBox.Show("Hash inválida!", "Erro");
                return;
            }

            MessageBox.Show("A hash passada que chegou aqui foi: " + hashs + "\n Hash do banco é: " + hashesDB, "Hash");

            string pPasse = "manteiguinha";

            // Variável para armazenar a data e hora completa
            DateTime horaC = DateTime.Now;

            string nicknameMd5 = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(nickNameDB))).Replace("-", "").ToLower();
            string emailMd5 = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(emailDB))).Replace("-", "").ToLower();
            string senhaMd5 = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(senha))).Replace("-", "").ToLower();
            string pPasseMd5 = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(pPasse))).Replace("-", "").ToLower();

            string part1 = nicknameMd5 + emailMd5;

            string part2 = senhaMd5 + pPasseMd5;

            string senhaCryp = part1 + part2;
            senha = BCrypt.Net.BCrypt.HashPassword(senhaCryp);

            string hashsCryp = horaC + part2 + part1;
            hashs = BCrypt.Net.BCrypt.HashPassword(hashsCryp);

            MessageBox.Show($"Senha criptograda é: {senha} \n Hash criptografada é \n {hashs}", "MD5");

            string updateQuery = "UPDATE tb_Usuario SET " + "senha = @senha, hashs = @hashs, dataAlteracao = @dataAlteracao " + "WHERE idUsuario = @idUsuario";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@senha", senha);
                    command.Parameters.AddWithValue("@hashs", hashs);
                    command.Parameters.AddWithValue("@dataAlteracao", horaC);
                    command.Parameters.AddWithValue("@idUsuario", idUsuarioDB);
                    int rowsAffected = command.ExecuteNonQuery();

                    if(rowsAffected <= 0)
                    {
                        MessageBox.Show("Erro ao atualizar senha!", "Erro");
                        return;
                    }

                    MessageBox.Show("Senha alterada com sucesso", "Sucesso");
                }
            }
        }

        private void aVoltarLogin(object sender, RoutedEventArgs e)
        {
            this.Close();
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}
