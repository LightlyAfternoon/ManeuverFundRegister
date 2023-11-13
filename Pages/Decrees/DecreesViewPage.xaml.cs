using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Реестр_маневренного_фонда.Pages.Decrees;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages
{
    /// <summary>
    /// Логика взаимодействия для DecreesViewPage.xaml
    /// </summary>
    public partial class DecreesViewPage : Page
    {
        ApplicationContext dbContext;
        Decree currentDecree;
        
        public DecreesViewPage()
        {
            InitializeComponent();

            try
            {
                dbContext = ApplicationContext.GetContext();

                dbContext.HousingFund.Load();
                dg_Decrees.ItemsSource = dbContext.Decree.ToList();

                cmb_HousingFund.ItemsSource = dbContext.HousingFund.ToList();
            }
            catch 
            {
                MessageBox.Show("Не получилось подключится к базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Filtering()
        {
            var currentDecrees = dbContext.Decree.ToList();
            
            if (dp_DateDecree.SelectedDate != null)
            {
                currentDecrees = currentDecrees.Where(d => d.DateDecree == dp_DateDecree.SelectedDate).ToList();
            }
            if (cmb_HousingFund.SelectedItem != null)
            {
                currentDecrees = currentDecrees.Where(d => d.HousingFundId == (cmb_HousingFund.SelectedItem as HousingFund).IdHousingFund).ToList();
            }
            if (chb_Inclusion.IsChecked == true)
            {
                currentDecrees = currentDecrees.Where(d => d.Status == true).ToList();
            }
            if (chb_Exclusion.IsChecked == true)
            {
                currentDecrees = currentDecrees.Where(d => d.Status == false).ToList();
            }

            dg_Decrees.ItemsSource = currentDecrees.ToList();
        }

        private void bt_EditDecree_Click(object sender, RoutedEventArgs e)
        {
            Decree currentDecree = (sender as Button).DataContext as Decree;
            NavigationService.Navigate(new EditDecreePage(currentDecree));
        }

        private void bt_DeleteDecree_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentDecree = (sender as Button).DataContext as Decree;
                DecreeManager dm = new DecreeManager();
                dm.RemoveDecree(currentDecree);
            }
            catch { }
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

        private void bt_DownloadFile_Click(object sender, RoutedEventArgs e)
        {
            Decree currentDecree = (sender as Button).DataContext as Decree;

            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = "Word файл (*.docx)|*.docx";
            saveFileDialog.FileName = $"Постановление №{currentDecree.NumberDecree} от {currentDecree.DateDecree.ToString("dd.MM.yyyy")}";

            if (saveFileDialog.ShowDialog() == true)
            {
                FileManager.getAttachedFile(currentDecree.File, saveFileDialog.FileName);
            }
        }

        private void cmb_HousingFund_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] words;

            words = cmb_HousingFund.Text.ToString().Split(new char[] { ' ', ',' });

            foreach (string word in words)
            {
                cmb_HousingFund.ItemsSource = dbContext.HousingFund.AsEnumerable().Where(h => h.FullAddress.ToLower().Contains(word.ToLower())).ToList();
            }

            cmb_HousingFund.IsDropDownOpen = true;
        }
    }
}
