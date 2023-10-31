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
            }
            catch
            {
                MessageBox.Show("Не получилось подключится к базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Filtering()
        {
            
        }

    }
}
