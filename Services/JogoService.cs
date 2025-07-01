using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Configuration;
using AppGameTito.Models;



namespace AppGameTito.Services
{
    public class JogoService
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // Método para carregar as opções dos ComboBoxes e CheckBoxes
        public ObservableCollection<T> CarregarLookup<T>(string nomeTabela, string idColuna, string nomeColuna) where T : LookupItem, new()
        {
            var colecao = new ObservableCollection<T>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand($"SELECT {idColuna}, {nomeColuna} FROM {nomeTabela}", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        colecao.Add(new T { Id = reader.GetInt32(0), Nome = reader.GetString(1) });
                    }
                }
            }
            return colecao;
        }

        public ObservableCollection<CategoriaSelecao> CarregarCategorias()
        {
            var colecao = new ObservableCollection<CategoriaSelecao>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT id_categoria, nome FROM CATEGORIA", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        colecao.Add(new CategoriaSelecao { Id = reader.GetInt32(0), Nome = reader.GetString(1), IsSelected = false });
                    }
                }
            }
            return colecao;
        }

        // Método principal para salvar o jogo no banco
        public bool CadastrarJogo(Jogo novoJogo)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // Usa uma transação para garantir que ambas as tabelas (JOGO e JOGO_CATEGORIA) sejam atualizadas
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Insere na tabela JOGO e obtém o ID do novo jogo
                        string queryJogo = @"INSERT INTO JOGO (nome, preco, descricao, data_lancamento, classificacao_id, tipo_id, assinatura_id)
                                             OUTPUT INSERTED.id_jogo
                                             VALUES (@Nome, @Preco, @Descricao, @DataLancamento, @ClassificacaoId, @TipoId, @AssinaturaId)";
                        var cmdJogo = new SqlCommand(queryJogo, conn, transaction);
                        cmdJogo.Parameters.AddWithValue("@Nome", novoJogo.Nome);
                        cmdJogo.Parameters.AddWithValue("@Preco", novoJogo.Preco);
                        cmdJogo.Parameters.AddWithValue("@Descricao", novoJogo.Descricao);
                        cmdJogo.Parameters.AddWithValue("@DataLancamento", novoJogo.DataLancamento);
                        cmdJogo.Parameters.AddWithValue("@ClassificacaoId", novoJogo.ClassificacaoId);
                        cmdJogo.Parameters.AddWithValue("@TipoId", novoJogo.TipoId);
                        cmdJogo.Parameters.AddWithValue("@AssinaturaId", novoJogo.AssinaturaId);
                        
                        int novoJogoId = (int)cmdJogo.ExecuteScalar();

                        // 2. Insere as categorias na tabela JOGO_CATEGORIA
                        foreach (var categoriaId in novoJogo.CategoriasIds)
                        {
                            string queryCategoria = "INSERT INTO JOGO_CATEGORIA (jogo_id, categoria_id) VALUES (@JogoId, @CategoriaId)";
                            var cmdCategoria = new SqlCommand(queryCategoria, conn, transaction);
                            cmdCategoria.Parameters.AddWithValue("@JogoId", novoJogoId);
                            cmdCategoria.Parameters.AddWithValue("@CategoriaId", categoriaId);
                            cmdCategoria.ExecuteNonQuery();
                        }

                        transaction.Commit(); // Confirma as alterações se tudo deu certo
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback(); // Desfaz tudo se deu algum erro
                        return false;
                    }
                }
            }
        }
    }
}
