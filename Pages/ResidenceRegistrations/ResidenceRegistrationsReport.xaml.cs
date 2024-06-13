using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        }

        public byte[] Generate(List<ResidenceRegistration> registrations)
        {
            List<int> array = new List<int>();
            foreach (ResidenceRegistration registration in registrations)
            {
                foreach (int startYear in registration.StartYear)
                {
                    array.Add(startYear);
                }
            }
            array.Sort();

            List<Tuple<int, ResidenceRegistration>> listTuple = new List<Tuple<int, ResidenceRegistration>>();
            foreach (int year in array)
            {
                foreach(ResidenceRegistration registration in registrations)
                {
                    if (registration.DateStartResidence.Year <= year && (registration.DateEndResidence == null || registration.DateEndResidence.Value.Year >= year))
                    {
                        listTuple.Add(new Tuple<int, ResidenceRegistration>(year, registration));
                    }
                }
            }
            listTuple = listTuple.Distinct().ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            ExcelPackage package = new();
            var sheet = package.Workbook.Worksheets.Add("Факты проживания в жилье");

            sheet.Cells[1, 1, 1, 1].LoadFromArrays(new object[][] { new[] { "Адрес", "Наниматель", "Проживает с", "До" } });
            int row = 2;
            int column = 1;
            foreach (var tuple in listTuple.GroupBy(l => l.Item1))
            {
                sheet.Cells[$"A{row}:D{row}"].Merge = true;
                sheet.Cells[$"A{row}:D{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                sheet.Cells[$"A{row}:D{row}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[$"A{row}:D{row}"].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                sheet.Cells[$"A{row}:D{row}"].Style.Font.Bold = true;
                sheet.Cells[row, column].Value = tuple.Key;
                row++;
                foreach (var item in tuple.GroupBy(t => t.Item2.HousingFund.Locality.NameLocality))
                {
                    sheet.Cells[$"A{row}:D{row}"].Merge = true;
                    sheet.Cells[$"A{row}:D{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    sheet.Cells[$"A{row}:D{row}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[$"A{row}:D{row}"].Style.Fill.BackgroundColor.SetColor(Color.AliceBlue);
                    sheet.Cells[$"A{row}:D{row}"].Style.Font.Color.SetColor(Color.DarkBlue);
                    sheet.Cells[row, column].Value = item.Key;
                    row++;
                    foreach (var item2 in item)
                    {
                        ResidenceRegistration registration = item2.Item2;
                        sheet.Cells[row, column].Value = registration.HousingFund.FullAddress;
                        sheet.Cells[row, column + 1].Value = registration.TempResident.FullName;
                        sheet.Cells[row, column + 2].Value = registration.DateStartResidence.ToShortDateString();
                        if (registration.DateEndResidence != null)
                            sheet.Cells[row, column + 3].Value = registration.DateEndResidence.Value.ToShortDateString();
                        else
                            sheet.Cells[row, column + 3].Value = "";

                        row++;
                    }
                }
            }
            
            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            return package.GetAsByteArray();
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

        private List<ResidenceRegistration> getFilteredList()
        {
            var currentResidenceRegistrations = dbContext.ResidenceRegistration.Distinct().OrderBy(r => r.DateStartResidence).ToList();

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
                currentResidenceRegistrations = currentResidenceRegistrations.Where(a => a.DateStartResidence >= dp_DateStartResidence.SelectedDate || a.DateEndResidence == null || (a.DateEndResidence != null && dp_DateStartResidence.SelectedDate <= a.DateEndResidence)).ToList();
            }
            if (dp_DateEndResidence.SelectedDate != null)
            {
                currentResidenceRegistrations = currentResidenceRegistrations.Where(a => a.DateEndResidence == null || a.DateEndResidence <= dp_DateEndResidence.SelectedDate).ToList();
            }

            return currentResidenceRegistrations;
        }

        private void CollectionViewSource_Filter(object sender, System.Windows.Data.FilterEventArgs e)
        {
            ResidenceRegistration r = e.Item as ResidenceRegistration;
            if (r != null)
            {
                if (getFilteredList().Any(rr => rr.IdRegistration == r.IdRegistration))
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
        }

        private void bt_GetExcel_Click(object sender, RoutedEventArgs e)
        {
            var ReportExcel = Generate(getFilteredList());

            File.WriteAllBytes($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{DateTime.Now.ToShortDateString()} Факты проживания в жилье.xlsx", ReportExcel);

            Process.Start("explorer.exe", $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}");
        }
    }

    public class ResidenceRegistrations : ObservableCollection<ResidenceRegistration>
    {
        public ResidenceRegistrations()
        {
            ApplicationContext? dbContext = ApplicationContext.GetContext();
            dbContext.Locality.Load();
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

                foreach (ResidenceRegistration residenceRegistration in dbContext.ResidenceRegistration.Distinct())
                {
                    registrations.Add(residenceRegistration);
                }
                return registrations;
            }
        }
    }
}