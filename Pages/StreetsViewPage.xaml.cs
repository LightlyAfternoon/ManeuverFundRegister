using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows.Controls;

namespace Реестр_маневренного_фонда.Pages
{
    /// <summary>
    /// Логика взаимодействия для StreetsViewPage.xaml
    /// </summary>
    public partial class StreetsViewPage : Page
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();

        public StreetsViewPage()
        {
            InitializeComponent();

            dbContext.Locality.Load();
            dg_Streets.ItemsSource = dbContext.Street.ToList();
        }
    }
}
