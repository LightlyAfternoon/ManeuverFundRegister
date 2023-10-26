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

namespace Реестр_маневренного_фонда.Pages.Agreements
{
    /// <summary>
    /// Логика взаимодействия для AddNewAgreementPage.xaml
    /// </summary>
    public partial class AddNewAgreementPage : Page
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();
        List<Decree> listDecrees = dbContext.Decree.OrderBy(d => d.DateDecree).ToList();
        List<HousingFund> listHousingFund = new List<HousingFund>();
        public AddNewAgreementPage()
        {
            InitializeComponent();

            foreach (Decree decree in listDecrees)
            {
                if (decree.HousingFundId != listDecrees.Skip(1)) // if this hfId != next hfId
                {
                    
                }
            }
            cmb_HousingFund.ItemsSource = dbContext.HousingFund.Where(h => h.).ToList();
            cmb_TempReident.ItemsSource = dbContext.TempResident.ToList();
        }
    }
}
