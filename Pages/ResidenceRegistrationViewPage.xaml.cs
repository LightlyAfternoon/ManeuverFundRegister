using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows.Controls;

namespace Реестр_маневренного_фонда.Pages
{
    /// <summary>
    /// Логика взаимодействия для ResidenceRegistrationViewPage.xaml
    /// </summary>
    public partial class ResidenceRegistrationViewPage : Page
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();

        public ResidenceRegistrationViewPage()
        {
            InitializeComponent();

            dbContext.HousingFund.Load();
            dbContext.TempResident.Load();
            dg_ResidenceRegistration.ItemsSource = dbContext.ResidenceRegistration.ToList();
        }
    }
}
