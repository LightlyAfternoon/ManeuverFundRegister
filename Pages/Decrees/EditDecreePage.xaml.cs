using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages.Decrees
{
    /// <summary>
    /// Логика взаимодействия для EditDecreePage.xaml
    /// </summary>
    public partial class EditDecreePage : Page
    {
        Decree decree;
        ApplicationContext dbContext = ApplicationContext.GetContext();

        public EditDecreePage(Decree currentDecree)
        {
            InitializeComponent();

            decree = currentDecree;
            cmb_HousingFund.ItemsSource = dbContext.HousingFund.ToList();

            tb_Number.Text = currentDecree.NumberDecree.ToString();
            dp_DateConclusion.SelectedDate = currentDecree.DateDecree.Date;
            if (currentDecree.Status == true)
            {
                rb_Inclusion.IsChecked = true;
            }
            else 
            {
                rb_Exclusion.IsChecked = true;
            }
        }

        private void bt_Edit_Click(object sender, RoutedEventArgs e)
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

            dm.EditDecree(decree, tb_Number.Text, dp_DateConclusion.SelectedDate, cmb_HousingFund.SelectedItem as HousingFund, status);
        }

        private void bt_AttachFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Word файлы (*.doc; *.docx)|*.doc;*.docx";

            if (openFileDialog.ShowDialog() == true)
            {
                decree.File = FileManager.attachFile(openFileDialog.FileName);

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
    }
}