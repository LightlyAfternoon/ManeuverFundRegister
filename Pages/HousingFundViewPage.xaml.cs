using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows.Controls;

namespace Реестр_маневренного_фонда.Pages
{
    /// <summary>
    /// Логика взаимодействия для HousingFundViewPage.xaml
    /// </summary>
    public partial class HousingFundViewPage : Page
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();

        public HousingFundViewPage()
        {
            InitializeComponent();

            dbContext.ImprovementDegree.Load();
            dg_HousingFund.ItemsSource = dbContext.HousingFund.ToList();
        }
    }
}
