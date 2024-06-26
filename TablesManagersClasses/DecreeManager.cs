﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Реестр_маневренного_фонда.Database.TablesClasses;
using Реестр_маневренного_фонда.Pages;

namespace Реестр_маневренного_фонда.TablesManagersClasses
{
    public class DecreeManager
    {
        private ApplicationContext dbContext = ApplicationContext.GetContext();

        List<HouseDecree> houseDecrees = new List<HouseDecree>();
        private string errors = string.Empty;

        private void showErrors(string? number, DateTime? dateDecree, List<HousingFund>? housingFund)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                errors += ("Необходимо ввести номер\n");
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
                    errors += ("Укажите номер постановления корректно (только цифры)\n");
                }
            }
            if (housingFund.Count() == 0)
            {
                errors += ("Необходимо выбрать жильё\n");
            }
            if (dateDecree == null)
            {
                errors += ("Необходимо выбрать дату постановления");
            }
        }
        
        public void AddDecree(Decree newDecree, string? number, DateTime? dateDecree, List<HousingFund>? housingFund, bool? status)
        {
            if (!string.IsNullOrWhiteSpace(number) && int.TryParse(number, out int value) && dbContext.Decree.Count(a => a.NumberDecree == Convert.ToInt32(number)) > 0)
            {
                errors += ("Постановление с данным номером уже добавлено\n");
            }

            showErrors(number, dateDecree, housingFund);
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
                    newDecree.Status = Convert.ToBoolean(status);

                    dbContext.Decree.Add(newDecree);
                    dbContext.SaveChanges();

                    foreach (HousingFund house in housingFund)
                    {
                        if (houseDecrees.Count(hd => hd.HousingFundId == house.IdHousingFund) < 1)
                        {
                            HouseDecree newHouseDecree = new HouseDecree();
                            newHouseDecree.DecreeId = newDecree.IdDecree;
                            newHouseDecree.HousingFundId = house.IdHousingFund;

                            houseDecrees.Add(newHouseDecree);
                            dbContext.HouseDecree.Add(newHouseDecree);
                            dbContext.SaveChanges();
                        }
                    }

                    MessageBox.Show("Постановление добавлено", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainFrameObj.mainFrame.Navigate(new DecreesViewPage());
                }
                catch
                {
                    MessageBox.Show("Не удалось добавить постановление", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void EditDecree(Decree currentDecree, string? number, DateTime? dateDecree, List<HousingFund>? housingFund, bool? status)
        {
            if (!string.IsNullOrWhiteSpace(number) && int.TryParse(number, out int value) && dbContext.Decree.Count(a => a.IdDecree == currentDecree.IdDecree && a.NumberDecree == Convert.ToInt32(number)) > 0)
            {
                errors += ("Постановление с данным номером уже добавлено\n");
            }

            showErrors(number, dateDecree, housingFund);
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
                    currentDecree.Status = Convert.ToBoolean(status);

                    houseDecrees = dbContext.HouseDecree.Where(hd => hd.DecreeId == currentDecree.IdDecree).ToList();
                    foreach (HousingFund house in housingFund)
                    {
                        if (houseDecrees.Count(hd => hd.HousingFundId == house.IdHousingFund) < 1)
                        {
                            HouseDecree newHouseDecree = new HouseDecree();
                            newHouseDecree.DecreeId = currentDecree.IdDecree;
                            newHouseDecree.HousingFundId = house.IdHousingFund;

                            dbContext.HouseDecree.Add(newHouseDecree);
                            dbContext.SaveChanges();
                        }
                    }
                    foreach (HouseDecree houseDecree in houseDecrees)
                    {
                        if (housingFund.Count(h => h.IdHousingFund == houseDecree.HousingFundId) < 1)
                        {
                            dbContext.HouseDecree.Remove(houseDecree);
                            dbContext.SaveChanges();
                        }
                    }

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
            List<HouseDecree> houseDecrees = dbContext.HouseDecree.Where(hd => hd.DecreeId == currentDecree.IdDecree).ToList();

            MessageBoxResult messageBoxResult = MessageBox.Show($"Удалить постановление?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        foreach (HouseDecree houseDecree in houseDecrees)
                        {
                            dbContext.HouseDecree.Remove(houseDecree);
                        }
                        dbContext.Decree.Remove(currentDecree);
                        dbContext.SaveChanges();

                        MessageBox.Show("Постановление удалено", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        MainFrameObj.mainFrame.Navigate(new DecreesViewPage());
                    }
                    catch
                    {
                        MessageBox.Show("Не получилось удалить постановление", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
    }
}