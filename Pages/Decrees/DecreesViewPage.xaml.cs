using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Реестр_маневренного_фонда.Pages.Decrees;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages
{
    /// <summary>
    /// Логика взаимодействия для DecreesViewPage.xaml
    /// </summary>
    public partial class DecreesViewPage : Page
    {
        ApplicationContext dbContext;
        Decree currentDecree;
        
        public DecreesViewPage()
        {
            InitializeComponent();

            try
            {
                dbContext = ApplicationContext.GetContext();

                dbContext.HousingFund.Load();
                dg_Decrees.ItemsSource = dbContext.Decree.ToList();

                cmb_HousingFund.ItemsSource = dbContext.HousingFund.ToList();
            }
            catch 
            {
                MessageBox.Show("Не получилось подключится к базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Filtering()
        {
            var currentDecrees = dbContext.Decree.ToList();
            
            if (dp_DateConclusionAgreement.SelectedDate != null)
            {
                currentDecrees = currentDecrees.Where(d => d.DateDecree == dp_DateDecree.SelectedDate);
            }
            if (cmb_HousingFund.SelectedItem != null)
            {
                currentDecrees = currentDecrees.Where(d => d.HousingFundId == (cmb_HousingFund.SelectedItem as HousingFund).IdHousingFund);
            }
            if (chb_Inclusion.IsChecked == true)
            {
                currentDecrees = currentDecrees.Where(d => d.Status == true);
            }
            if (chb_Exclusion.IsChecked == true)
            {
                currentDecrees = currentDecrees.Where(d => d.Status == false);
            }

            dg_Decrees.ItemsSource = currentDecrees.ToList();
        }

        private void bt_EditDecree_Click(object sender, RoutedEventArgs e)
        {
            Decree currentDecree = (sender as Button).DataContext as Decree;
            NavigationService.Navigate(new EditDecreePage(currentDecree));
        }

        private void bt_DeleteDecree_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentDecree = (sender as Button).DataContext as Decree;
                DecreeManager dm = new DecreeManager();
                dm.RemoveDecree(currentDecree);
            }
            catch { }
        }
    }
}
