// Pastas: ViewModels/CadastroViewModel.cs
using AppGameTito.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;

namespace AppGameTito.Views
{
    public partial class CadastroViewModel : ObservableObject
    {
        // Propriedades para os campos de texto. O Binding do XAML se conecta aqui.
        [ObservableProperty] private string _usuario;
        [ObservableProperty] private string _email;
        [ObservableProperty] private string _confirmarEmail;

        private UsuarioService _usuarioService;

        public CadastroViewModel()
        {
            _usuarioService = new UsuarioService();
        }

        // O comando que o botão "Cadastrar" vai chamar
        [RelayCommand]
        private void Cadastrar(object parameter)
        {
            // 1. Validação dos campos de texto (que estão bindados)
            if (string.IsNullOrWhiteSpace(Usuario) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(ConfirmarEmail))
            {
                MessageBox.Show("Preencha todos os campos!", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Validação dos emails
            if (Email != ConfirmarEmail)
            {
                MessageBox.Show("Os emails não conferem!", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 3. Pegar as senhas de dentro dos PasswordBoxes
            string senha = "";
            string confirmarSenha = "";

            if (parameter is Grid formGrid)
            {
                // Usamos FindName para achar os controles dentro da Grid que recebemos
                var senhaBox = formGrid.FindName("SenhaPasswordBox") as PasswordBox;
                var confirmarSenhaBox = formGrid.FindName("ConfirmarSenhaPasswordBox") as PasswordBox;

                if (senhaBox != null && confirmarSenhaBox != null)
                {
                    senha = senhaBox.Password;
                    confirmarSenha = confirmarSenhaBox.Password;
                }
            }

            // 4. Validação das senhas
            if (string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("O campo de senha não pode estar vazio.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (senha != confirmarSenha)
            {
                MessageBox.Show("As senhas não combinam!", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 5. Chamar o Serviço para criar o usuário
            string resultado = _usuarioService.CriarUsuario(Usuario.Trim(), Email.Trim(), senha);

            // 6. Mostrar o resultado para o usuário
            if (resultado == "sucesso")
            {
                MessageBox.Show($"{Usuario}, cadastrado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                // Aqui podemos navegar de volta para o login. Veremos como fazer isso de forma limpa depois.
            }
            else
            {
                // Mostra a mensagem de erro que veio do serviço (ex: "Usuário já existe")
                MessageBox.Show(resultado, "Erro de Cadastro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}