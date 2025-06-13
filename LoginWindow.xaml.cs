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

            if (usuarios.Count > 0)
            {
                MessageBox.Show($"Nickname: {usuarios[0].nickName} \nemail: {usuarios[0].email} \nsenha: {usuarios[0].senha}", "usuario");
            }
            else
            {
                MessageBox.Show("Nickname nenhum usuario encontrado", "usuario");
            }


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
