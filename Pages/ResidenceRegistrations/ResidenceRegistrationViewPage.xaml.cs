using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Реестр_маневренного_фонда.Pages.ResidenceRegistrations
{
    /// <summary>
    /// Логика взаимодействия для ResidenceRegistrationViewPage.xaml
    /// </summary>
    public partial class ResidenceRegistrationViewPage : Page
    {
        ApplicationContext dbContext;

        public ResidenceRegistrationViewPage()
        {
            InitializeComponent();

            try
            {
                dbContext = ApplicationContext.GetContext();

                dbContext.HousingFund.Load();
                dbContext.TempResident.Load();
                dg_ResidenceRegistration.ItemsSource = dbContext.ResidenceRegistration.ToList();
                
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
            var currentResidenceRegistrations = dbContext.ResidenceRegistration.ToList();
            
            if (cmb_FullNameTR.SelectedItem != null)
            {
                currentResidenceRegistrations = currentResidenceRegistrations.Where(a => a.TempResidentId == (cmb_FullNameTR.SelectedItem as TempResident).IdTempResident);
            }
            if (cmb_HousingFund.SelectedItem != null)
            {
                currentResidenceRegistrations = currentResidenceRegistrations.Where(a => a.HousingFundId == (cmb_HousingFund.SelectedItem as HousingFund).IdHousingFund);
            }
            if (dp_DateStartResidence.SelectedDate != null)
            {
                currentResidenceRegistrations = currentResidenceRegistrations.Where(a => a.DateStartResidence == dp_DateStartResidence.SelectedDate);
            }
            if (dp_DateEndResidence.SelectedDate != null)
            {
                currentResidenceRegistrations = currentResidenceRegistrations.Where(a => a.DateEndResidence == dp_DateEndResidence.SelectedDate);
            }

            dg_ResidenceRegistration.ItemsSource = currentResidenceRegistrations.ToList();
        }
    }
}
