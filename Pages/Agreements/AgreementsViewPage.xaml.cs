using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Реестр_маневренного_фонда.database.tables_classes;
using Реестр_маневренного_фонда.Pages.Agreements;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages
{
    /// <summary>
    /// Логика взаимодействия для AgreementsViewPage.xaml
    /// </summary>
    public partial class AgreementsViewPage : Page
    {
        ApplicationContext dbContext;
        Agreement currentAgreement;

        public AgreementsViewPage()
        {
            InitializeComponent();

            try
            {
                dbContext = ApplicationContext.GetContext();

                dbContext.HousingFund.Load();
                lb_Agreements.ItemsSource = dbContext.Agreement.ToList();

                cmb_TempResident.ItemsSource = dbContext.TempResident.ToList();
                cmb_HousingFund.ItemsSource = dbContext.HousingFund.ToList();
            }
            catch
            {
                MessageBox.Show("Не получилось подключится к базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Filtering()
        {
            var currentAgreements = dbContext.Agreement.ToList();
            
            if (cmb_TempResident.SelectedItem != null)
            {
                currentAgreements = currentAgreements.Where(a => a.TempResidentId == (cmb_TempResident.SelectedItem as TempResident).IdTempResident).ToList();
            }
            if (cmb_HousingFund.SelectedItem != null)
            {
                currentAgreements = currentAgreements.Where(a => a.HousingFundId == (cmb_HousingFund.SelectedItem as HousingFund).IdHousingFund).ToList();
            }
            if (dp_DateConclusionAgreement.SelectedDate != null)
            {
                currentAgreements = currentAgreements.Where(a => a.DateConclusionAgreement == dp_DateConclusionAgreement.SelectedDate).ToList();
            }
            if (dp_DateEndAgreement.SelectedDate != null)
            {
                currentAgreements = currentAgreements.Where(a => a.DateEndAgreement == dp_DateEndAgreement.SelectedDate).ToList();
            }
            if (dp_DateTerminationAgreement.SelectedDate != null)
            {
                currentAgreements = currentAgreements.Where(a => a.DateTerminationAgreement == dp_DateTerminationAgreement.SelectedDate).ToList();
            }

            lb_Agreements.ItemsSource = currentAgreements.ToList();
        }

        private void bt_EditAgreement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentAgreement = (sender as Button).DataContext as Agreement;

                NavigationService.Navigate(new EditAgreementPage(currentAgreement));
            }
            catch {}
        }

        private void bt_DeleteAgreement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AgreementManager am = new AgreementManager();
                currentAgreement = (sender as Button).DataContext as Agreement;
                am.RemoveAgreement(currentAgreement);
            }
            catch { }
        }

        private void bt_showOrHideFilterGrid_Click(object sender, RoutedEventArgs e)
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

        private void bt_Filter_Click(object sender, RoutedEventArgs e)
        {
            Filtering();
        }

        private void bt_DownloadFile_Click(object sender, RoutedEventArgs e)
        {
            Agreement currentAgreement = (sender as Button).DataContext as Agreement;

            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = "PDF файлы (*.pdf)|*.pdf|Документы Word (*.doc; *.docx)|*.doc;*.docx";
            saveFileDialog.FileName = $"Договор №{currentAgreement.NumberAgreement} от {currentAgreement.DateConclusionAgreement.ToString("dd.MM.yyyy")}";

            if (saveFileDialog.ShowDialog() == true)
            {
                FileManager.getAttachedFile(currentAgreement.File, saveFileDialog.FileName);
                try
                {
                    Process.Start("explorer.exe", Directory.GetParent(saveFileDialog.FileName).ToString());
                } catch { }
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

        private void cmb_TempResident_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] words;

            words = cmb_TempResident.Text.ToString().Split(' ');
            List<TempResident> findList = dbContext.TempResident.AsEnumerable().ToList();

            foreach (string word in words)
            {
                findList = findList.Where(h => h.FullName.ToLower().Contains(word.ToLower())).ToList();
                cmb_TempResident.ItemsSource = findList;
            }

            cmb_TempResident.IsDropDownOpen = true;
        }

        private void bt_AddNewAgreement_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddNewAgreementPage());
        }
    }
}