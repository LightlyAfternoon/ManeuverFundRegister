using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.TaskScheduler;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
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
        public MainWindow()
        {
            InitializeComponent();

            MainFrameObj.mainFrame = fr_Frame;

            using (TaskService ts = new TaskService())
            {
                
                    // Создание процесса консольной программы для проверки уведомлений
                    Process.Start(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString()).ToString()).ToString()).ToString()) + "\\ExecuteNotificationManagerClass\\bin\\Debug\\net6.0-windows10.0.17763.0\\ExecuteNotificationManagerClass.exe");

                    // Создание новой задачи и добавление её описания
                    TaskDefinition td = ts.NewTask();
                    td.RegistrationInfo.Description = "Добавление и удаление уведомлений";

                    // Создание триггера, который будет запускать задачу в 10 часов каждый день
                    td.Triggers.Add(new DailyTrigger { DaysInterval = 1 });

                    // Определение команды, которую нужно запустить
                    td.Actions.Add(new ExecAction(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString()).ToString()).ToString()).ToString()) + "\\ExecuteNotificationManagerClass\\bin\\Debug\\net6.0-windows10.0.17763.0\\ExecuteNotificationManagerClass.exe"));

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
            try
            {
                ApplicationContext.GetContext().Agreement.Load();
                lv_pop.ItemsSource = ApplicationContext.GetContext().Notification.ToList();

                if (!pop_Notif.IsOpen)
                {
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
            DragMove();

            if (pop_Notif.IsOpen)
                pop_Notif.IsOpen = false;
        }

        private void bt_CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void bt_MaxWindow_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void bt_MinWindow_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void bt_Info_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
