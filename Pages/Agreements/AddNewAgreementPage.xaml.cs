using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            cmb_TempReident.ItemsSource = dbContext.TempResident.ToList();
        }

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            AgreementManager am = new AgreementManager();
            am.AddAgreement(tb_Number.Text, cmb_TempReident.SelectedItem as TempResident, cmb_HousingFund.SelectedItem as HousingFund, dp_DateConclusion.SelectedDate, dp_DateEnd.SelectedDate, tb_Remark.Text);
        }
    }
}
