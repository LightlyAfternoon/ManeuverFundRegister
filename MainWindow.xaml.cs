using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using Реестр_маневренного_фонда.Pages;

namespace Реестр_маневренного_фонда
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();

        public MainWindow()
        {
            InitializeComponent();

            dbContext.Agreement.Load();
            dbContext.Notification.Load();
            List<Agreement> listAgreements = dbContext.Agreement.ToList();
            foreach (Agreement agreement in listAgreements)
            {
                if (agreement.DateEndAgreement <= DateTime.Now.AddMonths(6) && dbContext.Notification.Count(n => n.AgreementId == agreement.IdAgreement) < 1)
                {
                    Notification newNotification = new Notification
                    {
                        AgreementId = agreement.IdAgreement,
                        RecievingDate = DateTime.Now
                    };

                    dbContext.Notification.Add(newNotification);
                    dbContext.SaveChanges();
                }
            }
            foreach (Notification notification in dbContext.Notification.ToList())
            {
                if (notification.RecievingDate > DateTime.Now.AddMonths(6))
                {
                    dbContext.Remove(notification);
                    dbContext.SaveChanges();
                }
            }
        }

        private void bt_AgreementsViewPage_Click(object sender, RoutedEventArgs e)
        {
            fr_Frame.Navigate(new AgreementsViewPage());
        }

        private void bt_DecreesViewPage_Click(object sender, RoutedEventArgs e)
        {
            fr_Frame.Navigate(new DecreesViewPage());
        }

        private void bt_HousingFundViewPage_Click(object sender, RoutedEventArgs e)
        {
            fr_Frame.Navigate(new HousingFundViewPage());
        }

        private void bt_LocalitiesViewPage_Click(object sender, RoutedEventArgs e)
        {
            fr_Frame.Navigate(new LocalitiesViewPage());
        }

        private void bt_ResidenceRegistrationViewPage_Click(object sender, RoutedEventArgs e)
        {
            fr_Frame.Navigate(new ResidenceRegistrationViewPage());
        }

        private void bt_StreetsViewPage_Click(object sender, RoutedEventArgs e)
        {
            fr_Frame.Navigate(new StreetsViewPage());
        }

        private void bt_TempResidentsViewPage_Click(object sender, RoutedEventArgs e)
        {
            fr_Frame.Navigate(new TempResidentsViewPage());
        }

        private void bt_NotificationsView_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
