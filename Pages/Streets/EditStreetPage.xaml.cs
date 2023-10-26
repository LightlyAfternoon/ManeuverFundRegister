using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Реестр_маневренного_фонда.database.tables_classes;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages.Streets
{
    /// <summary>
    /// Логика взаимодействия для EditStreetPage.xaml
    /// </summary>
    public partial class EditStreetPage : Page
    {
        ApplicationContext dbContext;
        Street street;

        public EditStreetPage(Street currentStreet)
        {
            InitializeComponent();

            street = currentStreet;
            cmb_Locality.ItemsSource = dbContext.Locality.ToList();

            cmb_Locality.SelectedItem = currentStreet.Locality;
            tb_NameStreet.Text = currentStreet.NameStreet;
        }

        private void bt_Edit_Click(object sender, RoutedEventArgs e)
        {
            StreetManager sm = new StreetManager();
            sm.EditStreet(street, cmb_Locality.SelectedItem as Locality, tb_NameStreet.Text);
        }
    }
}
