using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows.Controls;

namespace Реестр_маневренного_фонда.Pages
{
    /// <summary>
    /// Логика взаимодействия для DecreesViewPage.xaml
    /// </summary>
    public partial class DecreesViewPage : Page
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();

        public DecreesViewPage()
        {
            InitializeComponent();

            dbContext.HousingFund.Load();
            dg_Decrees.ItemsSource = dbContext.Decree.ToList();
        }
    }
}
