using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace Реестр_маневренного_фонда.Pages
{
    /// <summary>
    /// Логика взаимодействия для AgreementsViewPage.xaml
    /// </summary>
    public partial class AgreementsViewPage : Page
    {
        ApplicationContext AppContext = ApplicationContext.GetContext();

        public AgreementsViewPage()
        {
            InitializeComponent();

            AppContext.Locality.Load();
            AppContext.Street.Load();
            AppContext.HousingFund.Load();
            dg_Agreements.ItemsSource = AppContext.Agreement.ToList();
        }
    }
}
