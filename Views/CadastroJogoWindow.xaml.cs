using AppGameTito.ViewModels;
using System.Windows;

namespace AppGameTito.Views
{
    public partial class CadastroJogoWindow : Window
    {
        public CadastroJogoWindow()
        {
            InitializeComponent();
            // Apenas liga a View ao ViewModel
            DataContext = new CadastroJogoViewModel();
        }
    }
}