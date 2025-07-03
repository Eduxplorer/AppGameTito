namespace AppGameTito.Models
{
    public class Jogo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataLancamento { get; set; }
        public int ClassificacaoId { get; set; }
        public int TipoId { get; set; }
        public int AssinaturaId { get; set; }
        // Usaremos esta lista para enviar os IDs das categorias selecionadas
        public List<int> CategoriasIds { get; set; } = new List<int>();   
    }
}
