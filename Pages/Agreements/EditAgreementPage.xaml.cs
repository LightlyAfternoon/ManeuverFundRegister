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

namespace Реестр_маневренного_фонда.Pages.Agreements
{
    /// <summary>
    /// Логика взаимодействия для EditAgreementPage.xaml
    /// </summary>
    public partial class EditAgreementPage : Page
    {
        Agreement agreement;
        ApplicationContext dbContext = ApplicationContext.GetContext();
        public EditAgreementPage(Agreement currentAgreement)
        {
            InitializeComponent();

            cmb_HousingFund.ItemsSource = dbContext.HousingFund.ToList();
            cmb_TempReident.ItemsSource = dbContext.TempResident.ToList();
            agreement = currentAgreement;

            tb_Number.Text = agreement.NumberAgreement.ToString();
            cmb_HousingFund.SelectedItem = agreement.HousingFund;
            cmb_TempReident.SelectedItem = agreement.TempResident;
            dp_DateConclusion.SelectedDate = agreement.DateTerminationAgreement;
            dp_DateEnd.SelectedDate = agreement.DateEndAgreement;
            dp_DateTermination.SelectedDate = agreement.DateTerminationAgreement;
            tb_Remark.Text = agreement.Remark;
        }

        private void bt_Edit_Click(object sender, RoutedEventArgs e)
        {
            AgreementManager am = new AgreementManager();
            am.EditAgreement(agreement, tb_Number.Text, cmb_TempReident.SelectedItem as TempResident, cmb_HousingFund.SelectedItem as Реестр_маневренного_фонда.HousingFund, dp_DateConclusion.SelectedDate, dp_DateEnd.SelectedDate, dp_DateTermination.SelectedDate, tb_Remark.Text);
        }
    }
}
