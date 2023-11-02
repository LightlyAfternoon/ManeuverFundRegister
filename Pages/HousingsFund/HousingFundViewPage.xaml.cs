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
            var currentHousesInFund = dbContext.HousingFund.ToList();
            
            if (cmb_Street.SelectedItem != null)
            {
                currentHousesInFund = currentHousesInFund.Where(h => h.StreetId == (cmb_Street.SelectedItem as Street).IdStreet);
            }
            if (!string.IsNullOrWhiteSpace(tb_HouseNumber.Text))
            {
                currentHousesInFund = currentHousesInFund.Where(h => h.HouseNumber.ToLower().Contains(tb_HouseNumber.Text.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(tb_ApartmentNumber.Text))
            {
                currentHousesInFund = currentHousesInFund.Where(h => h.ApartmentNumber.Contains(tb_ApartmentNumber.Text));
            }
            if (!string.IsNullOrWhiteSpace(tb_RoomNumber.Text))
            {
                currentHousesInFund = currentHousesInFund.Where(h => h.RoomNumber.Contains(tb_RoomNumber.Text));
            }
            if (cmb_ImprovementDegree.SelectedItem != null)
            {
                currentHousesInFund = currentHousesInFund.Where(h => h.ImprovementDegreeId == (cmb_ImprovementDegree.SelectedItem as ImprovementDegree).IdImprovementDegree);
            }
            if (!string.IsNullOrWhiteSpace(tb_DecreeArea.Text))
            {
                currentHousesInFund = currentHousesInFund.Where(h => h.DecreeArea.Contains(tb_DecreeArea.Text));
            }
            if (!string.IsNullOrWhiteSpace(tb_RegisterArea.Text))
            {
                currentHousesInFund = currentHousesInFund.Where(h => h.RegisterArea.Contains(tb_RegisterArea.Text));
            }

            dg_HousingFund.ItemsSource = currentHousesInFund.ToList();
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
