using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Реестр_маневренного_фонда.database.tables_classes;
using Реестр_маневренного_фонда.Pages.TempResidents;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages
{
    /// <summary>
    /// Логика взаимодействия для TempResidentsViewPage.xaml
    /// </summary>
    public partial class TempResidentsViewPage : Page
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();
        TempResident currentTempResident;

        public TempResidentsViewPage()
        {
            InitializeComponent();

            try
            {
                dbContext = ApplicationContext.GetContext();
                
                dg_TempResidents.ItemsSource = dbContext.TempResident.ToList();
            }
            catch
            {
                MessageBox.Show("Не получилось подключится к базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Filtering()
        {
            
        }

        private void bt_EditTempResident_Click(object sender, RoutedEventArgs e)
        {
            currentTempResident = (sender as Button).DataContext as TempResident;
            NavigationService.Navigate(new EditTempResidentPage(currentTempResident));
        }

        private void bt_DeleteTempResident_Click(object sender, RoutedEventArgs e)
        {
            currentTempResident = (sender as Button).DataContext as TempResident;
            TempResidentManager tm = new TempResidentManager();
            tm.RemoveTempResident(currentTempResident);
        }
    }
}
