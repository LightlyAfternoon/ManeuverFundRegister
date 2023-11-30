using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Реестр_маневренного_фонда.Database.TablesClasses;
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
        ApplicationContext dbContext;
        List<Notification> notifications = new();

        public MainWindow()
        {
            InitializeComponent();

            UpdateDB();

            DeleteTempFilesClass.deleteTempFiles();
            Directory.CreateDirectory(Path.GetTempPath() + @"\ManeuverFund");

            MainFrameObj.mainFrame = fr_Frame;

            using (TaskService ts = new TaskService())
            {
                // Создание процесса консольной программы для проверки уведомлений
                Process.Start(Directory.GetParent(Assembly.GetExecutingAssembly().Location.ToString()) + "\\ExecuteNotificationManagerClass.exe");

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

        private void UpdateDB()
        {
            FormattableString addHouseDecreeTableCommand = $"CREATE TABLE IF NOT EXISTS HouseDecree (IdHouseDecree INTEGER NOT NULL, DecreeId INTEGER NOT NULL, HousingFundId INTEGER NOT NULL, FOREIGN KEY(DecreeId) REFERENCES Decree(IdDecree), FOREIGN KEY(HousingFundId) REFERENCES HousingFund(IdHousingFund), PRIMARY KEY(IdHouseDecree AUTOINCREMENT));";
            dbContext = ApplicationContext.GetContext();
            dbContext.Database.ExecuteSql(addHouseDecreeTableCommand);

            foreach (Decree decree in dbContext.Decree.ToList())
            {
                int firstIdDecree = dbContext.Decree.First(d => d.NumberDecree == decree.NumberDecree && d.DateDecree == decree.DateDecree).IdDecree;
                if (dbContext.HouseDecree.Count(hd => hd.DecreeId == firstIdDecree && hd.HousingFundId == decree.HousingFundId) < 1)
                {
                    HouseDecree houseDecree = new HouseDecree();
                    houseDecree.DecreeId = firstIdDecree;
                    houseDecree.HousingFundId = decree.HousingFundId;
                    dbContext.HouseDecree.Add(houseDecree);

                    if (decree != dbContext.Decree.First(d => d.NumberDecree == decree.NumberDecree && d.DateDecree == decree.DateDecree))
                    {
                        dbContext.Decree.Remove(decree);
                    }
                    dbContext.SaveChanges();
                }
            }

            FormattableString changeAgreementNumberCommand = $"PRAGMA writable_schema=ON;PRAGMA foreign_keys=OFF; CREATE TABLE AgreementTemp (IdAgreement INTEGER NOT NULL UNIQUE,NumberAgreement INTEGER,TempResidentId INTEGER NOT NULL,HousingFundId INTEGER NOT NULL,DateConclusionAgreement DATETIME NOT NULL,DateEndAgreement DATETIME NOT NULL,DateTerminationAgreement DATETIME,Remark NVARCHAR,File VARBINARY,PRIMARY KEY(IdAgreement AUTOINCREMENT),FOREIGN KEY(HousingFundId) REFERENCES HousingFund(IdHousingFund),FOREIGN KEY(TempResidentId) REFERENCES TempResident(IdTempResident)); INSERT INTO AgreementTemp(IdAgreement,NumberAgreement,TempResidentId,HousingFundId,DateConclusionAgreement,DateEndAgreement,DateTerminationAgreement,Remark,File) SELECT IdAgreement,NumberAgreement,TempResidentId,HousingFundId,DateConclusionAgreement,DateEndAgreement,DateTerminationAgreement,Remark,File FROM Agreement; DROP TABLE Agreement; ALTER TABLE AgreementTemp RENAME TO Agreement; PRAGMA foreign_keys=ON; PRAGMA writable_schema=OFF;";
            dbContext.Database.ExecuteSql(changeAgreementNumberCommand);

            try
            {
                if (dbContext.HousingFund.Count(h => h.EntranceNumber != null) > 1)
                {
                }
                if (dbContext.HousingFund.Count(h => h.Sewerage != null) > 1)
                {
                }
            }
            catch
            {
                FormattableString changeHousingFundCommand1 = $"ALTER TABLE HousingFund ADD COLUMN LocalityId INTEGER;ALTER TABLE HousingFund ADD COLUMN FloorNumber INTEGER;ALTER TABLE HousingFund ADD COLUMN CWS BIT;ALTER TABLE HousingFund ADD COLUMN HWS BIT;ALTER TABLE HousingFund ADD COLUMN CG BIT;ALTER TABLE HousingFund ADD COLUMN BG BIT;ALTER TABLE HousingFund ADD COLUMN SH BIT;ALTER TABLE HousingFund ADD COLUMN CH BIT;ALTER TABLE HousingFund ADD COLUMN Electricity BIT;ALTER TABLE HousingFund ADD COLUMN KeysAvailability BIT;ALTER TABLE HousingFund ADD COLUMN EntranceNumber INTEGER;ALTER TABLE HousingFund ADD COLUMN Sewerage BIT;";
                dbContext.Database.ExecuteSql(changeHousingFundCommand1);
                FormattableString changeHousingFundCommand2 = $"PRAGMA writable_schema=ON;PRAGMA foreign_keys=OFF;CREATE TABLE HousingFundTemp(IdHousingFund INTEGER NOT NULL UNIQUE,LocalityId INTEGER,StreetId INTEGER,HouseNumber NVARCHAR(10),EntranceNumber INTEGER,FloorNumber INTEGER,ApartmentNumber INTEGER,RoomNumber NVARCHAR(20),DecreeArea REAL,RegisterArea REAL,Remark NVARCHAR,CWS BIT,HWS BIT,CG BIT,BG BIT,SH BIT,CH BIT,Electricity BIT,KeysAvailability BIT,Sewerage BIT,ImprovementDegreeId INTEGER,PRIMARY KEY(IdHousingFund AUTOINCREMENT),FOREIGN KEY(StreetId) REFERENCES Street(IdStreet),FOREIGN KEY(LocalityId) REFERENCES Locality(IdLocality),FOREIGN KEY(ImprovementDegreeId) REFERENCES ImprovementDegree(IdImprovementDegree));INSERT INTO HousingFundTemp(IdHousingFund,LocalityId,StreetId,HouseNumber,ApartmentNumber,RoomNumber,DecreeArea,RegisterArea,Remark,EntranceNumber,FloorNumber,CWS,HWS,CG,BG,SH,CH,Electricity,KeysAvailability,Sewerage,ImprovementDegreeId) SELECT h.IdHousingFund,s.LocalityId,s.IdStreet,h.HouseNumber,h.ApartmentNumber,h.RoomNumber,h.DecreeArea,h.RegisterArea,h.Remark,h.EntranceNumber,h.FloorNumber,h.CWS,h.HWS,h.CG,h.BG,h.SH,h.CH,h.Electricity,h.KeysAvailability,h.Sewerage,h.ImprovementDegreeId FROM HousingFund h INNER JOIN Street s ON h.StreetId=s.IdStreet; DROP TABLE HousingFund; ALTER TABLE HousingFundTemp RENAME TO HousingFund; PRAGMA foreign_keys=ON; PRAGMA writable_schema=OFF;";
                dbContext.Database.ExecuteSql(changeHousingFundCommand2);
                foreach (HousingFund housingFund in dbContext.HousingFund.ToList())
                {
                    if (housingFund.CWS == null)
                        housingFund.CWS = false;

                    if (housingFund.HWS == null)
                        housingFund.HWS = false;

                    if (housingFund.BG == null)
                        housingFund.BG = false;

                    if (housingFund.CG == null)
                        housingFund.CG = false;

                    if (housingFund.CH == null)
                        housingFund.CH = false;

                    if (housingFund.SH == null)
                        housingFund.SH = false;

                    if (housingFund.Electricity == null)
                        housingFund.Electricity = false;

                    if (housingFund.KeysAvailability == null)
                        housingFund.KeysAvailability = true;

                    if (housingFund.Sewerage == null)
                        housingFund.Sewerage = false;
                        
                    dbContext.HousingFund.Update(housingFund);
                    dbContext.SaveChanges();
                }
            }
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
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Show();
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
                ApplicationContext.GetContext().Notification.First(n => n == notification).IsViewed = true;
                ApplicationContext.GetContext().SaveChanges();
            }
        }
    }
}
