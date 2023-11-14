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
            var currentTempResidents = dbContext.TempResident.ToList();

            if (!string.IsNullOrWhiteSpace(tb_LastName.Text))
            {
                currentTempResidents = currentTempResidents.Where(t => t.LastName.ToLower().Contains(tb_LastName.Text.ToLower())).ToList();
            }
            if (!string.IsNullOrWhiteSpace(tb_FirstName.Text))
            {
                currentTempResidents = currentTempResidents.Where(t => t.FirstName.ToLower().Contains(tb_FirstName.Text.ToLower())).ToList();
            }
            if (!string.IsNullOrWhiteSpace(tb_Patronymic.Text))
            {
                currentTempResidents = currentTempResidents.Where(t => t.Patronymic.ToLower().Contains(tb_Patronymic.Text.ToLower())).ToList();
            }
            
            dg_TempResidents.ItemsSource = currentTempResidents.ToList();
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

        private void bt_AddNewTempResident_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}