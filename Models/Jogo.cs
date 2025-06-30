namespace AppGameTito.Models
{
    public class Jogo
    {
        public int IdJogo { get; set; }
        public string NomeJogo { get; set; }
        public string PrecoJogo { get; set; }
        public string Assinatura { get; set; }
        public DateTime DataLancamento { get; set; }
        public string Genero { get; set; }
        public string DescricaoJogo { get; set; }
        public string Requisitos { get; set; }
    }
}
