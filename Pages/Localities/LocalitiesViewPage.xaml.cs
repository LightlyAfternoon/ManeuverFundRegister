using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Реестр_маневренного_фонда.database.tables_classes;
using Реестр_маневренного_фонда.Pages.Decrees;
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
                
                lb_Localities.ItemsSource = dbContext.Locality.ToList();
            }
            catch
            {
                MessageBox.Show("Не получилось подключится к базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Filtering()
        {
            var currentLocalities = dbContext.Locality.ToList();
        
            if (!string.IsNullOrWhiteSpace(tb_NameLocality.Text))
            {
                currentLocalities = currentLocalities.Where(l => l.NameLocality.ToLower().Contains(tb_NameLocality.Text.ToLower())).ToList();
            }

            lb_Localities.ItemsSource = currentLocalities.ToList();
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

        private void bt_AddNewLocality_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddNewLocalityPage());
        }
    }
}