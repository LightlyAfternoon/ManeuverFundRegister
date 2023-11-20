using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

        public AddNewDecreePage()
        {
            InitializeComponent();

            cmb_HousingFund.ItemsSource = dbContext.HousingFund.ToList();
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

            dm.AddDecree(newDecree, tb_Number.Text, dp_DateConclusion.SelectedDate, cmb_HousingFund.SelectedItem as HousingFund, status); ;
        }

        private void bt_AttachFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Word файлы(*.doc;*.docx)|*.doc;*.docx";
            if (openFileDialog.ShowDialog() == true)
            {
                newDecree.File = FileManager.attachFile(openFileDialog.FileName);

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
    }
}