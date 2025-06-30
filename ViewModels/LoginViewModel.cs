// Pastas: ViewModels/LoginViewModel.cs

using AppGameTito.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls; // Importe este namespace para usar o PasswordBox

namespace AppGameTito.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        // Propriedade para o campo de texto de usuário/email.
        // O binding no XAML vai se conectar aqui.
        [ObservableProperty]
        private string _usuarioOuEmail;

        // Não precisamos de uma propriedade para a senha aqui,
        // pois vamos pegá-la diretamente do controle.

        private UsuarioService _usuarioService;

        public LoginViewModel()
        {
            _usuarioService = new UsuarioService();
        }

        // Este é o comando que o botão "Entrar" vai chamar.
        // Ele está preparado para receber um parâmetro (o PasswordBox).
        [RelayCommand]
        private void Login(object parameter)
        {
            string senha = "";

            // 1. Verificamos se o parâmetro recebido é um PasswordBox
            if (parameter is PasswordBox passwordBox)
            {
                // 2. Se for, pegamos a senha de dentro dele
                senha = passwordBox.Password;
            }

            // 3. O resto da sua lógica de validação
            if (string.IsNullOrWhiteSpace(UsuarioOuEmail) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Preencha todos os campos!", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var usuario = _usuarioService.GetUsuario(UsuarioOuEmail);

            if (usuario == null)
            {
                MessageBox.Show("Usuário ou senha inválidos.", "Erro de Login", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool isSenhaValida = BCrypt.Net.BCrypt.Verify(senha, usuario.Senha);

            if (isSenhaValida)
            {
                MessageBox.Show("Login realizado com sucesso!", "Sucesso");
                // Futuramente, aqui chamaremos o serviço de navegação para abrir a MainWindow
            }
            else
            {
                MessageBox.Show("Usuário ou senha inválidos.", "Erro de Login", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}