﻿using System.Linq;
using System.Windows;
using Реестр_маневренного_фонда.database.tables_classes;
using Реестр_маневренного_фонда.Pages;

namespace Реестр_маневренного_фонда.TablesManagersClasses
{
    public class LocalityManager
    {
        private ApplicationContext dbContext = ApplicationContext.GetContext();

        private string errors = string.Empty;

        private void showErrors(string? nameLocality)
        {
            if (string.IsNullOrWhiteSpace(nameLocality))
            {
                errors += ("Необходимо ввести название населённого пункта");
            }
        }
     
        public void AddLocality(string? nameLocality)
        {
            Locality newLocality = new Locality();

            if (!string.IsNullOrWhiteSpace(nameLocality) && dbContext.Locality.Any(l => l.NameLocality.Trim().Equals(nameLocality.Trim())))
            {
                errors += ("Населённый пункт с данным наименованием уже есть");
            }
            showErrors(nameLocality);
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(errors);
                errors = string.Empty;
            }
            else
            {
                try
                {
                    newLocality.NameLocality = nameLocality;

                    dbContext.Locality.Add(newLocality);
                    dbContext.SaveChanges();

                    MessageBox.Show("Населённый пункт добавлен", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainFrameObj.mainFrame.Navigate(new LocalitiesViewPage());
                }
                catch
                {
                    MessageBox.Show("Не удалось добавить населённый пункт", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void EditLocality(Locality currentLocality, string? nameLocality)
        {
            if (!string.IsNullOrWhiteSpace(nameLocality) && dbContext.Locality.Any(l => l.IdLocality != currentLocality.IdLocality && l.NameLocality.Trim().Equals(nameLocality.Trim())))
            {
                errors += ("Населённый пункт с данным наименованием уже есть");
            }
            showErrors(nameLocality);
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(errors);
                errors = string.Empty;
            }
            else
            {
                try
                {
                    currentLocality.NameLocality = nameLocality;

                    dbContext.Locality.Update(currentLocality);
                    dbContext.SaveChanges();

                    MessageBox.Show("Населённый пункт изменён", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainFrameObj.mainFrame.Navigate(new LocalitiesViewPage());
                }
                catch
                {
                    MessageBox.Show("Не удалось изменить населённый пункт", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void RemoveLocality(Locality currenLocality)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show($"Удалить населённый пункт?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        if (dbContext.HousingFund.Count(h => h.LocalityId == currenLocality.IdLocality) < 1)
                        {
                            dbContext.Locality.Remove(currenLocality);
                            dbContext.SaveChanges();

                            MessageBox.Show("Населённый пункт удалён", "", MessageBoxButton.OK, MessageBoxImage.Information);
                            MainFrameObj.mainFrame.Navigate(new LocalitiesViewPage());
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить населённый пункт", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Не удалось удалить населённый пункт", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
    }
}