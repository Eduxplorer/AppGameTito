using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Data.SqlClient;
using static AppGameTito.MainWindow;
using System.Security.Cryptography;

namespace AppGameTito
{
    /// <summary>
    /// Lógica interna para LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnEntrar(object sender, RoutedEventArgs e)
        {
            string usuarioOuEmail = txtUsuarioOuEmail.Text.Trim().ToLower();

            string senha = txtSenha.Password.Trim();

            string pPasse = "manteiguinha"; // Senha padrão para o usuário, pode ser alterada posteriormente

            // conexão com banco de dados

            string conn = "Server=localhost\\SQLEXPRESS;Database=games_tito;Trusted_Connection=True;TrustServerCertificate=True";

            string query = "SELECT * FROM tb_Usuario WHERE nickName = @nickName OR email = @nickName";

            List<Usuario> usuarios = new List<Usuario>();

            using (SqlConnection connection = new SqlConnection(conn))
            {

                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nickName", usuarioOuEmail);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read()) {
                        usuarios.Add(new Usuario
                        {
                            idUsuario = (int)reader["idUsuario"],
                            idAcl = (int)reader["idAcl"],
                            idStatus = (int)reader["idStatus"],   
                            nickName = reader["nickName"].ToString(),
                            email = reader["email"].ToString(),
                            senha = reader["senha"].ToString()
                        }
                       );
                    }
                    reader.Close();
                }
            }

            

            if (usuarios.Count <= 0)
            {
                MessageBox.Show("Usuário ou senha inválidos", "Erro de Login", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string nickNameDB = usuarios[0].nickName;
            string emailDB = usuarios[0].email;
            string senhaDB = usuarios[0].senha;

            int idUsuario = usuarios[0].idUsuario;
            int idStatus = usuarios[0].idStatus;
            int idAcl = usuarios[0].idAcl;

            string nickNameMd5 = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(nickNameDB))).Replace("-", "").ToLower();

            string emailMd5 = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(emailDB))).Replace("-", "").ToLower();

            string senhaMd5 = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(senha))).Replace("-", "").ToLower();

            string pPasseMd5 = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(pPasse))).Replace("-", "").ToLower();


            string part1 = nickNameMd5 + emailMd5;
            string part2 = senhaMd5 + pPasseMd5;

            string senhaCryp = part1 + part2;


            //senha = BCrypt.Net.BCrypt.HashPassword(senhaCrypt);


            bool vSenha = BCrypt.Net.BCrypt.Verify(senhaCryp, senhaDB);

            if(!(vSenha))
            {
                MessageBox.Show("Usuário ou senha invalida - Senha", "Erro - Senha");
                return;
            }

            MessageBox.Show("Usuário ok", "Usuário válido!");


            MessageBox.Show("Senha banco de dados: " + senhaDB + "\nSenha para comparação " + senha, "Usuário");


            //if (usuarios.Count > 0)
            //{
            //    MessageBox.Show($"Nickname: {usuarios[0].nickName} \nemail: {usuarios[0].email} \nsenha: {usuarios[0].senha}", "usuario");
            //}
            //else
            //{
            //    MessageBox.Show("Nickname nenhum usuario encontrado", "usuario");
            //}


        }

        private void aEsqueciSenha(object sender, RoutedEventArgs e)
        {
            this.Close();
            var esqueciSenha = new RecuperarWindow();
            esqueciSenha.Show();
        }

        private void aCadastreSe(object sender, RoutedEventArgs e)
        {
            this.Close();
            var cadastroWindow = new CadastroWindow();
            cadastroWindow.Show();
        }
    }
}
