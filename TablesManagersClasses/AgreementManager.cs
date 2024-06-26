﻿using System;
using System.Collections.Generic;
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
        private void showErrors(string? number, TempResident? tempResident, HousingFund? housingFund, DateTime? dateConclusion, DateTime? dateEnd, DateTime? dateTermination)
        {
            if (!string.IsNullOrWhiteSpace(number))
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
            if (dateConclusion != null && dateEnd != null && dateConclusion > dateEnd)
            {
                errors += ("Дата заключения не может быть позже даты окончания\n");
            }
            if (dateConclusion != null && dateTermination != null && dateConclusion > dateTermination)
            {
                errors += ("Дата заключения не может быть позже даты расторжения\n");
            }
        }

        public void AddAgreement(Agreement newAgreement, string? number, TempResident? tempResident, HousingFund? housingFund, DateTime? dateConclusion, DateTime? dateEnd, string? remark)
        {
            if (!string.IsNullOrWhiteSpace(number) && int.TryParse(number, out int value) && dbContext.Agreement.Count(a => a.NumberAgreement == Convert.ToInt32(number)) > 0)
            {
                errors += ("Договор с данным номером уже добавлен\n");
            }
            if ((tempResident != null && housingFund != null) && dbContext.Agreement.Count(a => a.TempResidentId == tempResident.IdTempResident && a.HousingFundId == housingFund.IdHousingFund && a.DateConclusionAgreement == dateConclusion) > 0)
            {
                errors += ("Договор с данным нанимателем, жильём и датой заключения уже добавлен\n");
            }

            showErrors(number, tempResident, housingFund, dateConclusion, dateEnd, null);
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(errors);
                errors = string.Empty;
            }
            else
            {
                try
                {
                    ResidenceRegistration? lastRegistration = null;
                    if (dbContext.ResidenceRegistration.Count(r => r.HousingFundId == housingFund.IdHousingFund) > 0)
                    {
                        lastRegistration = dbContext.ResidenceRegistration.OrderBy(t => t.DateStartResidence).Last(r => r.HousingFundId == housingFund.IdHousingFund);
                    }
                    if (lastRegistration != null && lastRegistration.DateEndResidence == null && lastRegistration.TempResidentId != tempResident.IdTempResident)
                    {
                        string messageBoxQuestionText = $"Для данного жилья на данный момент проживающим числится {lastRegistration.TempResident.FullName}. Поставить дату окончания его проживания в жилье и назначить сейчас проживающим {tempResident.FullName}?";
                        if (lastRegistration.DateStartResidence > dateConclusion)
                        {
                            messageBoxQuestionText = $"Для данного жилья на данный момент проживающим числится {lastRegistration.TempResident.FullName} и дата заключения поставлена раньше даты начала проживания текущего факта проживания. Добавить факт проживания вместе с датой окончания?";
                        }
                        MessageBoxResult boxResult = MessageBox.Show(messageBoxQuestionText, "", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        switch (boxResult)
                        {
                            case MessageBoxResult.Yes:
                                {
                                    if (!string.IsNullOrWhiteSpace(number))
                                    {
                                        newAgreement.NumberAgreement = Convert.ToInt32(number);
                                    }
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
                                        if (lastRegistration.DateEndResidence == null && lastRegistration.TempResidentId != tempResident.IdTempResident)
                                        {
                                            if (lastRegistration.DateStartResidence > dateConclusion && lastRegistration.DateStartResidence <= dateEnd)
                                                newRegistration.DateEndResidence = lastRegistration.DateStartResidence;
                                            else if (lastRegistration.DateStartResidence > dateConclusion && lastRegistration.DateStartResidence > dateEnd)
                                                newRegistration.DateEndResidence = dateEnd;
                                            else
                                                lastRegistration.DateEndResidence = DateTime.Now.Date;

                                            newRegistration.HousingFundId = housingFund.IdHousingFund;
                                            newRegistration.TempResidentId = tempResident.IdTempResident;
                                            newRegistration.DateStartResidence = (DateTime)dateConclusion;
                                            newRegistration.AgreementId = newAgreement.IdAgreement;

                                            dbContext.ResidenceRegistration.Add(newRegistration);
                                            dbContext.SaveChanges();
                                        }
                                        else if (lastRegistration.TempResidentId != tempResident.IdTempResident)
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

                                    break;
                                }
                            case MessageBoxResult.No:
                                MessageBox.Show("Необходимо выбрать другое жильё", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                break;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(number))
                        {
                            newAgreement.NumberAgreement = Convert.ToInt32(number);
                        }
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

                        if (lastRegistration == null || lastRegistration.DateEndResidence != null)
                        {
                            ResidenceRegistration newRegistration = new ResidenceRegistration();

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
                }
                catch
                {
                    MessageBox.Show("Не удалось добавить договор", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void EditAgreement(Agreement currentAgreement, string? number, TempResident? tempResident, HousingFund? housingFund, DateTime? dateConclusion, DateTime? dateEnd, DateTime? dateTermination, string? remark)
        {
            if (!string.IsNullOrWhiteSpace(number) && int.TryParse(number, out int value) && dbContext.Agreement.Count(a => a.IdAgreement != currentAgreement.IdAgreement && a.NumberAgreement == Convert.ToInt32(number)) > 0)
            {
                errors += ("Договор с данным номером уже добавлен\n");
            }
            if ((tempResident != null && housingFund != null) && dbContext.Agreement.Count(a => a.IdAgreement != currentAgreement.IdAgreement && a.TempResidentId == tempResident.IdTempResident && a.HousingFundId == housingFund.IdHousingFund && a.DateConclusionAgreement == dateConclusion) > 0)
            {
                errors += ("Договор с данным нанимателем, жильём и датой заключения уже добавлен\n");
            }

            showErrors(number, tempResident, housingFund, dateConclusion, dateEnd, dateTermination);
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

                        if (currentRegistration.DateStartResidence.Date.ToShortDateString() != dateConclusion.Value.Date.ToShortDateString())
                        {
                            MessageBoxResult messageBoxResult = MessageBox.Show($"Изменить дату начала проживания в соответствии с датой заключения договора?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
                        
                    }
                    if (!string.IsNullOrWhiteSpace(number))
                    {
                        currentAgreement.NumberAgreement = Convert.ToInt32(number);
                    }
                    else
                    {
                        currentAgreement.NumberAgreement = null;
                    }
                    currentAgreement.TempResidentId = tempResident.IdTempResident;
                    currentAgreement.HousingFundId = housingFund.IdHousingFund;
                    currentAgreement.DateConclusionAgreement = Convert.ToDateTime(dateConclusion);
                    currentAgreement.DateEndAgreement = Convert.ToDateTime(dateEnd);
                    currentAgreement.DateTerminationAgreement = dateTermination;

                    if (dateTermination != null)
                    {
                        List<Agreement> listAgreements = dbContext.Agreement.OrderBy(a => a.DateConclusionAgreement).OrderBy(a => a.HousingFundId).ToList();
                        List<ResidenceRegistration> listResidenceRegistration = dbContext.ResidenceRegistration.OrderBy(r => r.DateStartResidence).OrderBy(r => r.HousingFundId).ToList();
                        ResidenceRegistration lastRegistration = dbContext.ResidenceRegistration.OrderBy(t => t.DateStartResidence).Last(r => r.HousingFundId == housingFund.IdHousingFund);
                        if (listAgreements.SkipWhile(a => a.IdAgreement != currentAgreement.IdAgreement).Skip(1).FirstOrDefault() != null && listResidenceRegistration.SkipWhile(a => a.IdRegistration != lastRegistration.IdRegistration).Skip(1).FirstOrDefault() != null)
                        {
                            if (listResidenceRegistration.SkipWhile(a => a.IdRegistration != lastRegistration.IdRegistration).Skip(1).FirstOrDefault().AgreementId == listAgreements.SkipWhile(a => a.IdAgreement != currentAgreement.IdAgreement).Skip(1).FirstOrDefault().IdAgreement
                                && lastRegistration.DateEndResidence == null)
                            {
                                lastRegistration.DateEndResidence = currentAgreement.DateTerminationAgreement;
                                dbContext.ResidenceRegistration.Update(lastRegistration);
                            }
                        }
                        else if (lastRegistration.DateEndResidence == null && currentAgreement == listAgreements.Last() && lastRegistration == listResidenceRegistration.Last())
                        {
                            lastRegistration.DateEndResidence = currentAgreement.DateTerminationAgreement;
                            dbContext.ResidenceRegistration.Update(lastRegistration);
                        }
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
            MessageBoxResult messageBoxResult = MessageBox.Show($"Удалить договор?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        if (dbContext.ResidenceRegistration.Count(r => r.AgreementId == currentAgreement.IdAgreement) < 1)
                        {
                            foreach (Notification notification in dbContext.Notification.Where(n => n.AgreementId == currentAgreement.IdAgreement).ToList())
                            {
                                dbContext.Notification.Remove(notification);
                            }
                            dbContext.Agreement.Remove(currentAgreement);
                            dbContext.SaveChanges();

                            MessageBox.Show("Удаление прошло успешно", "", MessageBoxButton.OK, MessageBoxImage.Information);
                            MainFrameObj.mainFrame.Navigate(new AgreementsViewPage());
                        }
                        else
                        {
                            MessageBox.Show("Не получилось удалить договор", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Не получилось удалить договор", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
    }
}