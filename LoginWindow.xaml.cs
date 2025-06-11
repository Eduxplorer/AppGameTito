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

            string senha = txtSenha.Password.Trim().ToLower();



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
