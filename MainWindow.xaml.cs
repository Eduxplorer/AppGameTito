 using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AppGameTito
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private void CarregarUsuarios()
        {
            // Conectando com banco de dados

            // Define a String de conexão com banco de dados SQL Server usando a autenticação do Windows
            string conn = "Server=localhost\\SQLEXPRESS;Database=games_tito;Trusted_Connection=True;TrustServerCertificate=True";
        // Define a query de uso para consulta do usuário
         string query = "SELECT * FROM tb_Usuario";

            List <Usuario> usuarios = new List <Usuario>();

        // Usa um bloco 'using' para garantir que a conexão será fechada automaticamente
        using(SqlConnection connection = new SqlConnection(conn))
        {

            // Abre a conexão com o banco de dados
            connection.Open();

            // Cria um comando  SQL associado a conexão aberta
           SqlCommand command = new SqlCommand(query, connection);

            // Executa o comando e obtem um leitor para percorrer os resultados
            SqlDataReader reader = command.ExecuteReader();


            // percorre todos os registros e obtém um leitor de resultados e imprime no console
            while (reader.Read()) 
            {
                    // exibe no console o ID e o nickName
                    //Console.WriteLine($"id:{reader["idUsuario"]}, Nick: {reader["nickName"]}");

                    usuarios.Add(new Usuario
                    {
                        idUsuario = (int)reader["idUsuario"],
                        nickName = reader["nickName"].ToString(),
                        email = reader["email"].ToString(),
                        senha = reader["senha"].ToString(),
                        hashs = reader["hashs"].ToString(),
                        apiKey = reader["apiKey"].ToString(),
                        dataCriacao = reader["dataCriacao"].ToString(),
                        dataAlteracao = reader["dataAlteracao"].ToString()
                    }
                        );
            }

            reader.Close();

            lstUsuarios.ItemsSource = usuarios;
        }
}

        public class Usuario
        {
            public int idUsuario { get; set; }
            public string nickName { get; set; }

            public string email { get; set; }

            public string senha { get; set; }

            public string hashs { get; set; }

            public string apiKey { get; set; }

            public string dataCriacao { get; set; }

            public string dataAlteracao { get; set; }



        }
        public MainWindow()
        {
            InitializeComponent();
            CarregarUsuarios();


            // Vento que é disparado quando o controle do texto 'BuscaTxt' recebe o foco (O usuário clica ou interage com o campo)
            BuscaTxt.GotFocus += (s, e) =>
            {
                // Verifica se o texto do campo ainda é o texto padrão 'Pesquisar..'
                if(BuscaTxt.Text == "Pesquisar...")
                {
                    // Limpa o texto do campo de busca quando o usuário clica ou interage co ele 
                    BuscaTxt.Text = "";
                    // Altera a cor do texto para 'preto' quando o usuário começa a digitar
                    BuscaTxt.Foreground = Brushes.Black;
                };

                // Torna 'PesquisaLbl' invisivel (Oculta o texto sugerido 'Pesquisar...')
                PesquisaLbl.Visibility = Visibility.Collapsed;

            };

            BuscaTxt.LostFocus += (s, e) => {

                // Verifica se o campo de busca está vázio ou contém apenas espaços em branco
                if(string.IsNullOrWhiteSpace(BuscaTxt.Text))
                {

                    // Se o campo estiver vázio, torna o 'PesquisaLbl' visível novamente
                    PesquisaLbl.Visibility = Visibility.Visible;
                };
            };

            // Evento disparado quando o texto no controle de busca é alterado (O usuário digita algo n campo)
            BuscaTxt.TextChanged += (s, e) =>
            {
                // Verifica se o campo de busca está vazio ou contém apenas espaços em branco
                // Se estiver vazio, o 'PesquisaLbl' é visivel, caso contrário é oculto
                PesquisaLbl.Visibility = string.IsNullOrWhiteSpace(BuscaTxt.Text) ?
                    Visibility.Visible : Visibility.Collapsed;
            };

        }

        private const double CardWidthMargin = 320; 

        private void BntProximo(object sender, RoutedEventArgs e)
        {
            double offset = carouselScroll.HorizontalOffset + CardWidthMargin;
            carouselScroll.ScrollToHorizontalOffset(offset);
        }

        private void BntAnterior(object sender, RoutedEventArgs e)
        {
            double offset = carouselScroll.HorizontalOffset - CardWidthMargin;

            if(offset < 0) offset = 0;

            carouselScroll.ScrollToHorizontalOffset(offset);
                
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Cria uma nova instância da janela de login e exibe-a como uma caixa de diálogo modal
            // Isso impede que o usuário interaja com a janela principal até que a janela de login seja fechada
            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
        }
    }
}