using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using static AppGameTito.MainWindow;

namespace AppGameTito
{
    /// <summary>
    /// Lógica interna para RecuperarWindow.xaml
    /// </summary>
    public partial class RecuperarWindow : Window
    {
        public RecuperarWindow()
        {
            InitializeComponent();
        }


        private void aVoltarLogin(object sender, RoutedEventArgs e)
        {
            //this.Close();
            //var loginWindow = new LoginWindow();
            //loginWindow.Show();
        }

        private void btnRecuperar(object sender, RoutedEventArgs e)
        {
            string nickName = txtEmailOuUsuario.Text.Trim().ToLower();

            string conn = "Server=localhost\\SQLEXPRESS;Database=games_tito;Trusted_Connection=True;TrustServerCertificate=True;";
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
                            hashs = reader["hashs"].ToString(),
                        });
                    }
                    reader.Close();
                }
            }

            if (usuarios.Count <= 0)
            {
                MessageBox.Show("Usuário inválido!", "Erro");
                return;
            }

            int idUsuario = usuarios[0].idUsuario;
            string hashsDB = usuarios[0].hashs;

            // Fecha a janela atual
            this.Close();

            // MessageBox.Show("A hash recuperada é: " + hashsDB, "Hash");

            // Abre a janela AlteraSenhaWindow passando a hash como parâmetro
            var aSenha = new AlteraSenhaWindow(hashsDB);
            aSenha.Show();

        }
    }
}
