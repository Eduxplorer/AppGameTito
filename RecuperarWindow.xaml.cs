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
            this.Close();
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        private void btnRecuperar(object sender, RoutedEventArgs e)
        {
            string emailOuSenha = txtEmailOuSenha.Text.Trim().ToLower();

            string conn = "Server=localhost\\SQLEXPRESS;Database=games_tito;Trusted_Connection=True;TrustServerCertificate=True";

            string query = "SELECT * FROM tb_Usuario WHERE nickName = @nickName OR email = @nickName";

        }
    }
}
