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
            cmb_TempReident.ItemsSource = dbContext.TempResident.ToList();
            agreement = currentAgreement;

            tb_Number.Text = agreement.NumberAgreement.ToString();
            cmb_HousingFund.SelectedItem = agreement.HousingFund;
            cmb_TempReident.SelectedItem = agreement.TempResident;
            dp_DateConclusion.SelectedDate = agreement.DateConclusionAgreement;
            dp_DateEnd.SelectedDate = agreement.DateEndAgreement;
            dp_DateTermination.SelectedDate = agreement.DateTerminationAgreement;
            tb_Remark.Text = agreement.Remark;
        }

        private void bt_Edit_Click(object sender, RoutedEventArgs e)
        {
            AgreementManager am = new AgreementManager();
            am.EditAgreement(agreement, tb_Number.Text, cmb_TempReident.SelectedItem as TempResident, cmb_HousingFund.SelectedItem as HousingFund, dp_DateConclusion.SelectedDate, dp_DateEnd.SelectedDate, dp_DateTermination.SelectedDate, tb_Remark.Text);
        }

        private void bt_AttachFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Word файлы (*.doc; *.docx)|*.doc;*.docx";

            if (openFileDialog.ShowDialog() == true)
            {
                agreement.File = FileManager.attachFile(openFileDialog.FileName);
            }

            FileInfo fileInfo = new(openFileDialog.FileName);
            tbl_AttachedFile.Text = fileInfo.Name;
        }

        private void cmb_HousingFund_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}