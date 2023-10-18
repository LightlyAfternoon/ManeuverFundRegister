using System.Linq;
using System.Windows;
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

            try
            {
                dbContext = ApplicationContext.GetContext();
                
                dg_TempResidents.ItemsSource = dbContext.TempResident.ToList();
            }
            catch
            {
                MessageBox.Show("Не получилось подключится к базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
