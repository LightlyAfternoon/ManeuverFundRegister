using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Реестр_маневренного_фонда.Pages.Agreements;

namespace Реестр_маневренного_фонда.Pages
{
    /// <summary>
    /// Логика взаимодействия для AgreementsViewPage.xaml
    /// </summary>
    public partial class AgreementsViewPage : Page
    {
        ApplicationContext dbContext;

        public AgreementsViewPage()
        {
            InitializeComponent();

            try
            {
                dbContext = ApplicationContext.GetContext();

                dbContext.HousingFund.Load();
                dg_Agreements.ItemsSource = dbContext.Agreement.ToList();
            }
            catch
            {
                MessageBox.Show("Не получилось подключится к базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void bt_EditAgreement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Agreement currentAgreement = (sender as Button).DataContext as Agreement;

                NavigationService.Navigate(new EditAgreementPage(currentAgreement));
            }
            catch {}
        }

        private void bt_DeleteAgreement_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
