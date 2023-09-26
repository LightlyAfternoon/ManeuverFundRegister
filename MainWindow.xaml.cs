using Microsoft.EntityFrameworkCore;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Реестр_маневренного_фонда.Pages;

namespace Реестр_маневренного_фонда
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void bt_AgreementsViewPage_Click(object sender, RoutedEventArgs e)
        {
            tb.Text = ApplicationContext.GetContext().TempResident.First(t => t.IdTempResident == 1).FullName;
            fr_Frame.Navigate(new AgreementsViewPage());
        }
    }
}
