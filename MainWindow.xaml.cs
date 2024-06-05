using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.TaskScheduler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Реестр_маневренного_фонда.Pages;
using Реестр_маневренного_фонда.Pages.HousingsFund;
using Реестр_маневренного_фонда.Pages.ResidenceRegistrations;

namespace Реестр_маневренного_фонда
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationContext? dbContext;
        List<Notification> notifications = new();

        public MainWindow()
        {
            InitializeComponent();

            DeleteTempFilesClass.DeleteTempFiles();
            Directory.CreateDirectory(Path.GetTempPath() + @"\ManeuverFund");

            MainFrameObj.mainFrame = fr_Frame;

            // Создание процесса консольной программы для проверки уведомлений
            if (File.Exists(Directory.GetParent(Assembly.GetExecutingAssembly().Location.ToString()) + "\\ExecuteNotificationManagerClass.exe"))
            {
                Process.Start(Directory.GetParent(Assembly.GetExecutingAssembly().Location.ToString()) + "\\ExecuteNotificationManagerClass.exe");
            }
            else
            {
                Process.Start(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString()).ToString()).ToString()).ToString()) + "\\ExecuteNotificationManagerClass\\bin\\Debug\\net6.0-windows10.0.17763.0\\ExecuteNotificationManagerClass.exe");
            }

            /*try
            {
                using (TaskService ts = new TaskService())
                {
                    // Создание новой задачи и добавление её описания
                    TaskDefinition td = ts.NewTask();
                    td.RegistrationInfo.Description = "Добавление и удаление уведомлений";

                    // Создание триггера, который будет запускать задачу в 10 часов каждый день
                    td.Triggers.Add(new DailyTrigger { DaysInterval = 1 });

                    // Определение команды, которую нужно запустить
                    td.Actions.Add(new ExecAction(Directory.GetParent(Assembly.GetExecutingAssembly().Location.ToString()) + "\\ExecuteNotificationManagerClass.exe"));

                    // Регистрация задачи в планировщике
                    ts.RootFolder.RegisterTaskDefinition(@"AddAndRemoveNotifications", td);
                }
            }
            catch { }*/
        }

        private void bt_AgreementsViewPage_Click(object sender, RoutedEventArgs e)
        {
            fr_Frame.Navigate(new AgreementsViewPage());

            bt_MenuVisibility.Content = "☰";
            sp_ViewPageButtons.Visibility = Visibility.Collapsed;
        }

        private void bt_DecreesViewPage_Click(object sender, RoutedEventArgs e)
        {
            fr_Frame.Navigate(new DecreesViewPage());

            bt_MenuVisibility.Content = "☰";
            sp_ViewPageButtons.Visibility = Visibility.Collapsed;
        }

        private void bt_HousingFundViewPage_Click(object sender, RoutedEventArgs e)
        {
            fr_Frame.Navigate(new HousingFundViewPage());

            bt_MenuVisibility.Content = "☰";
            sp_ViewPageButtons.Visibility = Visibility.Collapsed;
        }

        private void bt_LocalitiesViewPage_Click(object sender, RoutedEventArgs e)
        {
            fr_Frame.Navigate(new LocalitiesViewPage());

            bt_MenuVisibility.Content = "☰";
            sp_ViewPageButtons.Visibility = Visibility.Collapsed;
        }

        private void bt_ResidenceRegistrationViewPage_Click(object sender, RoutedEventArgs e)
        {
            fr_Frame.Navigate(new ResidenceRegistrationViewPage());

            bt_MenuVisibility.Content = "☰";
            sp_ViewPageButtons.Visibility = Visibility.Collapsed;
        }

        private void bt_StreetsViewPage_Click(object sender, RoutedEventArgs e)
        {
            fr_Frame.Navigate(new StreetsViewPage());

            bt_MenuVisibility.Content = "☰";
            sp_ViewPageButtons.Visibility = Visibility.Collapsed;
        }

        private void bt_TempResidentsViewPage_Click(object sender, RoutedEventArgs e)
        {
            fr_Frame.Navigate(new TempResidentsViewPage());

            bt_MenuVisibility.Content = "☰";
            sp_ViewPageButtons.Visibility = Visibility.Collapsed;
        }

        private void bt_NotificationsView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!pop_Notif.IsOpen)
                {
                    ApplicationContext.GetContext().Agreement.Load();
                    try
                    {
                        notifications = ApplicationContext.GetContext().Notification.OrderByDescending(n => n.RecievingDate).ToList();
                        lv_pop.ItemsSource = notifications;
                    }
                    catch { }

                    pop_Notif.IsOpen = true;
                }
                else
                {
                    pop_Notif.IsOpen = false;
                }
            }
            catch
            {
                MessageBox.Show("Не получилось подключится к базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (pop_Notif.IsOpen)
                pop_Notif.IsOpen = false;
                
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void bt_CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void bt_MaxWindow_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                ResizeMode = ResizeMode.CanResize;
                bt_MaxWindow.Content = "☐";
            }
            else
            {
                ResizeMode = ResizeMode.NoResize;
                WindowState = WindowState.Maximized;
                bt_MaxWindow.Content = "❐";
            }
        }

        private void bt_MinWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void bt_Info_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", $"{Directory.GetParent(Assembly.GetExecutingAssembly().Location.ToString())}\\Resources\\Руководство пользователя.docx");
        }

        private void bt_MenuVisibility_Click(object sender, RoutedEventArgs e)
        {
            if (sp_ViewPageButtons.Visibility == Visibility.Collapsed)
            {
                bt_MenuVisibility.Content= "✕";
                sp_ViewPageButtons.Visibility = Visibility.Visible;
            }
            else
            {
                bt_MenuVisibility.Content= "☰";
                sp_ViewPageButtons.Visibility = Visibility.Collapsed;
            }
        }

        private void pop_Notif_Closed(object sender, System.EventArgs e)
        {
            foreach (Notification notification in notifications)
            {
                try
                {
                    notification.IsViewed = true;
                    ApplicationContext.GetContext().Update(notification);
                    ApplicationContext.GetContext().SaveChanges();
                }
                catch { }
            }
        }
    }
}