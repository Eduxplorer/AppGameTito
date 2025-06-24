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
    /// Lógica interna para CadastroJogoWindow.xaml
    /// </summary>
    public partial class CadastroJogoWindow : Window
    {
        // Crie uma propriedade para guardar a lista de gêneros
        public List<GeneroSelecao> ListaDeGeneros { get; set; }
        public CadastroJogoWindow()
        {
            InitializeComponent();

            // 1. Crie e popule a lista de gêneros
            ListaDeGeneros = new List<GeneroSelecao>()
        {
            new GeneroSelecao { Nome = "Ação", IsSelected = false },
            new GeneroSelecao { Nome = "Aventura", IsSelected = false },
            new GeneroSelecao { Nome = "RPG", IsSelected = false },
            new GeneroSelecao { Nome = "Estratégia", IsSelected = false },
            new GeneroSelecao { Nome = "Simulação", IsSelected = false },
            new GeneroSelecao { Nome = "Esporte", IsSelected = false },
            new GeneroSelecao { Nome = "Luta", IsSelected = false },
            new GeneroSelecao { Nome = "Puzzle", IsSelected = false }
        };

            // 2. Associe a lista ao ListBox no XAML
            GenerosListBox.ItemsSource = ListaDeGeneros;
        }

        // GeneroSelecao.cs
        public class GeneroSelecao
        {
            public string Nome { get; set; }
            public bool IsSelected { get; set; }
        }
    }
}
