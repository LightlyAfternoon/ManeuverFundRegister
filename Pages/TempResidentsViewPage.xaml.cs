using System.Linq;
using System.Windows.Controls;

namespace Реестр_маневренного_фонда.Pages
{
    /// <summary>
    /// Логика взаимодействия для TempResidentsViewPage.xaml
    /// </summary>
    public partial class TempResidentsViewPage : Page
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();

        public TempResidentsViewPage()
        {
            InitializeComponent();

            dg_TempResidents.ItemsSource = dbContext.TempResident.ToList();
        }
    }
}
