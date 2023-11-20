using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Реестр_маневренного_фонда.database.tables_classes;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages.Agreements
{
    /// <summary>
    /// Логика взаимодействия для EditAgreementPage.xaml
    /// </summary>
    public partial class EditAgreementPage : Page
    {
        Agreement agreement;
        ApplicationContext dbContext = ApplicationContext.GetContext();

        List<Decree> listDecrees = new List<Decree>();
        List<HousingFund> listAvailableHousingFund = new List<HousingFund>();
        public EditAgreementPage(Agreement currentAgreement)
        {
            InitializeComponent();

            listDecrees = dbContext.Decree.OrderBy(d => d.DateDecree).ToList();
            listAvailableHousingFund.Add(currentAgreement.HousingFund);
            foreach (Decree decree in listDecrees)
            {
                if (decree == listDecrees.Last(d => d.HousingFundId == decree.HousingFundId) && decree.Status == true && decree.HousingFundId != currentAgreement.HousingFundId)
                {
                    listAvailableHousingFund.Add(dbContext.HousingFund.First(h => h.IdHousingFund == decree.HousingFundId));
                }
            }
            foreach (HousingFund housingFund in dbContext.HousingFund.ToList())
            {
                if (dbContext.Decree.Count(d => d.HousingFundId == housingFund.IdHousingFund) < 1 && housingFund.IdHousingFund != currentAgreement.HousingFundId)
                {
                    listAvailableHousingFund.Add(housingFund);
                }
            }

            cmb_HousingFund.ItemsSource = listAvailableHousingFund;
            cmb_TempResident.ItemsSource = dbContext.TempResident.ToList();
            agreement = currentAgreement;

            tb_Number.Text = agreement.NumberAgreement.ToString();
            cmb_HousingFund.SelectedItem = agreement.HousingFund;
            cmb_TempResident.SelectedItem = agreement.TempResident;
            dp_DateConclusion.SelectedDate = agreement.DateConclusionAgreement;
            dp_DateEnd.SelectedDate = agreement.DateEndAgreement;
            dp_DateTermination.SelectedDate = agreement.DateTerminationAgreement;
            tb_Remark.Text = agreement.Remark;
        }

        private void bt_Edit_Click(object sender, RoutedEventArgs e)
        {
            AgreementManager am = new AgreementManager();
            am.EditAgreement(agreement, tb_Number.Text, cmb_TempResident.SelectedItem as TempResident, cmb_HousingFund.SelectedItem as HousingFund, dp_DateConclusion.SelectedDate, dp_DateEnd.SelectedDate, dp_DateTermination.SelectedDate, tb_Remark.Text);
        }

        private void bt_AttachFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Word файлы (*.doc; *.docx)|*.doc;*.docx";

            if (openFileDialog.ShowDialog() == true)
            {
                agreement.File = FileManager.attachFile(openFileDialog.FileName);
                
                FileInfo fileInfo = new(openFileDialog.FileName);
                tbl_AttachedFile.Text = fileInfo.Name;
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

        private void cmb_HousingFund_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tb_ImprName.Visibility = Visibility.Visible;
            HousingFund selectedHousingFund = cmb_HousingFund.SelectedItem as HousingFund;

            if (selectedHousingFund != null)
            {
                try
                {
                    tb_ImprName.Text = selectedHousingFund.ImprovementDegree.NameImprovementDegree;
                }
                catch { }
            }
        }
    }
}