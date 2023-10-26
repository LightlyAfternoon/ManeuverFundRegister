using System.Windows;
using System.Windows.Controls;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages.Localities
{
    /// <summary>
    /// Логика взаимодействия для AddNewLocalityPage.xaml
    /// </summary>
    public partial class AddNewLocalityPage : Page
    {
        ApplicationContext dbContext;

        public AddNewLocalityPage()
        {
            InitializeComponent();
        }

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            LocalityManager lm = new LocalityManager();
            lm.AddLocality(tb_NameLocality.Text);
        }
    }
}