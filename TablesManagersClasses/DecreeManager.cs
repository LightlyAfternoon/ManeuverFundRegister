﻿using System;
using System.Windows;
using Реестр_маневренного_фонда.Pages;

namespace Реестр_маневренного_фонда.TablesManagersClasses
{
    public class DecreeManager
    {
        private ApplicationContext dbContext = ApplicationContext.GetContext();

        private string errors = string.Empty;

        private void showErrors(string? number, DateTime? dateDecree, HousingFund? housingFund, bool? status)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                errors += ("Неоходимо ввести номер\n");
            }
            else
            {
                try
                {
                    checked
                    {
                        Convert.ToInt32(number);
                    }
                }
                catch
                {
                    errors += ("Укажите номер постановления корректно (только цифры)");
                }
            }
            if (housingFund == null)
            {
                errors += ("Необходимо выбрать жильё\n");
            }
            if (dateDecree == null)
            {
                errors += ("Необходимо выбрать дату постановления\n");
            }

            MessageBox.Show(errors);
            errors = string.Empty;
        }
        public void AddDecree(string? number, DateTime? dateDecree, HousingFund? housingFund, bool? status)
        {
            Decree newDecree = new Decree();
            
            showErrors(number, dateDecree, housingFund, status);
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(errors);
                errors = string.Empty;
            }
            else
            {
                try
                {
                    newDecree.NumberDecree = Convert.ToInt32(number);
                    newDecree.DateDecree = Convert.ToDateTime(dateDecree);
                    newDecree.HousingFund = housingFund;
                    newDecree.Status = Convert.ToBoolean(status);

                    dbContext.Decree.Add(newDecree);
                    dbContext.SaveChanges();

                    MessageBox.Show("Постановление добавлено", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainFrameObj.mainFrame.Navigate(new DecreesViewPage());
                }
                catch
                {
                    MessageBox.Show("Не удалось добавить постановление", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void EditDecree(Decree currentDecree, string? number, DateTime? dateDecree, HousingFund? housingFund, bool? status)
        {
            showErrors(number, dateDecree, housingFund, status);
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(errors);
                errors = string.Empty;
            }
            else
            {
                try
                {
                    currentDecree.NumberDecree = Convert.ToInt32(number);
                    currentDecree.DateDecree = Convert.ToDateTime(dateDecree);
                    currentDecree.HousingFund = housingFund;
                    currentDecree.Status = Convert.ToBoolean(status);

                    dbContext.Decree.Update(currentDecree);
                    dbContext.SaveChanges();

                    MessageBox.Show("Постановление изменено", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainFrameObj.mainFrame.Navigate(new DecreesViewPage());
                }
                catch
                {
                    MessageBox.Show("Не удалось изменить постановление", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void RemoveDecree(Decree currentDecree)
        {
            try
            {
                dbContext.Decree.Remove(currentDecree);
                dbContext.SaveChanges();
                
                MessageBox.Show("Постановление удалено", "", MessageBoxButton.OK, MessageBoxImage.Information);
                MainFrameObj.mainFrame.Navigate(new DecreesViewPage());
            }
            catch
            {
                MessageBox.Show("Не получилось удалить постановление", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
