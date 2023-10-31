using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Реестр_маневренного_фонда.database.tables_classes;
using Реестр_маневренного_фонда.Pages.Localities;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages
{
    /// <summary>
    /// Логика взаимодействия для LocalitiesViewPage.xaml
    /// </summary>
    public partial class LocalitiesViewPage : Page
    {
        ApplicationContext dbContext;
        Locality currentLocality;
        
        public LocalitiesViewPage()
        {
            InitializeComponent();

            try
            {
                dbContext = ApplicationContext.GetContext();
                
                dg_Localities.ItemsSource = dbContext.Locality.ToList();
            }
            catch
            {
                MessageBox.Show("Не получилось подключится к базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Filtering()
        {
            
        }

        private void bt_EditLocality_Click(object sender, RoutedEventArgs e)
        {
            currentLocality = (sender as Button).DataContext as Locality;
            NavigationService.Navigate(new EditLocalityPage(currentLocality));
        }

        private void bt_DeleteLocality_Click(object sender, RoutedEventArgs e)
        {
            LocalityManager lm = new LocalityManager();
            currentLocality = (sender as Button).DataContext as Locality;
            lm.RemoveLocality(currentLocality);
        }
    }
}
