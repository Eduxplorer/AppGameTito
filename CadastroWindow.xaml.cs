using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography; 
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
using BCrypt.Net; // Certifique-se de ter instalado o pacote BCrypt.Net-Next via NuGet

namespace AppGameTito
{
    /// <summary>
    /// Lógica interna para CadastroWindow.xaml
    /// </summary>
    public partial class CadastroWindow : Window
    {
        public CadastroWindow()
        {
            InitializeComponent();
        }

        private void btnCadastrar(object sender, RoutedEventArgs e)
        {

            string nickName = txtUsuario.Text.Trim().ToLower(); // Converte o texto para minúsculas e remove espaços em branco no início e no final

            string email = txtEmail.Text.Trim().ToLower(); // Converte o texto para minúsculas e remove espaços em branco no início e no final

            string confEmail = txtConfirmarEmail.Text.Trim().ToLower(); // Converte o texto para minúsculas e remove espaços em branco no início e no final

            string senha = txtSenha.Password.Trim().ToLower();

            string confSenha = txtConfirmarSenha.Password.Trim();

            string pPasse = "manteiguinha"; // Senha padrão para o usuário, pode ser alterada posteriormente

            // Variavel para armazenar a data e hora completa

            DateTime horaC = DateTime.Now;

            // Validação de campos preenchidos

            if (
                string.IsNullOrWhiteSpace(nickName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(confEmail) ||
                string.IsNullOrWhiteSpace(senha) ||
                string.IsNullOrWhiteSpace(confSenha)
                )
            {
                MessageBox.Show("Preencha todos os campos!", "Erro!", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUsuario.Focus(); // Foca no campo de usuário
                return;
            }

            // Validação de email

            if (email != confEmail)
            {
                MessageBox.Show("Os emails não conferem!", "Erro!", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                txtEmail.SelectAll(); // Seleciona todo o texto do campo de email
                return;
            }

            // Validação de senha

            if (senha != confSenha)
            {
                MessageBox.Show("As senhas não combinam", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtSenha.Focus();
                txtSenha.SelectAll(); // Seleciona todo o texto do campo de senha
                return;
            }


         

            //string nickNameMd5 = GerarMD5(nickName);
            string nickNameMd5 = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(nickName))).Replace("-", "").ToLower();

            string emailMd5 = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(email))).Replace("-", "").ToLower();

            string senhaMd5 = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(senha))).Replace("-", "").ToLower();

            string pPasseMd5 = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(pPasse))).Replace("-", "").ToLower();


            string part1 = nickNameMd5 + emailMd5;
            string part2 = senhaMd5 + pPasseMd5;

            string senhaCrypt = part1 + part2;

            string hashsCrypt = horaC + part2 + part1;

            senha = BCrypt.Net.BCrypt.HashPassword(senhaCrypt);

            string hashs = BCrypt.Net.BCrypt.HashPassword(hashsCrypt);

           // MessageBox.Show("Senha criptografa é: " + senha, "MD5");
           // MessageBox.Show("A hashs criptografada é: " + hashs, "MD5");

         //   MessageBox.Show("pPasseMD5 em MD5: " + pPasseMd5, "MD5");
        //    MessageBox.Show("senhaCrypt: " + senhaCrypt, "MD5");
          //  MessageBox.Show("hashsCrypt: " + hashsCrypt, "MD5");


            // Comentar todo o bloco para não gravar dados em excesso

            string conn = "Server=localhost\\SQLEXPRESS;Database=games_tito;Trusted_Connection=True;TrustServerCertificate=True";
            
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    // Abre a conexão com o banco de dados
                    connection.Open();

                    // Verifica se já existe usuário 'nickName' e o email 'email' no banco de dados

                    string checkQuery = "SELECT COUNT(*) FROM tb_Usuario WHERE nickName = @nickName OR email = @email";

                    using (SqlCommand command = new SqlCommand(checkQuery, connection))
                    {
                        command.Parameters.AddWithValue("@nickName", nickName);
                        command.Parameters.AddWithValue("@email", email);

                        int count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("Usuário ou email já cadastrado!", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                            txtUsuario.Focus();
                            txtUsuario.SelectAll(); // Seleciona todo o texto do campo de usuário
                            return;
                        }



                    }


                    // Insere o novo usuário no banco de dados
                    string insertQuery = @"INSERT INTO tb_Usuario
                                           (
                                            nickName,
                                            email,
                                            senha,
                                            hashs,
                                            apiKey,
                                            idStatus,
                                            idAcl,
                                            dataCriacao,
                                            dataAlteracao
                                            )
                                            VALUES
                                            (
                                            @nickName,
                                            @email,
                                            @senha,
                                            @hashs,
                                            @apiKey,
                                            @idStatus,
                                            @idAcl,
                                            @dataCriacao,
                                            @dataAlteracao
                                            )";

                    const int idStatus = 2; // Ativo
                    const int idAcl = 2; // Usuário comum
                    string apiKey = "chaveTito";


                    using (SqlCommand cmdInsert = new SqlCommand(insertQuery, connection))
                    {
                        cmdInsert.Parameters.AddWithValue("@nickName", nickName);
                        cmdInsert.Parameters.AddWithValue("@email", email);
                        cmdInsert.Parameters.AddWithValue("@senha", senha);
                        cmdInsert.Parameters.AddWithValue("@hashs", hashs);
                        cmdInsert.Parameters.AddWithValue("apiKey", apiKey);
                        cmdInsert.Parameters.AddWithValue("@idStatus", idStatus);
                        cmdInsert.Parameters.AddWithValue("@idAcl", idAcl);
                        cmdInsert.Parameters.AddWithValue("@dataCriacao", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmdInsert.Parameters.AddWithValue("@dataAlteracao", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        cmdInsert.ExecuteNonQuery();

                    }

                }
                MessageBox.Show($"{nickName}, cadastrado com sucesso!", "Aviso");
                this.Close();
                new LoginWindow().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao cadastrar usuário: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }  

        private void aLogin(object sender, RoutedEventArgs e)
        {
            this.Close();
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}
