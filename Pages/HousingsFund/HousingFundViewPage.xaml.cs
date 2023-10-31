using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages.HousingsFund
{
    /// <summary>
    /// Логика взаимодействия для HousingFundViewPage.xaml
    /// </summary>
    public partial class HousingFundViewPage : Page
    {
        ApplicationContext dbContext;
        HousingFund currentHousingFund;

        public HousingFundViewPage()
        {
            InitializeComponent();

            try
            {
                dbContext = ApplicationContext.GetContext();

                dbContext.ImprovementDegree.Load();
                dg_HousingFund.ItemsSource = dbContext.HousingFund.ToList();

                cmb_Street.ItemsSource = dbContext.Street.ToList();
                cmb_ImprovementDegree.ItemsSource = dbContext.ImprovementDegree.ToList();
            }
            catch
            {
                MessageBox.Show("Не получилось подключится к базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Filtering()
        {
            
        }

        private void bt_EditHousingFund_Click(object sender, RoutedEventArgs e)
        {
            currentHousingFund = (sender as Button).DataContext as HousingFund;
            NavigationService.Navigate(currentHousingFund);
        }

        private void bt_DeleteHousingFund_Click(object sender, RoutedEventArgs e)
        {
            currentHousingFund = (sender as Button).DataContext as HousingFund;
            HousingFundManager hm = new HousingFundManager();
            hm.RemoveHouseInFund(currentHousingFund);
        }
    }
}
