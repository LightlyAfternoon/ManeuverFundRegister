using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages.TempResidents
{
    /// <summary>
    /// Логика взаимодействия для AddNewTempResidentPage.xaml
    /// </summary>
    public partial class AddNewTempResidentPage : Page
    {
        ApplicationContext dbContext;

        public AddNewTempResidentPage()
        {
            InitializeComponent();
        }

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            TempResidentManager tm = new TempResidentManager();
            tm.AddTempResident(tb_LastName.Text, tb_FirstName.Text, tb_Patronymic.Text, tb_Remark.Text);
        }
    }
}