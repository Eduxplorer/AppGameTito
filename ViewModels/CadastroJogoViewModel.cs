using AppGameTito.Models;
using AppGameTito.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace AppGameTito.ViewModels
{
    public partial class CadastroJogoViewModel : ObservableObject
    {
        [ObservableProperty] private string _nome;
        [ObservableProperty] private decimal _preco;
        [ObservableProperty] private string _descricao;
        [ObservableProperty] private DateTime? _dataLancamento = DateTime.Now;

        [ObservableProperty] private LookupItem _selectedClassificacao;
        [ObservableProperty] private LookupItem _selectedTipo;
        [ObservableProperty] private LookupItem _selectedAssinatura;

        public ObservableCollection<LookupItem> Classificacoes { get; set; }
        public ObservableCollection<LookupItem> Tipos { get; set; }
        public ObservableCollection<LookupItem> Assinaturas { get; set; }
        public ObservableCollection<CategoriaSelecao> Categorias { get; set; }

        private readonly JogoService _jogoService;

        public CadastroJogoViewModel()
        {
            _jogoService = new JogoService();
            // O CONSTRUTOR APENAS INICIALIZA AS LISTAS VAZIAS
            Classificacoes = new ObservableCollection<LookupItem>();
            Tipos = new ObservableCollection<LookupItem>();
            Assinaturas = new ObservableCollection<LookupItem>();
            Categorias = new ObservableCollection<CategoriaSelecao>();
        }

        // MÉTODO ÚNICO E CORRIGIDO PARA CARREGAR DADOS
        public async Task CarregarDadosAsync()
        {
            try
            {
                // Limpa as listas para o caso de um recarregamento
                Classificacoes.Clear();
                Tipos.Clear();
                Assinaturas.Clear();
                Categorias.Clear();

                // Usa os nomes de tabela em minúsculo
                var classificacoes = await Task.Run(() => _jogoService.CarregarLookup<LookupItem>("classificacao", "id_classificacao", "sigla"));
                foreach(var item in classificacoes) Classificacoes.Add(item);

                var tipos = await Task.Run(() => _jogoService.CarregarLookup<LookupItem>("tipo", "id_tipo", "nome"));
                foreach(var item in tipos) Tipos.Add(item);

                var assinaturas = await Task.Run(() => _jogoService.CarregarLookup<LookupItem>("assinatura", "id_assinatura", "status"));
                foreach(var item in assinaturas) Assinaturas.Add(item);

                var categorias = await Task.Run(() => _jogoService.CarregarCategorias());
                foreach(var item in categorias) Categorias.Add(item);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Não foi possível conectar ao banco de dados: {ex.Message}", "Erro de Conexão", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void Cadastrar()
        {
            if (string.IsNullOrWhiteSpace(Nome) || SelectedClassificacao == null || SelectedTipo == null || SelectedAssinatura == null)
            {
                MessageBox.Show("Preencha todos os campos obrigatórios.", "Erro de Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var novoJogo = new Jogo
            {
                Nome = this.Nome,
                Preco = this.Preco,
                Descricao = this.Descricao,
                DataLancamento = this.DataLancamento,
                ClassificacaoId = this.SelectedClassificacao.Id,
                TipoId = this.SelectedTipo.Id,
                AssinaturaId = this.SelectedAssinatura.Id,
                CategoriasIds = Categorias.Where(c => c.IsSelected).Select(c => c.Id).ToList()
            };

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