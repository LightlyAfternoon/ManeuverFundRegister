using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using Реестр_маневренного_фонда.Pages;
using Реестр_маневренного_фонда.TablesManagersClasses;

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
            //работает
            //NotificationManager notificationManager = new NotificationManager();
            //notificationManager.AddNotification();
            //notificationManager.RemoveNotification();

            using (TaskService ts = new TaskService())
            {
                // Создание новой задачи и добавление её описания
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "Добавление и удаление уведомлений";

                // Создание триггера, который будет запускать задачу в 10 часов каждый день
                td.Triggers.Add(new DailyTrigger { DaysInterval = 1 });

                // Определение команды, которую нужно запустить
                td.Actions.Add(new ExecAction("C:\\Users\\Vika\\source\\repos\\LightlyAfternoon\\ExecuteNotificationManagerClass\\bin\\Debug\\net6.0-windows10.0.17763.0\\ExecuteNotificationManagerClass.exe"));

                // Регистрация задачи в планировщике
                ts.RootFolder.RegisterTaskDefinition(@"AddAndRemoveNotifications", td);
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
