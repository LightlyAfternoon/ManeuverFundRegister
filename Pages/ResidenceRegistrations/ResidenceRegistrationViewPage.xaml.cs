using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Реестр_маневренного_фонда.database.tables_classes;
using Реестр_маневренного_фонда.Pages.Localities;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages.ResidenceRegistrations
{
    /// <summary>
    /// Логика взаимодействия для ResidenceRegistrationViewPage.xaml
    /// </summary>
    public partial class ResidenceRegistrationViewPage : Page
    {
        ApplicationContext dbContext;
        ResidenceRegistration currentRegistration;

        public ResidenceRegistrationViewPage()
        {
            InitializeComponent();

            try
            {
                dbContext = ApplicationContext.GetContext();

                dbContext.HousingFund.Load();
                dbContext.TempResident.Load();
                lb_ResidenceRegistration.ItemsSource = dbContext.ResidenceRegistration.OrderBy(r => r.DateStartResidence).ToList();
                
                cmb_FullNameTR.ItemsSource = dbContext.TempResident.ToList();
                cmb_HousingFund.ItemsSource = dbContext.HousingFund.ToList();
            }
            catch
            {
                MessageBox.Show("Не получилось подключится к базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Filtering()
        {
            var currentResidenceRegistrations = dbContext.ResidenceRegistration.OrderBy(r => r.DateStartResidence).ToList();
            
            if (cmb_FullNameTR.SelectedItem != null)
            {
                currentResidenceRegistrations = currentResidenceRegistrations.Where(a => a.TempResidentId == (cmb_FullNameTR.SelectedItem as TempResident).IdTempResident).ToList();
            }
            if (cmb_HousingFund.SelectedItem != null)
            {
                currentResidenceRegistrations = currentResidenceRegistrations.Where(a => a.HousingFundId == (cmb_HousingFund.SelectedItem as HousingFund).IdHousingFund).ToList();
            }
            if (dp_DateStartResidence.SelectedDate != null)
            {
                currentResidenceRegistrations = currentResidenceRegistrations.Where(a => a.DateStartResidence == dp_DateStartResidence.SelectedDate).ToList();
            }
            if (dp_DateEndResidence.SelectedDate != null)
            {
                currentResidenceRegistrations = currentResidenceRegistrations.Where(a => a.DateEndResidence == dp_DateEndResidence.SelectedDate).ToList();
            }

            lb_ResidenceRegistration.ItemsSource = currentResidenceRegistrations.ToList();
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

        private void cmb_HousingFund_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] words;

            words = cmb_HousingFund.Text.ToString().Split(new char[] { ' ', ',' });

            foreach (string word in words)
            {
                cmb_HousingFund.ItemsSource = dbContext.HousingFund.AsEnumerable().Where(h => h.FullAddress.ToLower().Contains(word.ToLower())).ToList();
            }

            cmb_HousingFund.IsDropDownOpen = true;
        }

        private void cmb_FullNameTR_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] words;

            words = cmb_FullNameTR.Text.ToString().Split(' ');

            foreach (string word in words)
            {
                cmb_FullNameTR.ItemsSource = dbContext.TempResident.AsEnumerable().Where(h => h.FullName.ToLower().Contains(word.ToLower())).ToList();
            }

            cmb_FullNameTR.IsDropDownOpen = true;
        }

        private void bt_EditRegitration_Click(object sender, RoutedEventArgs e)
        {
            currentRegistration = (sender as Button).DataContext as ResidenceRegistration;
            NavigationService.Navigate(new EditResidenceRegistrationPage(currentRegistration));
        }

        private void bt_DeleteRegistration_Click(object sender, RoutedEventArgs e)
        {
            ResidenceRegistrationManager rrm = new ResidenceRegistrationManager();
            currentRegistration = (sender as Button).DataContext as ResidenceRegistration;
            rrm.RemoveResidenceRegistration(currentRegistration);
        }
    }
}
