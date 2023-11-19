using System;
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
            cmb_ImprovementDegree.ItemsSource = dbContext.ImprovementDegree.ToList();

            cmb_Locality.SelectedItem = currentHousingFund.Street.Locality;
            cmb_Street.SelectedItem = currentHousingFund.Street;
            tb_HouseNumber.Text = currentHousingFund.HouseNumber;
            tb_ApartmentNumber.Text = currentHousingFund.ApartmentNumber.ToString();
            tb_RoomNumber.Text = currentHousingFund.RoomNumber.ToString();
            cmb_ImprovementDegree.SelectedItem = currentHousingFund.ImprovementDegree;
            tb_DecreeArea.Text = currentHousingFund.DecreeArea.ToString();
            tb_RegisterArea.Text = currentHousingFund.RegisterArea.ToString();
            tb_Remark.Text = currentHousingFund.Remark;
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
            hm.EditHouseInFund(housingFund, cmb_Street.SelectedItem as Street, tb_HouseNumber.Text, tb_ApartmentNumber.Text, tb_RoomNumber.Text, cmb_ImprovementDegree.SelectedItem as ImprovementDegree, tb_DecreeArea.Text, tb_RegisterArea.Text, tb_Remark.Text);
        }

        private void cmb_Locality_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] words;

            words = cmb_Locality.Text.ToString().Split(new char[] { ' ', ',' });

            foreach (string word in words)
            {
                cmb_Locality.ItemsSource = dbContext.Locality.AsEnumerable().Where(l => l.NameLocality.ToLower().Contains(word.ToLower())).ToList();
            }

            cmb_Locality.IsDropDownOpen = true;
        }

        private void cmb_Street_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] words;

            words = cmb_Street.Text.ToString().Split(new char[] { ' ', ',' });

            foreach (string word in words)
            {
                cmb_Street.ItemsSource = dbContext.Street.AsEnumerable().Where(l => l.NameStreet.ToLower().Contains(word.ToLower())).ToList();
            }

            cmb_Street.IsDropDownOpen = true;
        }
    }
}