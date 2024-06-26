﻿using System.Linq;
using System.Windows;
using Реестр_маневренного_фонда.database.tables_classes;
using Реестр_маневренного_фонда.Pages;

namespace Реестр_маневренного_фонда.TablesManagersClasses
{
    public class StreetManager
    {
        private ApplicationContext dbContext = ApplicationContext.GetContext();

        private string errors = string.Empty;

        private void showErrors(Locality? locality, string? nameStreet)
        {
            if (locality == null)
            {
                errors += ("Необходимо выбрать населённый пункт\n");
            }
            if (string.IsNullOrWhiteSpace(nameStreet))
            {
                errors += ("Необходимо ввести название улицы");
            }
        }
        
        public void AddStreet(Locality? locality, string? nameStreet)
        {
            if (!string.IsNullOrWhiteSpace(nameStreet) && dbContext.Street.Any(l => l.NameStreet.Trim().Equals(nameStreet.Trim())))
            {
                errors += ("Улица с данным наименованием уже есть");
            }
            Street newStreet = new Street();
            
            showErrors(locality, nameStreet);
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(errors);
                errors = string.Empty;
            }
            else
            {
                try
                {
                    newStreet.LocalityId = locality.IdLocality;
                    newStreet.NameStreet = nameStreet;

                    dbContext.Street.Add(newStreet);
                    dbContext.SaveChanges();

                    MessageBox.Show("Улица добавлена", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainFrameObj.mainFrame.Navigate(new StreetsViewPage());
                }
                catch
                {
                    MessageBox.Show("Не удалось добавить улицу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void EditStreet(Street currentStreet, Locality? locality, string? nameStreet)
        {
            if (!string.IsNullOrWhiteSpace(nameStreet) && dbContext.Street.Any(l => l.IdStreet != currentStreet.IdStreet && l.NameStreet.Trim().Equals(nameStreet.Trim())))
            {
                errors += ("Улица с данным наименованием уже есть");
            }
            showErrors(locality, nameStreet);
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(errors);
                errors = string.Empty;
            }
            else
            {
                try
                {
                    currentStreet.LocalityId = locality.IdLocality;
                    currentStreet.NameStreet = nameStreet;

                    dbContext.Street.Update(currentStreet);
                    dbContext.SaveChanges();

                    MessageBox.Show("Улица изменена", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainFrameObj.mainFrame.Navigate(new StreetsViewPage());
                }
                catch
                {
                    MessageBox.Show("Не удалось изменить улицу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void RemoveStreet(Street currenStreet)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show($"Удалить улицу?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        if (dbContext.HousingFund.Count(h => h.StreetId == currenStreet.IdStreet) < 1)
                        {
                            dbContext.Street.Remove(currenStreet);
                            dbContext.SaveChanges();

                            MessageBox.Show("Улица удалена", "", MessageBoxButton.OK, MessageBoxImage.Information);
                            MainFrameObj.mainFrame.Navigate(new StreetsViewPage());
                        }
                        else
                        {
                            MessageBox.Show("Не получилось удалить улицу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Не получилось удалить улицу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
    }
}