using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Реестр_маневренного_фонда.database.tables_classes;
using Реестр_маневренного_фонда.Pages;

namespace Реестр_маневренного_фонда.TablesManagersClasses
{
    public class HousingFundManager
    {
        private ApplicationContext dbContext = ApplicationContext.GetContext();

        private string errors = string.Empty;

        private void showErrors(Street? street, string? houseNumber, string? apartmentNumber, string? roomNumber, ImprovementDegree? improvementDegree, string? decreeArea, string? registerArea)
        {
            if (street == null)
            {
                errors += ("Необходимо выбрать адрес жилья\n");
            }
            if (string.IsNullOrWhiteSpace(houseNumber))
            {
                errors += ("Неоходимо ввести номер дома\n");
            }
            if (!string.IsNullOrWhiteSpace(apartmentNumber))
            {
                try
                {
                    checked
                    {
                        Convert.ToInt32(apartmentNumber);
                    }
                }
                catch
                {
                    errors += ("Укажите номер квартиры корректно (только цифры)");
                }
            }
            if (!string.IsNullOrWhiteSpace(roomNumber))
            {
                try
                {
                    checked
                    {
                        Convert.ToInt32(roomNumber);
                    }
                }
                catch
                {
                    errors += ("Укажите номер комнаты корректно (только цифры)");
                }
            }
            if (improvementDegree == null)
            {
                errors += ("Необходимо выбрать степень благоустроенности\n");
            }
            if (!string.IsNullOrWhiteSpace(decreeArea))
            {
                try
                {
                    checked
                    {
                        Convert.ToDouble(decreeArea);
                    }
                }
                catch
                {
                    errors += ("Укажите площадь по постановлению корректно");
                }
            }
            if (!string.IsNullOrWhiteSpace(registerArea))
            {
                try
                {
                    checked
                    {
                        Convert.ToDouble(registerArea);
                    }
                }
                catch
                {
                    errors += ("Укажите площадь по реестру корректно");
                }
            }

            MessageBox.Show(errors);
            errors = string.Empty;
        }
        
        public void AddHouseInFund(Street? street, string? houseNumber, string? apartmentNumber, string? roomNumber, ImprovementDegree? improvementDegree, string? decreeArea, string? registerArea, string? remark)
        {
            HousingFund newHouseInFund = new HousingFund();

            showErrors(street, houseNumber, apartmentNumber, roomNumber, improvementDegree, decreeArea, registerArea);
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(errors);
                errors = string.Empty;
            }
            else
            {
                try
                {
                    newHouseInFund.StreetId = street.IdStreet;
                    newHouseInFund.HouseNumber = houseNumber;
                    if (!string.IsNullOrWhiteSpace(apartmentNumber))
                    {
                        newHouseInFund.ApartmentNumber = Convert.ToInt32(apartmentNumber);
                    }
                    if (!string.IsNullOrWhiteSpace(roomNumber))
                    {
                        newHouseInFund.RoomNumber = Convert.ToInt32(roomNumber);
                    }
                    newHouseInFund.ImprovementDegreeId = improvementDegree.IdImprovementDegree;
                    if (!string.IsNullOrWhiteSpace(decreeArea))
                    {
                        newHouseInFund.DecreeArea = Convert.ToDouble(decreeArea);
                    }
                    if (!string.IsNullOrWhiteSpace(registerArea))
                    {
                        newHouseInFund.RegisterArea = Convert.ToDouble(registerArea);
                    }
                    if (!string.IsNullOrWhiteSpace(remark))
                    {
                        newHouseInFund.Remark = remark;
                    }

                    dbContext.HousingFund.Add(newHouseInFund);
                    dbContext.SaveChanges();

                    MessageBox.Show("Жильё добавлено", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainFrameObj.mainFrame.Navigate(new HousingFundViewPage());
                }
                catch
                {
                    MessageBox.Show("Не удалось добавить жильё", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void EditHouseInFund(HousingFund currentHouseInFund, Street? street, string? houseNumber, string? apartmentNumber, string? roomNumber, ImprovementDegree? improvementDegree, string? decreeArea, string? registerArea, string? remark)
        {
            showErrors(street, houseNumber, apartmentNumber, roomNumber, improvementDegree, decreeArea, registerArea);
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(errors);
                errors = string.Empty;
            }
            else
            {
                try
                {
                    currentHouseInFund.StreetId = street.IdStreet;
                    currentHouseInFund.HouseNumber = houseNumber;
                    if (!string.IsNullOrWhiteSpace(apartmentNumber))
                    {
                        currentHouseInFund.ApartmentNumber = Convert.ToInt32(apartmentNumber);
                    }
                    if (!string.IsNullOrWhiteSpace(roomNumber))
                    {
                        currentHouseInFund.RoomNumber = Convert.ToInt32(roomNumber);
                    }
                    newHouseInFund.ImprovementDegreeId = improvementDegree.IdImprovementDegree;
                    if (!string.IsNullOrWhiteSpace(decreeArea))
                    {
                        currentHouseInFund.DecreeArea = Convert.ToDouble(decreeArea);
                    }
                    if (!string.IsNullOrWhiteSpace(registerArea))
                    {
                        currentHouseInFund.RegisterArea = Convert.ToDouble(registerArea);
                    }
                    if (!string.IsNullOrWhiteSpace(remark))
                    {
                        currentHouseInFund.Remark = remark;
                    }

                    dbContext.HousingFund.Update(currentHouseInFund);
                    dbContext.SaveChanges();

                    MessageBox.Show("Данные жилья изменены", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainFrameObj.mainFrame.Navigate(new HousingFundViewPage());
                }
                catch
                {
                    MessageBox.Show("Не удалось изменить данные жилья", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void RemoveHouseInFund(HousingFund currentHouseInFund)
        {
            try
            {
                dbContext.HousingFund.Remove(currentHouseInFund);
                dbContext.SaveChanges();
                
                MessageBox.Show("Жильё удалено", "", MessageBoxButton.OK, MessageBoxImage.Information);
                MainFrameObj.mainFrame.Navigate(new HousingFundViewPage());
            }
            catch
            {
                MessageBox.Show("Не удалось удалить жильё", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
