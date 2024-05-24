using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Win32;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages.Decrees
{
    /// <summary>
    /// Логика взаимодействия для AddNewDecreePage.xaml
    /// </summary>
    public partial class AddNewDecreePage : Page
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();

        Decree newDecree = new Decree();
        List<HousingFund> housingFund = new List<HousingFund>();

        public AddNewDecreePage()
        {
            InitializeComponent();

            cmb_HousingFund.ItemsSource = dbContext.HousingFund.ToList();

            dp_DateConclusion.SelectedDate = DateTime.Now.Date;
        }

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            DecreeManager dm = new DecreeManager();

            bool status;
            if (rb_Inclusion.IsChecked == true)
            {
                status = true;
            }
            else
            {
                status = false;
            }

            foreach (ComboBox comboBox in sp_HousesFund.Children)
            {
                if (comboBox.SelectedItem != null)
                {
                    housingFund.Add(comboBox.SelectedItem as HousingFund);
                }
            }

            dm.AddDecree(newDecree, tb_Number.Text, dp_DateConclusion.SelectedDate, housingFund, status);
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

                    newDecree.File = FileManager.attachFile(openFileDialog.FileName);

                    var doc = PdfiumViewer.PdfDocument.Load(openFileDialog.FileName);
                    pdfViewer.Document = doc;
                }
                catch { }
            }
        }

        private void cmb_HousingFund_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] words;
            ComboBox thisComboBox = ((FrameworkElement)sender) as ComboBox;

            words = thisComboBox.Text.ToString().Split(new char[] { ' ', ',' });
            List<HousingFund> findList = dbContext.HousingFund.AsEnumerable().ToList();

            foreach (string word in words)
            {
                findList = findList.Where(h => h.FullAddress.ToLower().Contains(word.ToLower())).ToList();
                thisComboBox.ItemsSource = findList;
            }

            thisComboBox.IsDropDownOpen = true;
        }

        private void bt_AddAnotherOneHousingFund_Click(object sender, RoutedEventArgs e)
        {
            if (sp_HousesFund.Children.Count < 20)
            {
                ComboBox? comboBox = new ComboBox()
                {
                    Height = 25,
                    ItemsSource = dbContext.HousingFund.ToList(),
                    DisplayMemberPath = "FullAddress",
                    IsEditable = true,
                };
                comboBox.AddHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(cmb_HousingFund_TextChanged));
                sp_HousesFund.Children.Add(comboBox);
            }
            else
            {
                bt_AddAnotherOneHousingFund.Visibility = Visibility.Collapsed;
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