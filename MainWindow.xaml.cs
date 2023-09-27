using System;
using System.IO;
using System.Linq;
using System.Windows;
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

            tb.Text = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        }

        private void bt_AgreementsViewPage_Click(object sender, RoutedEventArgs e)
        {
            tb.Text = ApplicationContext.GetContext().TempResident.First(t => t.IdTempResident == 1).FullName;
            fr_Frame.Navigate(new AgreementsViewPage());
        }

        private void bt_DecreesViewPage_Click(object sender, RoutedEventArgs e)
        {
            fr_Frame.Navigate(new DecreesViewPage());
        }
    }
}
