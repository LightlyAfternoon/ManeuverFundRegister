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
    /// Логика взаимодействия для AddNewAgreementPage.xaml
    /// </summary>
    public partial class AddNewAgreementPage : Page
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();

        Agreement newAgreement = new Agreement();

        List<Decree> listDecrees = new List<Decree>();
        List<HousingFund> listAvailableHousingFund = new List<HousingFund>();

        public AddNewAgreementPage()
        {
            InitializeComponent();

            listDecrees = dbContext.Decree.OrderBy(d => d.DateDecree).ToList();
            foreach (Decree decree in listDecrees)
            {
                if (decree == listDecrees.Last(d => d.HousingFundId == decree.HousingFundId) && decree.Status == true)
                {
                    listAvailableHousingFund.Add(dbContext.HousingFund.First(h => h.IdHousingFund == decree.HousingFundId));
                }
            }
            foreach (HousingFund housingFund in dbContext.HousingFund.ToList())
            {
                if (dbContext.Decree.Count(d => d.HousingFundId == housingFund.IdHousingFund) < 1)
                {
                    listAvailableHousingFund.Add(housingFund);
                }
            }

            cmb_HousingFund.ItemsSource = listAvailableHousingFund;
            cmb_TempResident.ItemsSource = dbContext.TempResident.ToList();
        }

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            AgreementManager am = new AgreementManager();
            am.AddAgreement(newAgreement,tb_Number.Text, cmb_TempResident.SelectedItem as TempResident, cmb_HousingFund.SelectedItem as HousingFund, dp_DateConclusion.SelectedDate, dp_DateEnd.SelectedDate, tb_Remark.Text);
        }

        private void bt_AttachFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Word файлы (*.doc; *.docx)|*.doc;*.docx";

            if (openFileDialog.ShowDialog() == true)
            {
                newAgreement.File = FileManager.attachFile(openFileDialog.FileName);

                FileInfo fileInfo = new(openFileDialog.FileName);
                tbl_AttachedFile.Text = fileInfo.Name;
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

        private void cmb_TempResident_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] words;

            words = cmb_TempResident.Text.ToString().Split(' ');

            foreach (string word in words)
            {
                cmb_TempResident.ItemsSource = dbContext.TempResident.AsEnumerable().Where(h => h.FullName.ToLower().Contains(word.ToLower())).ToList();
            }

            cmb_TempResident.IsDropDownOpen = true;
        }

        private void cmb_HousingFund_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tb_ImprName.Visibility = Visibility.Visible;
            HousingFund selectedHousingFund = cmb_HousingFund.SelectedItem as HousingFund;

            if (selectedHousingFund != null)
            {
                tb_ImprName.Text = selectedHousingFund.ImprovementDegree.NameImprovementDegree;
            }
        }
    }
}