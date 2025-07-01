using AppGameTito.Models;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;

namespace AppGameTito.Services
{
    public class JogoService
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // ---- MÉTODO CORRIGIDO ----
        // Agora ele usa os parâmetros 'nomeTabela', 'idColuna' e 'nomeColuna' para montar a query
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
                // CORREÇÃO: Nome da tabela em minúsculo
                var cmd = new SqlCommand("SELECT id_categoria, nome FROM categoria", conn);
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

        public bool CadastrarJogo(Jogo novoJogo)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // ALERTA: Verifique se os nomes 'jogo' e 'jogo_categoria' estão com o case correto para o seu banco
                        string queryJogo = @"INSERT INTO jogo (nome, preco, descricao, data_lancamento, classificacao_id, tipo_id, assinatura_id)
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

                        foreach (var categoriaId in novoJogo.CategoriasIds)
                        {
                            string queryCategoria = "INSERT INTO jogo_categoria (jogo_id, categoria_id) VALUES (@JogoId, @CategoriaId)";
                            var cmdCategoria = new SqlCommand(queryCategoria, conn, transaction);
                            cmdCategoria.Parameters.AddWithValue("@JogoId", novoJogoId);
                            cmdCategoria.Parameters.AddWithValue("@CategoriaId", categoriaId);
                            cmdCategoria.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }
    }
}