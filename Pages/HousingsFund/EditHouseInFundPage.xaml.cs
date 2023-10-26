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
    }
}