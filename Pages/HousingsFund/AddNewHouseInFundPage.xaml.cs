using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Реестр_маневренного_фонда.database.tables_classes;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages.HousingsFund
{
    /// <summary>
    /// Логика взаимодействия для AddNewHouseInFundPage.xaml
    /// </summary>
    public partial class AddNewHouseInFundPage : Page
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();

        public AddNewHouseInFundPage()
        {
            InitializeComponent();

            cmb_Locality.ItemsSource = dbContext.Locality.ToList();
        }

        private void cmb_Locality_DropDownClosed(object sender, EventArgs e)
        {
            if (cmb_Locality.SelectedItem != null)
            {
                Locality currentLocality = cmb_Locality.SelectedItem as Locality;
                cmb_Street.IsEnabled = true;
                cmb_Street.ItemsSource = dbContext.Street.Where(s => s.LocalityId == currentLocality.IdLocality).ToList();
            }
        }

        private void cmb_Locality_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmb_Street.SelectedItem = null;
        }

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            HousingFundManager hm = new HousingFundManager();
            hm.AddHouseInFund(cmb_Locality.SelectedItem as Locality, cmb_Street.SelectedItem as Street, tb_HouseNumber.Text, tb_EntranceNumber.Text, 
                tb_FloorNumber.Text, tb_ApartmentNumber.Text, tb_RoomNumber.Text, tb_DecreeArea.Text, tb_RegisterArea.Text, tb_Remark.Text, 
                chb_CWS.IsChecked, chb_HWS.IsChecked, chb_CG.IsChecked, chb_BG.IsChecked, chb_SH.IsChecked, chb_CH.IsChecked, chb_Electricity.IsChecked, chb_KeysAvalibility.IsChecked, chb_Sewerage.IsChecked);
        }

        private void cmb_Locality_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] words;

            words = cmb_Locality.Text.ToString().Split(new char[] { ' ', ',' });
            List<Locality> findList = dbContext.Locality.ToList();

            foreach (string word in words)
            {
                findList = findList.Where(l => l.NameLocality.ToLower().Contains(word.ToLower())).ToList();
                cmb_Locality.ItemsSource = findList;
            }

            cmb_Locality.IsDropDownOpen = true;
        }

        private void cmb_Street_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] words;

            words = cmb_Street.Text.ToString().Split(new char[] { ' ', ',' });
            List<Street> findList = dbContext.Street.ToList();

            foreach (string word in words)
            {
                findList = findList.Where(s => s.NameStreet.ToLower().Contains(word.ToLower())).ToList();
                cmb_Street.ItemsSource = findList;
            }

            cmb_Street.IsDropDownOpen = true;
        }
    }
}
