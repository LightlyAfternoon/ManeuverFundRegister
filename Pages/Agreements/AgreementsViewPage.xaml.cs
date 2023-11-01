using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Реестр_маневренного_фонда.Pages.Agreements;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages
{
    /// <summary>
    /// Логика взаимодействия для AgreementsViewPage.xaml
    /// </summary>
    public partial class AgreementsViewPage : Page
    {
        ApplicationContext dbContext;
        Agreement currentAgreement;

        public AgreementsViewPage()
        {
            InitializeComponent();

            try
            {
                dbContext = ApplicationContext.GetContext();

                dbContext.HousingFund.Load();
                dg_Agreements.ItemsSource = dbContext.Agreement.ToList();

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
            var currentAgreements = dbContext.Agreement.ToList();
            
            if (cmb_FullNameTR.SelectedItem != null)
            {
                currentAgreements = currentAgreements.Where(a => a.TempResidentId == (cmb_FullNameTR.SelectedItem as TempResident).IdTempResident);
            }
            if (cmb_HousingFund.SelectedItem != null)
            {
                currentAgreements = currentAgreements.Where(a => a.HousingFundId == cmb_HousingFund.SelectedItem as HousingFund).IdHousingFund;
            }
            if (dp_DateConclusionAgreement.SelectedDate != null)
            {
                currentAgreements = currentAgreements.Where(a => a.DateConclusionAgreement == dp_DateConclusionAgreement.SelectedDate);
            }
            if (dp_DateEndAgreement.SelectedDate != null)
            {
                currentAgreements = currentAgreements.Where(a => a.DateEndAgreement == dp_DateEndAgreement.SelectedDate);
            }
            if (dp_DateTerminationAgreement.SelectedDate != null)
            {
                currentAgreements = currentAgreements.Where(a => a.DateTerminationAgreement == dp_DateTerminationAgreement.SelectedDate);
            }

            dg_Agreements.ItemsSource = currentAgreements.ToList();
        }

        private void bt_EditAgreement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentAgreement = (sender as Button).DataContext as Agreement;

                NavigationService.Navigate(new EditAgreementPage(currentAgreement));
            }
            catch {}
        }

        private void bt_DeleteAgreement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AgreementManager am = new AgreementManager();
                currentAgreement = (sender as Button).DataContext as Agreement;
                am.RemoveAgreement(currentAgreement);
            }
            catch { }
        }

        private void bt_showOrHideFilterGrid_Click(object sender, RoutedEventArgs e)
        {
            if (gr_FilterGrid.Visibility == Visibility.Collapsed)
            {
                gr_FilterGrid.Visibility = Visibility.Visible;
            }
            else
            {
                gr_FilterGrid.Visibility = Visibility.Collapsed;
            }
        }
    }
}
