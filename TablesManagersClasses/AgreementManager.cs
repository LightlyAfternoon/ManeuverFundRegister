using System;
using System.Linq;
using System.Windows;
using Реестр_маневренного_фонда.database.tables_classes;
using Реестр_маневренного_фонда.Pages;

namespace Реестр_маневренного_фонда.TablesManagersClasses
{
    public class AgreementManager
    {
        private ApplicationContext dbContext = ApplicationContext.GetContext();

        private string errors = string.Empty;
        private void showErrors(string? number, TempResident? tempResident, HousingFund? housingFund, DateTime? dateConclusion, DateTime? dateEnd)
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
                    errors += ("Укажите номер договора корректно (только цифры)\n");
                }
            }
            if (tempResident == null) 
            {
                errors += ("Необходимо выбрать нанимателя\n");
            }
            if (housingFund == null)
            {
                errors += ("Необходимо выбрать жильё\n");
            }
            if (dateConclusion == null)
            {
                errors += ("Необходимо выбрать дату заключения договора\n");
            }
            if (dateEnd == null)
            {
                errors += ("Необходимо выбрать дату окончания договора\n");
            }
            if ((tempResident != null && housingFund != null) && dbContext.Agreement.Count(a => a.TempResidentId == tempResident.IdTempResident && a.HousingFundId == housingFund.IdHousingFund && a.DateConclusionAgreement == dateConclusion) > 1)
            {
                errors += ("Договор с данным нанимателем, жильём и датой заключения уже добавлен\n");
            }
        }

        public void AddAgreement(Agreement newAgreement, string? number, TempResident? tempResident, HousingFund? housingFund, DateTime? dateConclusion, DateTime? dateEnd, string? remark)
        {

            showErrors(number, tempResident, housingFund, dateConclusion, dateEnd);
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(errors);
                errors = string.Empty;
            }
            else
            {
                try
                {
                    newAgreement.NumberAgreement = Convert.ToInt32(number);
                    newAgreement.TempResidentId = tempResident.IdTempResident;
                    newAgreement.HousingFundId = housingFund.IdHousingFund;
                    newAgreement.DateConclusionAgreement = Convert.ToDateTime(dateConclusion);
                    newAgreement.DateEndAgreement = Convert.ToDateTime(dateEnd);
                    if (!string.IsNullOrWhiteSpace(remark))
                    {
                        newAgreement.Remark = remark;
                    }

                    dbContext.Agreement.Add(newAgreement);
                    dbContext.SaveChanges();


                    ResidenceRegistration newRegistration = new ResidenceRegistration();
                    if (dbContext.ResidenceRegistration.Count(r => r.HousingFundId == housingFund.IdHousingFund) > 0)
                    {
                        ResidenceRegistration lastRegistration = dbContext.ResidenceRegistration.OrderBy(t => t.DateStartResidence).Last(r => r.HousingFundId == housingFund.IdHousingFund);
                        if (lastRegistration.DateEndResidence == null)
                        {
                            if (lastRegistration.TempResidentId != tempResident.IdTempResident)
                            {
                                MessageBoxResult boxResult = MessageBox.Show($"Для данного жилья на данный момент проживающим числится {lastRegistration.TempResident.FullName}. Поставить дату окончания его проживания в жилье и назначить сейчас проживающим {tempResident.FullName}?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

                                switch (boxResult)
                                {
                                    case MessageBoxResult.Yes:

                                        newRegistration.HousingFundId = housingFund.IdHousingFund;
                                        newRegistration.TempResidentId = tempResident.IdTempResident;
                                        newRegistration.DateStartResidence = (DateTime)dateConclusion;
                                        newRegistration.AgreementId = newAgreement.IdAgreement;

                                        lastRegistration.DateEndResidence = newRegistration.DateStartResidence;

                                        dbContext.ResidenceRegistration.Add(newRegistration);
                                        dbContext.ResidenceRegistration.Update(lastRegistration);
                                        dbContext.SaveChanges();
                                        break;
                                    case MessageBoxResult.No:
                                        MessageBox.Show("Необходимо выбрать другое жильё", "", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                                        MainFrameObj.mainFrame.Navigate(new AgreementsViewPage());
                                        break;
                                }
                            }
                        }
                        else
                        {
                            newRegistration.HousingFundId = housingFund.IdHousingFund;
                            newRegistration.TempResidentId = tempResident.IdTempResident;
                            newRegistration.DateStartResidence = (DateTime)dateConclusion;
                            newRegistration.AgreementId = newAgreement.IdAgreement;
    
                            dbContext.ResidenceRegistration.Add(newRegistration);
                            dbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        newRegistration.HousingFundId = housingFund.IdHousingFund;
                        newRegistration.TempResidentId = tempResident.IdTempResident;
                        newRegistration.DateStartResidence = (DateTime)dateConclusion;
                        newRegistration.AgreementId = newAgreement.IdAgreement;

                        dbContext.ResidenceRegistration.Add(newRegistration);
                        dbContext.SaveChanges();
                    }


                    MessageBox.Show("Договор добавлен", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainFrameObj.mainFrame.Navigate(new AgreementsViewPage());
                }
                catch
                {
                    MessageBox.Show("Не удалось добавить договор", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void EditAgreement(Agreement currentAgreement, string? number, TempResident? tempResident, HousingFund? housingFund, DateTime? dateConclusion, DateTime? dateEnd, DateTime? dateTermination, string? remark)
        {
            showErrors(number, tempResident, housingFund, dateConclusion, dateEnd);
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(errors);
                errors = string.Empty;
            }
            else
            {
                try
                {
                    if (dbContext.ResidenceRegistration.Count(r => r.AgreementId == currentAgreement.IdAgreement) > 0)
                    {
                        ResidenceRegistration currentRegistration = dbContext.ResidenceRegistration.First(r => r.AgreementId == currentAgreement.IdAgreement);

                        MessageBoxResult messageBoxResult = MessageBox.Show($"Изменить дату начала проживания в соответсвии с датой заключения договора?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        switch (messageBoxResult)
                        {
                            case MessageBoxResult.Yes:
                                currentRegistration.DateStartResidence = Convert.ToDateTime(dateConclusion);
                                dbContext.ResidenceRegistration.Update(currentRegistration);
                                break;
                            case MessageBoxResult.No:
                                break;
                        }
                    }

                    currentAgreement.NumberAgreement = Convert.ToInt32(number);
                    currentAgreement.TempResidentId = tempResident.IdTempResident;
                    currentAgreement.HousingFundId = housingFund.IdHousingFund;
                    currentAgreement.DateConclusionAgreement = Convert.ToDateTime(dateConclusion);
                    currentAgreement.DateEndAgreement = Convert.ToDateTime(dateEnd);
                    if (dateTermination != null)
                    {
                        currentAgreement.DateTerminationAgreement = dateTermination;
                    }
                    if (!string.IsNullOrWhiteSpace(remark))
                    {
                        currentAgreement.Remark = remark;
                    }

                    dbContext.Agreement.Update(currentAgreement);
                    dbContext.SaveChanges();

                    MessageBox.Show("Договор изменён", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainFrameObj.mainFrame.Navigate(new AgreementsViewPage());
                }
                catch
                {
                    MessageBox.Show("Не удалось изменить договор", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void RemoveAgreement(Agreement currentAgreement)
        {
            try
            {
                MessageBoxResult messageBoxResult = MessageBox.Show($"Удалить договор?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes:
                        foreach (Notification notification in dbContext.Notification.Where(n => n.AgreementId == currentAgreement.IdAgreement).ToList())
                        {
                            dbContext.Notification.Remove(notification);
                        }
                        dbContext.Agreement.Remove(currentAgreement);
                        dbContext.SaveChanges();

                        MessageBox.Show("Удаление прошло успешно", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        MainFrameObj.mainFrame.Navigate(new AgreementsViewPage());
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            catch
            {
                MessageBox.Show("Не получилось удалить договор", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}