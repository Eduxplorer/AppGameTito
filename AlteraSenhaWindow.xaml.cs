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
    /// Lógica interna para AlteraSenhaWindow.xaml
    /// </summary>
    public partial class AlteraSenhaWindow : Window
    {
        public AlteraSenhaWindow()
        {
            InitializeComponent();
        }

        private void btnAlterarSenha(object sender, RoutedEventArgs e)
        {
            this.Close();
            new AlteraSenhaWindow().Show();

        }

        private void aVoltarLogin(object sender, RoutedEventArgs e)
        {
            this.Close();
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}
