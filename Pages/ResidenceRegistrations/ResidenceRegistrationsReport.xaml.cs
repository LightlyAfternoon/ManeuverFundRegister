using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Реестр_маневренного_фонда.database.tables_classes;

namespace Реестр_маневренного_фонда.Pages.ResidenceRegistrations
{
    /// <summary>
    /// Логика взаимодействия для ResidenceRegistrationsReport.xaml
    /// </summary>
    public partial class ResidenceRegistrationsReport : Page
    {
        ApplicationContext? dbContext = ApplicationContext.GetContext();
        public ResidenceRegistrationsReport()
        {
            InitializeComponent();

            cmb_TempResident.ItemsSource = dbContext.TempResident.ToList();
            cmb_HousingFund.ItemsSource = dbContext.HousingFund.ToList();
            //dg_RegistrationsReport.ItemsSource = registrations;
        }

        private void cmb_TempResident_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] words;

            words = cmb_TempResident.Text.ToString().Split(' ');
            List<TempResident> findList = dbContext.TempResident.AsEnumerable().ToList();

            foreach (string word in words)
            {
                findList = findList.Where(h => h.FullName.ToLower().Contains(word.ToLower())).ToList();
                cmb_TempResident.ItemsSource = findList;
            }

            cmb_TempResident.IsDropDownOpen = true;
        }

        private void cmb_HousingFund_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] words;

            words = cmb_HousingFund.Text.ToString().Split(new char[] { ' ', ',' });
            List<HousingFund> findList = dbContext.HousingFund.AsEnumerable().ToList();

            foreach (string word in words)
            {
                findList = findList.Where(h => h.FullAddress.ToLower().Contains(word.ToLower())).ToList();
                cmb_HousingFund.ItemsSource = findList;
            }

            cmb_HousingFund.IsDropDownOpen = true;
        }

        private void bt_Filter_Click(object sender, RoutedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dg_RegistrationsReport.ItemsSource).Refresh();
        }

        private void bt_ShowOrHideFilterGrid_Click(object sender, RoutedEventArgs e)
        {
            if (gr_FilterGrid.Visibility == Visibility.Collapsed)
            {
                gr_FilterGrid.Visibility = Visibility.Visible;
                bt_ShowOrHideFilterGrid.Content = "^";
            }
            else
            {
                gr_FilterGrid.Visibility = Visibility.Collapsed;
                bt_ShowOrHideFilterGrid.Content = "v";
            }
        }

        private void CollectionViewSource_Filter(object sender, System.Windows.Data.FilterEventArgs e)
        {

            var currentResidenceRegistrations = dbContext.ResidenceRegistration.OrderBy(r => r.DateStartResidence).ToList();

            if (cmb_TempResident.SelectedItem != null)
            {
                currentResidenceRegistrations = currentResidenceRegistrations.Where(a => a.TempResidentId == (cmb_TempResident.SelectedItem as TempResident).IdTempResident).ToList();
            }
            if (cmb_HousingFund.SelectedItem != null)
            {
                currentResidenceRegistrations = currentResidenceRegistrations.Where(a => a.HousingFundId == (cmb_HousingFund.SelectedItem as HousingFund).IdHousingFund).ToList();
            }
            if (dp_DateStartResidence.SelectedDate != null)
            {
                currentResidenceRegistrations = currentResidenceRegistrations.Where(a => a.DateStartResidence >= dp_DateStartResidence.SelectedDate).ToList();
            }
            if (dp_DateEndResidence.SelectedDate != null)
            {
                currentResidenceRegistrations = currentResidenceRegistrations.Where(a => a.DateEndResidence <= dp_DateEndResidence.SelectedDate).ToList();
            }

            ResidenceRegistration r = e.Item as ResidenceRegistration;
            if (r != null)
            // If filter is turned on, filter completed items.
            {
                if (currentResidenceRegistrations.Any(rr => rr.IdRegistration == r.IdRegistration))
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
        }
    }
    public class ResidenceRegistrations : ObservableCollection<ResidenceRegistration>
    {
        public ResidenceRegistrations()
        {
            ApplicationContext? dbContext = ApplicationContext.GetContext();
            dbContext.HousingFund.Load();
            dbContext.TempResident.Load();
            foreach (ResidenceRegistration registration in Registrations)
            {
                Add(registration);
            }
        }

        ObservableCollection<ResidenceRegistration> Registrations
        {
            get
            {
                ApplicationContext? dbContext = ApplicationContext.GetContext();
                ObservableCollection<ResidenceRegistration> registrations = new ObservableCollection<ResidenceRegistration>();

                foreach (ResidenceRegistration residenceRegistration in dbContext.ResidenceRegistration)
                {
                    registrations.Add(residenceRegistration);
                }
                return registrations;
            }
        }
    }
}