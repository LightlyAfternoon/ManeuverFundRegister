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
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages.Decrees
{
    /// <summary>
    /// Логика взаимодействия для EditDecreePage.xaml
    /// </summary>
    public partial class EditDecreePage : Page
    {
        Decree decree;
        ApplicationContext dbContext = ApplicationContext.GetContext();

        public EditDecreePage(Decree currentDecree)
        {
            InitializeComponent();

            decree = currentDecree;
            cmb_HousingFund.ItemsSource = dbContext.HousingFund.ToList();

            tb_Number.Text = currentDecree.NumberDecree.ToString();
            dp_DateConclusion.SelectedDate = currentDecree.DateDecree.Date;
            if (currentDecree.Status == true)
            {
                rb_Inclusion.IsChecked = true;
            }
            else 
            {
                rb_Exclusion.IsChecked = true;
            }
        }

        private void bt_Edit_Click(object sender, RoutedEventArgs e)
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

            dm.EditDecree(decree, tb_Number.Text, dp_DateConclusion.SelectedDate, cmb_HousingFund.SelectedItem as HousingFund, status);
        }

        private void bt_AttachFile_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
