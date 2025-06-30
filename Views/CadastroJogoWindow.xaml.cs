using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Data.SqlClient;

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
            GenerosItemsControl.ItemsSource = ListaDeGeneros;
        }

        // GeneroSelecao.cs
        public class GeneroSelecao
        {
            public string Nome { get; set; }
            public bool IsSelected { get; set; }
        }

        private void CadastrarButton_Click(object sender, RoutedEventArgs e)
        {
            string nomeJogo = NomeJogoTextBox.Text.Trim();
            string precoJogo = PrecoTextBox.Text.Trim();
            string assinatura = AssinaturaComboBox.Text;
            DateTime? dataLancamento = DataLancamentoPicker.SelectedDate;
            string generoSelecionado = string.Join(", ", ListaDeGeneros.Where(g => g.IsSelected).Select(g => g.Nome));
            string descricaoJogo = DescricaoTextBox.Text.Trim();
            string requisitos = RequisitosTextBox.Text.Trim();
            string classificacao = ClassificacaoIndicativaComboBox.Text;
            string tipo = TipoComboBox.Text;

            Debug.WriteLine(assinatura);

            Debug.WriteLine(generoSelecionado);
            Debug.WriteLine(generoSelecionado.GetType().FullName);


            // Validação de campos preenchidos

            if (string.IsNullOrWhiteSpace(nomeJogo) ||
                string.IsNullOrWhiteSpace(precoJogo) ||
                string.IsNullOrWhiteSpace(assinatura) ||
                !dataLancamento.HasValue ||
                string.IsNullOrWhiteSpace(generoSelecionado) ||
                string.IsNullOrWhiteSpace(descricaoJogo) ||
                string.IsNullOrWhiteSpace(requisitos) ||
                string.IsNullOrWhiteSpace(classificacao) ||
                string.IsNullOrWhiteSpace(tipo))
            {
                MessageBox.Show("Por favor, preencha todos os campos obrigatórios.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(nomeJogo.Length < 3)
            {
                MessageBox.Show("O nome do jogo deve ter pelo menos 3 caracteres.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } else if (nomeJogo.Length > 200)
            {
                MessageBox.Show("O nome do jogo deve ter no máximo 200 caracteres.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(precoJogo, out decimal preco) || preco < 0)
            {
                MessageBox.Show("Por favor, insira um preço válido.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (preco > 1000)
            {
                MessageBox.Show("O preço do jogo não pode ser maior que R$1000,00.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }


            // ---> VALIDAÇÃO DA DATA DE LANÇAMENTO

            DateTime dataMinima = new DateTime(1970, 1, 1);
            DateTime dataMaxima = DateTime.Now.AddYears(5);

            DateTime dataSelecionada = dataLancamento.Value;

            if(dataSelecionada < dataMinima || dataSelecionada > dataMaxima)
            {
                MessageBox.Show(
                        $"A data de lançamento é inválida.\nPor favor, insira uma data entre {dataMinima:dd/MM/yyyy} e {dataMaxima:dd/MM/yyyy}.",
                        "Data Inválida",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                return;
            }
            
            
            // Conexão com o banco de dados
            
            string conn = "Server=localhost\\SQLEXPRESS;Database=games_tito;Trusted_Connection=True;TrustServerCertificate=True";
            
            
            

        



        }
    }
}