using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Реестр_маневренного_фонда.database.tables_classes;
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
                currentHousesInFund = currentHousesInFund.Where(h => h.StreetId == (cmb_Street.SelectedItem as Street).IdStreet).ToList();
            }
            if (!string.IsNullOrWhiteSpace(tb_HouseNumber.Text))
            {
                currentHousesInFund = currentHousesInFund.Where(h => h.HouseNumber.ToLower().Contains(tb_HouseNumber.Text.ToLower())).ToList();
            }
            if (!string.IsNullOrWhiteSpace(tb_ApartmentNumber.Text))
            {
                currentHousesInFund = currentHousesInFund.Where(h => h.ApartmentNumber.ToString() == tb_ApartmentNumber.Text).ToList();
            }
            if (!string.IsNullOrWhiteSpace(tb_RoomNumber.Text))
            {
                currentHousesInFund = currentHousesInFund.Where(h => h.RoomNumber.ToString() == tb_RoomNumber.Text).ToList();
            }
            if (cmb_ImprovementDegree.SelectedItem != null)
            {
                currentHousesInFund = currentHousesInFund.Where(h => h.ImprovementDegreeId == (cmb_ImprovementDegree.SelectedItem as ImprovementDegree).IdImprovementDegree).ToList();
            }
            if (!string.IsNullOrWhiteSpace(tb_DecreeArea.Text))
            {
                currentHousesInFund = currentHousesInFund.Where(h => h.DecreeArea.ToString() == tb_DecreeArea.Text).ToList();
            }
            if (!string.IsNullOrWhiteSpace(tb_RegisterArea.Text))
            {
                currentHousesInFund = currentHousesInFund.Where(h => h.RegisterArea.ToString() == tb_RegisterArea.Text).ToList();
            }

            dg_HousingFund.ItemsSource = currentHousesInFund.ToList();
        }

        private void bt_EditHousingFund_Click(object sender, RoutedEventArgs e)
        {
            currentHousingFund = (sender as Button).DataContext as HousingFund;
            NavigationService.Navigate(new EditHouseInFundPage(currentHousingFund));
        }

        private void bt_DeleteHousingFund_Click(object sender, RoutedEventArgs e)
        {
            currentHousingFund = (sender as Button).DataContext as HousingFund;
            HousingFundManager hm = new HousingFundManager();
            hm.RemoveHouseInFund(currentHousingFund);
        }

        private void bt_Filter_Click(object sender, RoutedEventArgs e)
        {
            Filtering();
        }

        private void bt_ShowOrHideFilterGrid_Click(object sender, RoutedEventArgs e)
        {
            if (gr_FilterGrid.Visibility == Visibility.Collapsed)
            {
                gr_FilterGrid.Visibility = Visibility.Visible;
                bt_ShowOrHideFilterGrid.Content = "^";
            }
            else
            {
                gr_FilterGrid.Visibility = Visibility.Collapsed;
                bt_ShowOrHideFilterGrid.Content = "v";
            }
        }

        private void bt_AddNewHouseInFund_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}