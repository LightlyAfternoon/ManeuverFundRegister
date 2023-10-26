using System.Windows;
using System.Windows.Controls;
using Реестр_маневренного_фонда.database.tables_classes;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages.Localities
{
    /// <summary>
    /// Логика взаимодействия для EditLocalityPage.xaml
    /// </summary>
    public partial class EditLocalityPage : Page
    {
        Locality locality;

        public EditLocalityPage(Locality currentLocality)
        {
            InitializeComponent();

            locality = currentLocality;

            tb_NameLocality.Text = currentLocality.NameLocality;
        }

        private void bt_Edit_Click(object sender, RoutedEventArgs e)
        {
            LocalityManager lm = new LocalityManager();
            lm.EditLocality(locality, tb_NameLocality.Text);
        }
    }
}