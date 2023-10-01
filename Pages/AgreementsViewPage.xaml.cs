using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows.Controls;

namespace Реестр_маневренного_фонда.Pages
{
    /// <summary>
    /// Логика взаимодействия для AgreementsViewPage.xaml
    /// </summary>
    public partial class AgreementsViewPage : Page
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();

        public AgreementsViewPage()
        {
            InitializeComponent();

            dbContext.HousingFund.Load();
            dg_Agreements.ItemsSource = dbContext.Agreement.ToList();
        }
    }
}
