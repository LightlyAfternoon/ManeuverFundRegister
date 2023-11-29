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
    /// Логика взаимодействия для EditHouseInFundPage.xaml
    /// </summary>
    public partial class EditHouseInFundPage : Page
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();
        HousingFund housingFund;

        public EditHouseInFundPage(HousingFund currentHousingFund)
        {
            InitializeComponent();

            housingFund = currentHousingFund;

            cmb_Locality.ItemsSource = dbContext.Locality.ToList();
            cmb_Street.ItemsSource = dbContext.Street.Where(s => s.IdStreet == currentHousingFund.StreetId).ToList();

            cmb_Locality.SelectedItem = currentHousingFund.Locality;
            cmb_Street.SelectedItem = currentHousingFund.Street;
            tb_HouseNumber.Text = currentHousingFund.HouseNumber;
            tb_EntranceNumber.Text = currentHousingFund.EntranceNumber.ToString();
            tb_FloorNumber.Text = currentHousingFund.FloorNumber.ToString();
            tb_ApartmentNumber.Text = currentHousingFund.ApartmentNumber.ToString();
            tb_RoomNumber.Text = currentHousingFund.RoomNumber.ToString();
            tb_DecreeArea.Text = currentHousingFund.DecreeArea.ToString();
            tb_RegisterArea.Text = currentHousingFund.RegisterArea.ToString();
            tb_Remark.Text = currentHousingFund.Remark;

            if (currentHousingFund.KeysAvailability == true)
            {
                chb_KeysAvalibility.IsChecked = true;
            }
            if (currentHousingFund.CWS == true )
            {
                chb_CWS.IsChecked = true;
            }
            if (currentHousingFund.HWS == true)
            {
                chb_HWS.IsChecked = true;
            }
            if (currentHousingFund.CG == true)
            {
                chb_CG.IsChecked = true;
            }
            if (currentHousingFund.BG == true)
            {
                chb_BG.IsChecked = true;
            }
            if (currentHousingFund.SH == true)
            {
                chb_SH.IsChecked = true;
            }
            if (currentHousingFund.CH == true)
            {
                chb_CH.IsChecked = true;
            }
            if (currentHousingFund.Electricity == true)
            {
                chb_Electricity.IsChecked = true;
            }
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

        private void bt_Edit_Click(object sender, RoutedEventArgs e)
        {
            HousingFundManager hm = new HousingFundManager();
            hm.EditHouseInFund(housingFund, cmb_Locality.SelectedItem as Locality, cmb_Street.SelectedItem as Street, tb_HouseNumber.Text, tb_EntranceNumber.Text, tb_FloorNumber.Text, 
                               tb_ApartmentNumber.Text, tb_RoomNumber.Text, tb_DecreeArea.Text, tb_RegisterArea.Text, tb_Remark.Text, chb_CWS.IsChecked, 
                               chb_HWS.IsChecked, chb_CG.IsChecked, chb_BG.IsChecked, chb_SH.IsChecked, chb_CH.IsChecked, chb_Electricity.IsChecked, chb_KeysAvalibility.IsChecked);
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