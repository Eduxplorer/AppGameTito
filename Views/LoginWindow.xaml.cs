using AppGameTito.ViewModels;
using System.Windows;

// Garanta que o namespace da LoginWindow também foi atualizado
namespace AppGameTito.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }

        // --- MÉTODO ADICIONADO ABAIXO ---
        /// <summary>
        /// Este método é chamado quando o usuário clica no hyperlink "Esqueci a Senha".
        /// </summary>
        private void aEsqueciSenha(object sender, RoutedEventArgs e)
        {
            // Cria uma instância da janela de recuperação de senha
            var recuperarView = new RecuperarWindow();

            // Exibe a nova janela
            recuperarView.Show();

            // Fecha a janela de login atual
            this.Close();
        }

        /// <summary>
        /// Este método é chamado quando o usuário clica no hyperlink "Cadastre-se".
        /// </summary>
        private void aCadastreSe(object sender, RoutedEventArgs e)
        {
            // Cria uma instância da janela de cadastro
            var cadastroView = new CadastroWindow();

            // Exibe a nova janela
            cadastroView.Show();

            // Fecha a janela de login atual
            this.Close();
        }
    }
}