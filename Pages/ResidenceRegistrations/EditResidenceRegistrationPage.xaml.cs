using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Реестр_маневренного_фонда.Pages.ResidenceRegistrations
{
    /// <summary>
    /// Логика взаимодействия для EditResidenceRegistrationPage.xaml
    /// </summary>
    public partial class EditResidenceRegistrationPage : Page
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();
        
        public EditResidenceRegistrationPage(ResidenceRegistration currentResidenceRegistration)
        {
            InitializeComponent();

            cmb_HousingFund.ItemsSource = dbContext.HousingFund.ToList();
            cmb_TempResident.ItemsSource = dbContext.TempResident.ToList();
            cmb_Agreement.ItemsSource = dbContext.Agreement.ToList();

            cmb_HousingFund.SelectedItem = currentResidenceRegistration.HousingFund;
            cmb_TempResident.SelectedItem = currentResidenceRegistration.TempResident;
            dp_DateStartResidence.SelectedDate = currentResidenceRegistration.DateStartResidence;
            dp_DateEndResidence.SelectedDate = currentResidenceRegistration.DateEndResidence;
            cmb_Agreement.SelectedItem = currentResidenceRegistration.Agreement;
        }
    }
}
