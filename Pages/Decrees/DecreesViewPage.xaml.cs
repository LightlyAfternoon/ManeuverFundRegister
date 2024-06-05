using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Реестр_маневренного_фонда.Database.TablesClasses;
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
                dbContext.HouseDecree.Load();

                lb_Decrees.ItemsSource = dbContext.Decree.ToList();

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
                HousingFund housingFund = (cmb_HousingFund.SelectedItem as HousingFund);
                List<HouseDecree> houseDecrees = dbContext.HouseDecree.Where(hd => hd.HousingFundId == housingFund.IdHousingFund).ToList();
                currentDecrees = currentDecrees.Where(d => houseDecrees.Where(hd => hd.DecreeId == d.IdDecree).Count() > 0).ToList();
            }
            if (chb_Inclusion.IsChecked == true && chb_Exclusion.IsChecked == false)
            {
                currentDecrees = currentDecrees.Where(d => d.Status == true).ToList();
            }
            if (chb_Inclusion.IsChecked == false && chb_Exclusion.IsChecked == true)
            {
                currentDecrees = currentDecrees.Where(d => d.Status == false).ToList();
            }

            lb_Decrees.ItemsSource = currentDecrees.ToList();
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
            saveFileDialog.Filter = "PDF файлы (*.pdf)|*.pdf|Документы Word (*.doc; *.docx)|*.doc;*.docx";
            saveFileDialog.FileName = $"Постановление №{currentDecree.NumberDecree} от {currentDecree.DateDecree.ToString("dd.MM.yyyy")}";

            if (saveFileDialog.ShowDialog() == true)
            {
                FileManager.getAttachedFile(currentDecree.File, saveFileDialog.FileName);
                try
                {
                    Process.Start("explorer.exe", Directory.GetParent(saveFileDialog.FileName).ToString());
                }
                catch { }
            }
        }

        private void cmb_HousingFund_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] words;

            words = cmb_HousingFund.Text.ToString().Split(new char[] { ' ', ',' });
            List<HousingFund> findList = dbContext.HousingFund.AsEnumerable().ToList();

            foreach (string word in words)
            {
                findList = findList.Where(h => h.FullAddress.ToLower().Contains(word.ToLower())).ToList();
                cmb_HousingFund.ItemsSource = findList;
            }

            cmb_HousingFund.IsDropDownOpen = true;
        }

        private void bt_AddNewDecree_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddNewDecreePage());
        }
    }
}