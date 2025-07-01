using AppGameTito.Models;
using AppGameTito.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace AppGameTito.ViewModels
{
    public partial class CadastroJogoViewModel : ObservableObject
    {
        // Propriedades para os campos do formulário
        [ObservableProperty] private string _nome;
        [ObservableProperty] private decimal _preco;
        [ObservableProperty] private string _descricao;
        [ObservableProperty] private DateTime? _dataLancamento = DateTime.Now;

        // Propriedades para os itens selecionados nos ComboBoxes
        [ObservableProperty] private LookupItem _selectedClassificacao;
        [ObservableProperty] private LookupItem _selectedTipo;
        [ObservableProperty] private LookupItem _selectedAssinatura;

        // Coleções para popular os ComboBoxes e CheckBoxes
        public ObservableCollection<LookupItem> Classificacoes { get; set; }
        public ObservableCollection<LookupItem> Tipos { get; set; }
        public ObservableCollection<LookupItem> Assinaturas { get; set; }
        public ObservableCollection<CategoriaSelecao> Categorias { get; set; }

        private readonly JogoService _jogoService;

        public CadastroJogoViewModel()
        {
            _jogoService = new JogoService();
            // Carrega os dados dos ComboBoxes e CheckBoxes ao iniciar a tela
            Classificacoes = _jogoService.CarregarLookup<LookupItem>("CLASSIFICACAO", "id_classificacao", "sigla");
            Tipos = _jogoService.CarregarLookup<LookupItem>("TIPO", "id_tipo", "nome");
            Assinaturas = _jogoService.CarregarLookup<LookupItem>("ASSINATURA", "id_assinatura", "status");
            Categorias = _jogoService.CarregarCategorias();
        }

        [RelayCommand]
        private void Cadastrar()
        {
            // Validações básicas
            if (string.IsNullOrWhiteSpace(Nome) || SelectedClassificacao == null || SelectedTipo == null || SelectedAssinatura == null)
            {
                MessageBox.Show("Preencha todos os campos obrigatórios.", "Erro de Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Cria o objeto Jogo com os dados da tela
            var novoJogo = new Jogo
            {
                Nome = this.Nome,
                Preco = this.Preco,
                Descricao = this.Descricao,
                DataLancamento = this.DataLancamento,
                ClassificacaoId = this.SelectedClassificacao.Id,
                TipoId = this.SelectedTipo.Id,
                AssinaturaId = this.SelectedAssinatura.Id,
                // Pega os IDs de todas as categorias marcadas com "IsSelected = true"
                CategoriasIds = Categorias.Where(c => c.IsSelected).Select(c => c.Id).ToList()
            };

            // Chama o serviço para salvar no banco
            bool sucesso = _jogoService.CadastrarJogo(novoJogo);

            if (sucesso)
            {
                MessageBox.Show("Jogo cadastrado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Ocorreu um erro ao cadastrar o jogo.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}