using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Реестр_маневренного_фонда.database.tables_classes;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages
{
    /// <summary>
    /// Логика взаимодействия для StreetsViewPage.xaml
    /// </summary>
    public partial class StreetsViewPage : Page
    {
        ApplicationContext dbContext;
        Street currentStreet;

        public StreetsViewPage()
        {
            InitializeComponent();

            try
            {
                dbContext = ApplicationContext.GetContext();
                
                dbContext.Locality.Load();
                dg_Streets.ItemsSource = dbContext.Street.ToList();

                cmb_Locality.ItemsSource = dbContext.Locality.ToList();
            }
            catch
            {
                MessageBox.Show("Не получилось подключится к базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Filtering()
        {
            var currentStreets = dbContext.Street.ToList();

            if (cmb_Locality.SelectedItem != null)
            {
                currentStreets = currentStreets.Where(s => s.LocalityId == (cmb_Locality.SelectedItem as Locality).IdLocality).ToList();
            }
            if (!string.IsNullOrWhiteSpace(tb_NameStreet.Text))
            {
                currentStreets = currentStreets.Where(s => s.NameStreet.ToLower().Contains(tb_NameStreet.Text.ToLower())).ToList();
            }
        
            dg_Streets.ItemsSource = currentStreets.ToList();
        }

        private void bt_EditStreet_Click(object sender, RoutedEventArgs e)
        {
            currentStreet = (sender as Button).DataContext as Street;
            NavigationService.Navigate(currentStreet);
        }

        private void bt_DeleteStreet_Click(object sender, RoutedEventArgs e)
        {
            currentStreet = (sender as Button).DataContext as Street;
            StreetManager sm = new StreetManager();
            sm.RemoveStreet(currentStreet);
        }

        private void bt_Filter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bt_ShowOrHideFilterGrid_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
