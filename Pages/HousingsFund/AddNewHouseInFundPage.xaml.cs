using System;
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
            cmb_ImprovementDegree.ItemsSource = dbContext.ImprovementDegree.ToList();
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
            hm.AddHouseInFund(cmb_Street.SelectedItem as Street, tb_HouseNumber.Text, tb_ApartmentNumber.Text, tb_RoomNumber.Text, cmb_ImprovementDegree.SelectedItem as ImprovementDegree, tb_DecreeArea.Text, tb_RegisterArea.Text, tb_Remark.Text);
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