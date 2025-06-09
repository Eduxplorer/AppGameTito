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

            if(email != confEmail)
            {
                MessageBox.Show("Os emails não conferem!", "Erro!", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                txtEmail.SelectAll(); // Seleciona todo o texto do campo de email
                return;
            }

            // Validação de senha

           if(senha != confSenha)
            {
                MessageBox.Show("As senhas não combinam", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtSenha.Focus();
                txtSenha.SelectAll(); // Seleciona todo o texto do campo de senha
                return;
            }
            MessageBox.Show($"{nickName}, cadastrado com sucesso!", "Aviso");
        }

        private void aLogin(object sender, RoutedEventArgs e)
        {
            this.Close();
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}
