namespace AppGameTito.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public int IdAcl { get; set; }
        public int IdStatus { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; } // Armazenará o hash da senha
        public string Hashs { get; set; } // Você pode reavaliar a necessidade deste campo
        public string ApiKey { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}