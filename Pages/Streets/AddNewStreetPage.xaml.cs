using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Реестр_маневренного_фонда.database.tables_classes;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages.Streets
{
    /// <summary>
    /// Логика взаимодействия для AddNewStreetPage.xaml
    /// </summary>
    public partial class AddNewStreetPage : Page
    {
        ApplicationContext dbContext;

        public AddNewStreetPage()
        {
            InitializeComponent();

            cmb_Locality.ItemsSource = dbContext.Locality.ToList();
        }

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            StreetManager sm = new StreetManager();
            sm.AddStreet(cmb_Locality.SelectedItem as Locality, tb_NameStreet.Text);
        }
    }
}