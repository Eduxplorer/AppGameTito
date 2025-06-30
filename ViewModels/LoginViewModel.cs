using AppGameTito.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace AppGameTito.ViewModels
{
    // ObservableObject já implementa INotifyPropertyChanged
    public partial class LoginViewModel : ObservableObject
    {
        // A anotação [ObservableProperty] cria a propriedade e o boilerplate para nós
        [ObservableProperty]
        private string _usuarioOuEmail;

        [ObservableProperty]
        private string _senha;

        private UsuarioService _usuarioService;

        public LoginViewModel()
        {
            _usuarioService = new UsuarioService();
        }

        // [RelayCommand] cria um ICommand para ser usado na View
        [RelayCommand]
        private void Login()
        {
            if (string.IsNullOrWhiteSpace(UsuarioOuEmail) || string.IsNullOrWhiteSpace(Senha))
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

            // BCrypt.Verify compara a senha digitada com o hash salvo no banco
            bool isSenhaValida = BCrypt.Net.BCrypt.Verify(Senha, usuario.Senha);

            if (isSenhaValida)
            {
                MessageBox.Show("Login realizado com sucesso!", "Sucesso");
                // Aqui você pode navegar para a MainWindow
                // Vamos ver como fazer isso no Passo 5
            }
            else
            {
                MessageBox.Show("Usuário ou senha inválidos.", "Erro de Login", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}