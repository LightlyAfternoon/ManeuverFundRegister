using System;
using System.Windows;
using Реестр_маневренного_фонда.database.tables_classes;
using Реестр_маневренного_фонда.Pages.HousingsFund;

namespace Реестр_маневренного_фонда.TablesManagersClasses
{
    public class HousingFundManager
    {
        private ApplicationContext dbContext = ApplicationContext.GetContext();

        private string errors = string.Empty;

        private void showErrors(Locality? locality, string? houseNumber, string? entranceNumber, string? floorNumber, string? apartmentNumber, string? roomNumber, string? decreeArea, string? registerArea)
        {
            if (locality == null)
            {
                errors += ("Необходимо выбрать населённый пункт\n");
            }
            if (string.IsNullOrWhiteSpace(houseNumber))
            {
                errors += ("Необходимо ввести номер дома\n");
            }
            if (!string.IsNullOrWhiteSpace(entranceNumber))
            {
                try
                {
                    checked
                    {
                        Convert.ToInt32(entranceNumber);
                    }
                }
                catch
                {
                    errors += ("Укажите номер подъезда корректно (только цифры)\n");
                }
            }
            if (!string.IsNullOrWhiteSpace(floorNumber))
            {
                try
                {
                    checked
                    {
                        Convert.ToInt32(floorNumber);
                    }
                }
                catch
                {
                    errors += ("Укажите номер этажа корректно (только цифры)\n");
                }
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
                    errors += ("Укажите номер квартиры корректно (только цифры)\n");
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
                    errors += ("Укажите номер комнаты корректно (только цифры)\n");
                }
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
                    errors += ("Укажите площадь по постановлению корректно\n");
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
        }
        
        public void AddHouseInFund(Locality? locality, Street? street, string? houseNumber, string? entranceNumber, string? floorNumber, string? apartmentNumber, 
                                   string? roomNumber, string? decreeArea, string? registerArea, string? remark, bool? CWS, bool? HWS, bool? CG, 
                                   bool? BG, bool? SH, bool? CH, bool? Electricity, bool? KeysAvailability)
        {
            HousingFund newHouseInFund = new HousingFund();

            showErrors(locality, houseNumber, entranceNumber, floorNumber, apartmentNumber, roomNumber, decreeArea, registerArea);
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(errors);
                errors = string.Empty;
            }
            else
            {
                try
                {
                    newHouseInFund.LocalityId = locality.IdLocality;
                    if (street != null)
                    {
                        newHouseInFund.StreetId = street.IdStreet;
                    }
                    newHouseInFund.HouseNumber = houseNumber;
                    if (!string.IsNullOrWhiteSpace(entranceNumber))
                    {
                        newHouseInFund.EntranceNumber = Convert.ToInt32(entranceNumber);
                    }
                    if (!string.IsNullOrWhiteSpace(floorNumber))
                    {
                        newHouseInFund.FloorNumber = Convert.ToInt32(floorNumber);
                    }
                    if (!string.IsNullOrWhiteSpace(apartmentNumber))
                    {
                        newHouseInFund.ApartmentNumber = Convert.ToInt32(apartmentNumber);
                    }
                    if (!string.IsNullOrWhiteSpace(roomNumber))
                    {
                        newHouseInFund.RoomNumber = Convert.ToInt32(roomNumber);
                    }
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
                    if (CWS == true)
                    {
                        newHouseInFund.CWS = true;
                    }
                    else
                    {
                        newHouseInFund.CWS = false;
                    }
                    if (HWS == true)
                    {
                        newHouseInFund.HWS = true;
                    }
                    else
                    {
                        newHouseInFund.HWS = false;
                    }
                    if (CG == true)
                    {
                        newHouseInFund.CG = true;
                    }
                    else
                    {
                        newHouseInFund.CG = false;
                    }
                    if (BG == true)
                    {
                        newHouseInFund.BG = true;
                    }
                    else
                    {
                        newHouseInFund.BG = false;
                    }
                    if (SH == true)
                    {
                        newHouseInFund.SH = true;
                    }
                    else
                    {
                        newHouseInFund.SH = false;
                    }
                    if (CH == true)
                    {
                        newHouseInFund.CH = true;
                    }
                    else
                    {
                        newHouseInFund.CH = false;
                    }
                    if (Electricity == true)
                    {
                        newHouseInFund.Electricity = true;
                    }
                    else
                    {
                        newHouseInFund.Electricity = false;
                    }
                    if (KeysAvailability == true)
                    {
                        newHouseInFund.KeysAvailability = true;
                    }
                    else
                    {
                        newHouseInFund.KeysAvailability = false;
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

        public void EditHouseInFund(HousingFund? currentHouseInFund, Locality? locality, Street? street, string? houseNumber, string? entranceNumber, string? floorNumber, 
                                    string? apartmentNumber, string? roomNumber, string? decreeArea, string? registerArea, string? remark,
                                    bool? CWS, bool? HWS, bool? CG, bool? BG, bool? SH, bool? CH, bool? Electricity, bool? KeysAvailability)
        {
            showErrors(locality, houseNumber, entranceNumber, floorNumber, apartmentNumber, roomNumber, decreeArea, registerArea);
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(errors);
                errors = string.Empty;
            }
            else
            {
                try
                {
                    currentHouseInFund.LocalityId = locality.IdLocality;
                    if (street != null)
                    {
                        currentHouseInFund.StreetId = street.IdStreet;
                    }
                    currentHouseInFund.HouseNumber = houseNumber;
                    if (!string.IsNullOrWhiteSpace(entranceNumber))
                    {
                        currentHouseInFund.EntranceNumber = Convert.ToInt32(entranceNumber);
                    }
                    if (!string.IsNullOrWhiteSpace(floorNumber))
                    {
                        currentHouseInFund.FloorNumber = Convert.ToInt32(floorNumber);
                    }
                    if (!string.IsNullOrWhiteSpace(apartmentNumber))
                    {
                        currentHouseInFund.ApartmentNumber = Convert.ToInt32(apartmentNumber);
                    }
                    if (!string.IsNullOrWhiteSpace(roomNumber))
                    {
                        currentHouseInFund.RoomNumber = Convert.ToInt32(roomNumber);
                    }
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
                    if (CWS == true)
                    {
                        currentHouseInFund.CWS = true;
                    }
                    else
                    {
                        currentHouseInFund.CWS = false;
                    }
                    if (HWS == true)
                    {
                        currentHouseInFund.HWS = true;
                    }
                    else
                    {
                        currentHouseInFund.HWS = false;
                    }
                    if (CG == true)
                    {
                        currentHouseInFund.CG = true;
                    }
                    else
                    {
                        currentHouseInFund.CG = false;
                    }
                    if (BG == true)
                    {
                        currentHouseInFund.BG = true;
                    }
                    else
                    {
                        currentHouseInFund.BG = false;
                    }
                    if (SH == true)
                    {
                        currentHouseInFund.SH = true;
                    }
                    else
                    {
                        currentHouseInFund.SH = false;
                    }
                    if (CH == true)
                    {
                        currentHouseInFund.CH = true;
                    }
                    else
                    {
                        currentHouseInFund.CH = false;
                    }
                    if (Electricity == true)
                    {
                        currentHouseInFund.Electricity = true;
                    }
                    else
                    {
                        currentHouseInFund.Electricity = false;
                    }
                    if (KeysAvailability == true)
                    {
                        currentHouseInFund.KeysAvailability = true;
                    }
                    else
                    {
                        currentHouseInFund.KeysAvailability = false;
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
                MessageBoxResult messageBoxResult = MessageBox.Show($"Удалить нанимателя?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes:
                        dbContext.HousingFund.Remove(currentHouseInFund);
                        dbContext.SaveChanges();
                
                        MessageBox.Show("Жильё удалено", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        MainFrameObj.mainFrame.Navigate(new HousingFundViewPage());
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            catch
            {
                MessageBox.Show("Не удалось удалить жильё", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}