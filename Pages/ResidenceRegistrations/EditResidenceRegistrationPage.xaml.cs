using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages.ResidenceRegistrations
{
    /// <summary>
    /// Логика взаимодействия для EditResidenceRegistrationPage.xaml
    /// </summary>
    public partial class EditResidenceRegistrationPage : Page
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();
        ResidenceRegistration residenceRegistration;

        public EditResidenceRegistrationPage(ResidenceRegistration currentResidenceRegistration)
        {
            InitializeComponent();

            residenceRegistration = currentResidenceRegistration;

            cmb_Agreement.ItemsSource = dbContext.Agreement.ToList();
            cmb_HousingFund.ItemsSource = dbContext.HousingFund.ToList();
            cmb_TempReident.ItemsSource = dbContext.TempResident.ToList();

            cmb_Agreement.SelectedItem = currentResidenceRegistration.Agreement;
            cmb_HousingFund.SelectedItem = currentResidenceRegistration.HousingFund;
            cmb_TempReident.SelectedItem = currentResidenceRegistration.TempResident;
            dp_DateStart.SelectedDate = currentResidenceRegistration.DateStartResidence;
            dp_DateEnd.SelectedDate = currentResidenceRegistration.DateEndResidence;
        }

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            ResidenceRegistrationManager rrm = new ResidenceRegistrationManager();
            rrm.EditResidenceRegistration(residenceRegistration, dp_DateEnd.SelectedDate);
        }
    }
}