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
            MessageBox.Show("A hash passada que chegou aqui foi: " + hashs, "Hash");
            string nickName = txtEmailOuUsuario.Text.Trim().ToLower();


        }

        private void aVoltarLogin(object sender, RoutedEventArgs e)
        {
            this.Close();
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}
