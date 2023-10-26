using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages.Decrees
{
    /// <summary>
    /// Логика взаимодействия для AddNewDecreePage.xaml
    /// </summary>
    public partial class AddNewDecreePage : Page
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();

        public AddNewDecreePage()
        {
            InitializeComponent();

            cmb_HousingFund.ItemsSource = dbContext.HousingFund.ToList();
        }

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            DecreeManager dm = new DecreeManager();

            bool status;
            if (rb_Inclusion.IsChecked == true)
            {
                status = true;
            }
            else
            {
                status = false;
            }

            dm.AddDecree(tb_Number.Text, dp_DateConclusion.SelectedDate, cmb_HousingFund.SelectedItem as HousingFund, status); ;
        }
    }
}