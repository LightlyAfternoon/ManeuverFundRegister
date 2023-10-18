using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Реестр_маневренного_фонда.Pages
{
    /// <summary>
    /// Логика взаимодействия для HousingFundViewPage.xaml
    /// </summary>
    public partial class HousingFundViewPage : Page
    {
        ApplicationContext dbContext;

        public HousingFundViewPage()
        {
            InitializeComponent();

            try
            {
                dbContext = ApplicationContext.GetContext();

                dbContext.ImprovementDegree.Load();
                dg_HousingFund.ItemsSource = dbContext.HousingFund.ToList();
            }
            catch 
            {
                MessageBox.Show("Не получилось подключится к базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
