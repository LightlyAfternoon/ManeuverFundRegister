using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Реестр_маневренного_фонда.database.tables_classes;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages.Streets
{
    /// <summary>
    /// Логика взаимодействия для EditStreetPage.xaml
    /// </summary>
    public partial class EditStreetPage : Page
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();
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

        private void cmb_Locality_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] words;

            words = cmb_Locality.Text.ToString().Split(new char[] { ' ', ',' });

            foreach (string word in words)
            {
                cmb_Locality.ItemsSource = dbContext.Locality.AsEnumerable().Where(l => l.NameLocality.ToLower().Contains(word.ToLower())).ToList();
            }

            cmb_Locality.IsDropDownOpen = true;
        }
    }
}