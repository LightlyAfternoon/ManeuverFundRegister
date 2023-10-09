using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows.Controls;

namespace Реестр_маневренного_фонда.Pages
{
    /// <summary>
    /// Логика взаимодействия для LocalitiesViewPage.xaml
    /// </summary>
    public partial class LocalitiesViewPage : Page
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();
        public LocalitiesViewPage()
        {
            InitializeComponent();

            dg_Localities.ItemsSource = dbContext.Locality.ToList();
        }
    }
}
