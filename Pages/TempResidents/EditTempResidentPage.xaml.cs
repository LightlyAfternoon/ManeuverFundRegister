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

namespace Реестр_маневренного_фонда.Pages.TempResidents
{
    /// <summary>
    /// Логика взаимодействия для EditTempResidentPage.xaml
    /// </summary>
    public partial class EditTempResidentPage : Page
    {
        ApplicationContext dbContext;
        TempResident tempResident;

        public EditTempResidentPage(TempResident currentTempResident)
        {
            InitializeComponent();

            tempResident = currentTempResident;

            tb_LastName.Text = tempResident.LastName;
            tb_FirstName.Text = tempResident.FirstName;
            tb_Patronymic.Text = tempResident.Patronymic;
            tb_Remark.Text = tempResident.Remark;
        }

        private void bt_Edit_Click(object sender, RoutedEventArgs e)
        {
            TempResidentManager tm = new TempResidentManager();
            tm.EditTempResident(tempResident, tb_LastName.Text, tb_FirstName.Text, tb_Patronymic.Text, tb_Remark.Text);
        }
    }
}
