using System.Diagnostics;
using AppGameTito.ViewModels;
using System.Windows;
using AppGameTito.Services;

namespace AppGameTito.Views
{
    public partial class CadastroJogoWindow : Window
    {
        public CadastroJogoWindow()
        {
            InitializeComponent();
            DataContext = new CadastroJogoViewModel();
            
            // Este evento garante que o carregamento aconteça
            // DEPOIS que a janela estiver pronta.
            this.Loaded += async (sender, e) => {
                if (DataContext is CadastroJogoViewModel viewModel)
                {
                    await viewModel.CarregarDadosAsync();
                }
            };
        }
        
        // private void TestButton_Click(object sender, RoutedEventArgs e)
        // {
        //     Debug.WriteLine("--- INICIANDO TESTE DE CARREGAMENTO ---");
        //     try
        //     {
        //
        //         var servico = new JogoService();
        //
        //
        //         var categorias = servico.CarregarCategorias();
        //
        //      
        //         Debug.WriteLine($"Número de categorias encontradas: {categorias.Count}");
        //
        //         if (categorias.Count > 0)
        //         {
        //             Debug.WriteLine($"Primeira categoria: {categorias[0].Nome}");
        //         }
        //         else
        //         {
        //             Debug.WriteLine("A coleção de categorias está vazia.");
        //         }
        //
        //         MessageBox.Show($"Teste concluído! Verifique a janela 'Output' no Rider.\nItens encontrados: {categorias.Count}", "Teste");
        //     }
        //     catch (Exception ex)
        //     {
        //         Debug.WriteLine($"Ocorreu uma exceção: {ex.Message}");
        //         MessageBox.Show($"Ocorreu um erro durante o teste: {ex.Message}", "Erro de Teste");
        //     }
        //     Debug.WriteLine("--- FIM DO TESTE ---");
        // }
    }
}