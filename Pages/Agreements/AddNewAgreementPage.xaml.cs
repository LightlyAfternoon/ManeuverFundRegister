using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
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
            openFileDialog.Filter = "PDF файлы (*.pdf)|*.pdf";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    if (pdfViewer.Document != null)
                    {
                        pdfViewer.Document.Dispose();
                        pdfViewer.Renderer.Document.Dispose();
                    }

                    newAgreement.File = FileManager.attachFile(openFileDialog.FileName);

                    var doc = PdfiumViewer.PdfDocument.Load(openFileDialog.FileName);
                    pdfViewer.Document = doc;
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

        private void tb_Term_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Convert.ToInt16(tb_Term.Text);

                if (dp_DateConclusion.SelectedDate != null)
                {
                    dp_DateEnd.SelectedDate = dp_DateConclusion.SelectedDate.Value.AddMonths(Convert.ToInt16(tb_Term.Text));
                }
            }
            catch
            {
                tb_Term.Text = string.Empty;
            }
        }

        private void dp_DateConclusion_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tb_Term.Text) && dp_DateConclusion.SelectedDate != null)
            {
                dp_DateEnd.SelectedDate = dp_DateConclusion.SelectedDate.Value.AddMonths(Convert.ToInt16(tb_Term.Text));
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (pdfViewer.Document != null)
            {
                pdfViewer.Document.Dispose();
                pdfViewer.Renderer.Document.Dispose();
            }
        }
    }
}