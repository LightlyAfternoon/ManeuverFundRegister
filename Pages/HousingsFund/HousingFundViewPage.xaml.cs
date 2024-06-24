using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Реестр_маневренного_фонда.database.tables_classes;
using Реестр_маневренного_фонда.TablesManagersClasses;

namespace Реестр_маневренного_фонда.Pages.HousingsFund
{
    /// <summary>
    /// Логика взаимодействия для HousingFundViewPage.xaml
    /// </summary>
    public partial class HousingFundViewPage : Page
    {
        ApplicationContext dbContext;
        HousingFund currentHousingFund;

        public HousingFundViewPage()
        {
            InitializeComponent();

            try
            {
                dbContext = ApplicationContext.GetContext();

                lb_HousingFund.ItemsSource = dbContext.HousingFund.AsEnumerable().OrderBy(h => h.listNum).ToList();

                cmb_Locality.ItemsSource = dbContext.Locality.ToList();
                cmb_Street.IsEnabled = false;
            }
            catch
            {
                MessageBox.Show("Не получилось подключится к базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Filtering()
        {
            var currentHousesInFund = dbContext.HousingFund.AsEnumerable().OrderBy(h => h.listNum).ToList();
            
            if (cmb_Locality.SelectedItem != null)
            {
                currentHousesInFund = currentHousesInFund.Where(h => h.LocalityId == (cmb_Locality.SelectedItem as Locality).IdLocality).ToList();
            }
            if (cmb_Street.SelectedItem != null)
            {
                currentHousesInFund = currentHousesInFund.Where(h => h.StreetId == (cmb_Street.SelectedItem as Street).IdStreet).ToList();
            }
            if (!string.IsNullOrWhiteSpace(tb_HouseNumber.Text))
            {
                currentHousesInFund = currentHousesInFund.Where(h => h.HouseNumber.ToLower().Contains(tb_HouseNumber.Text.ToLower())).ToList();
            }
            if (!string.IsNullOrWhiteSpace(tb_ApartmentNumber.Text))
            {
                currentHousesInFund = currentHousesInFund.Where(h => h.ApartmentNumber.ToString() == tb_ApartmentNumber.Text).ToList();
            }
            if (!string.IsNullOrWhiteSpace(tb_DecreeArea.Text))
            {
                currentHousesInFund = currentHousesInFund.Where(h => h.DecreeArea.ToString() == tb_DecreeArea.Text).ToList();
            }
            if (!string.IsNullOrWhiteSpace(tb_RegisterArea.Text))
            {
                currentHousesInFund = currentHousesInFund.Where(h => h.RegisterArea.ToString() == tb_RegisterArea.Text).ToList();
            }
            if (cmb_Freedom.SelectedIndex == 1)
            {
                currentHousesInFund = currentHousesInFund.AsEnumerable().Where(h => h.Freedom.Equals("Свободно")).ToList();
            }
            else if (cmb_Freedom.SelectedIndex == 2)
            {
                currentHousesInFund = currentHousesInFund.AsEnumerable().Where(h => h.Freedom.Equals("Занято")).ToList();
            }
            else if (cmb_Freedom.SelectedIndex == 3)
            {
                currentHousesInFund = currentHousesInFund.AsEnumerable().Where(h => h.Freedom.Equals("Исключено")).ToList();
            }
            else if (cmb_Freedom.SelectedIndex == 4)
            {
                currentHousesInFund = currentHousesInFund.AsEnumerable().Where(h => h.Freedom.Equals("Нет постановления")).ToList();
            }

            lb_HousingFund.ItemsSource = currentHousesInFund.ToList();
        }

        private void bt_EditHousingFund_Click(object sender, RoutedEventArgs e)
        {
            currentHousingFund = (sender as Button).DataContext as HousingFund;
            NavigationService.Navigate(new EditHouseInFundPage(currentHousingFund));
        }

        private void bt_DeleteHousingFund_Click(object sender, RoutedEventArgs e)
        {
            currentHousingFund = (sender as Button).DataContext as HousingFund;
            HousingFundManager hm = new HousingFundManager();
            hm.RemoveHouseInFund(currentHousingFund);
        }

        private void bt_Filter_Click(object sender, RoutedEventArgs e)
        {
            Filtering();
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

        private void bt_AddNewHouseInFund_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddNewHouseInFundPage());
        }

        public byte[] Generate(List<HousingFund> housingFund)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            ExcelPackage package = new();
            var sheet = package.Workbook.Worksheets.Add("Реестр жилья фонда");

            sheet.Cells[1, 1, 1, 1].LoadFromArrays(new object[][] { new[] { "Населённый пункт", "Улица", "Ключи", "№ дома", "№ подъезда", "№ этажа", "№ квартиры", "№ комнаты", "Площадь по постановлению", "Площадь по реестру", "ХВС", "ГВС", "Газ централизованный", "Газ баллонный", "Отопление печное", "Отопление централизованное", "Электричество", "Примечание", "Статус" } });
            int row = 2;
            int column = 1;
            foreach (var item in housingFund)
            {
                sheet.Cells[row, column].Value = item.Locality.NameLocality;
                if (item.StreetId != null)
                    sheet.Cells[row, column + 1].Value = item.Street.NameStreet;
                else
                    sheet.Cells[row, column + 1].Value = "";
                if (item.KeysAvailability != null)
                    sheet.Cells[row, column + 2].Value = "+";
                else
                    sheet.Cells[row, column + 2].Value = "-";
                sheet.Cells[row, column + 3].Value = item.HouseNumber;
                sheet.Cells[row, column + 4].Value = item.EntranceNumber;
                sheet.Cells[row, column + 5].Value = item.FloorNumber;
                sheet.Cells[row, column + 6].Value = item.ApartmentNumber;
                sheet.Cells[row, column + 7].Value = item.RoomNumber;
                sheet.Cells[row, column + 8].Value = item.DecreeArea;
                sheet.Cells[row, column + 9].Value = item.RegisterArea;
                if (item.CWS == true)
                    sheet.Cells[row, column + 10].Value = "+";
                else
                    sheet.Cells[row, column + 10].Value = "-";
                if (item.HWS == true)
                    sheet.Cells[row, column + 11].Value = "+";
                else
                    sheet.Cells[row, column + 11].Value = "-";
                if (item.CG == true)
                    sheet.Cells[row, column + 12].Value = "+";
                else
                    sheet.Cells[row, column + 12].Value = "-";
                if (item.BG == true)
                    sheet.Cells[row, column + 13].Value = "+";
                else
                    sheet.Cells[row, column + 13].Value = "-";
                if (item.SH == true)
                    sheet.Cells[row, column + 14].Value = "+";
                else
                    sheet.Cells[row, column + 14].Value = "-";
                if (item.CH == true)
                    sheet.Cells[row, column + 15].Value = "+";
                else
                    sheet.Cells[row, column + 15].Value = "-";
                if (item.Electricity == true)
                    sheet.Cells[row, column + 16].Value = "+";
                else
                    sheet.Cells[row, column + 16].Value = "-";
                sheet.Cells[row, column + 17].Value = item.Remark;
                sheet.Cells[row, column + 18].Value = item.Freedom;

                row++;
            }
            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            return package.GetAsByteArray();
        }

        private void bt_GetExcel_Click(object sender, RoutedEventArgs e)
        {
            var ReportExcel = Generate(lb_HousingFund.ItemsSource as List<HousingFund>);

            string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{DateTime.Now.ToShortDateString()} Реестр жилья фонда.xlsx";
            try
            {
                File.WriteAllBytes(path, ReportExcel);

                Process.Start("explorer.exe", $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}");
                Process.Start("explorer.exe", path);
            }
            catch
            {
                MessageBox.Show("Не удалось изменить или открыть файл.");
            }
        }

        private void cmb_Locality_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmb_Street.SelectedItem = null;
            if (cmb_Locality.SelectedItem != null)
            {
                cmb_Street.ItemsSource = dbContext.Street.Where(s => s.LocalityId == (cmb_Locality.SelectedItem as Locality).IdLocality).ToList();
                cmb_Street.IsEnabled = true;
            }
            else
            {
                cmb_Street.ItemsSource = null;
                cmb_Street.IsEnabled = false;
            }
        }
    }
}