using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Реестр_маневренного_фонда.Pages
{
    /// <summary>
    /// Логика взаимодействия для DecreesViewPage.xaml
    /// </summary>
    public partial class DecreesViewPage : Page
    {
        ApplicationContext dbContext;

        public DecreesViewPage()
        {
            InitializeComponent();

            try
            {
                dbContext = ApplicationContext.GetContext();

                dbContext.HousingFund.Load();
                dg_Decrees.ItemsSource = dbContext.Decree.ToList();
            }
            catch 
            {
                MessageBox.Show("Не получилось подключится к базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
