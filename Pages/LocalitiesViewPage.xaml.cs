using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Реестр_маневренного_фонда.Pages
{
    /// <summary>
    /// Логика взаимодействия для LocalitiesViewPage.xaml
    /// </summary>
    public partial class LocalitiesViewPage : Page
    {
        ApplicationContext dbContext;
        public LocalitiesViewPage()
        {
            InitializeComponent();

            try
            {
                dbContext = ApplicationContext.GetContext();
                
                dg_Localities.ItemsSource = dbContext.Locality.ToList();
            }
            catch
            {
                MessageBox.Show("Не получилось подключится к базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
