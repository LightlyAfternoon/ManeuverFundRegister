using System;
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
                errors += ("Необходимо выбрать дату окончания договора");
            }
            if (dbContext.Agreement.Count(a => a.TempResidentId == tempResident.IdTempResident && a.HousingFundId == housingFund.IdHousingFund && string.Format("{0:dd.MM.yyyy}", a.DateConclusion) == string.Format("{0:dd.MM.yyyy}", dateConclusion)) > 0)
            {
                errors += ("Договор с данным нанимателем, жильём и датой заключения уже добавлен");
            }
        }

        public void AddAgreement(string? number, TempResident? tempResident, HousingFund? housingFund, DateTime? dateConclusion, DateTime? dateEnd, string? remark)
        {
            Agreement newAgreement = new Agreement();
            ResidenceRegistration newRegistration = new ResidenceRegistration();

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

                    newRegistration.HousingFundId = housingFund.IdHousingFund;
                    newRegistration.TempResidentId = tempResident.IdTempResident;
                    newRegistration.DateStartResidence = (DateTime)dateConclusion;

                    dbContext.Agreement.Add(newAgreement);
                    dbContext.ResidenceRegistration.Add(newRegistration);
                    dbContext.SaveChanges();

                    MessageBox.Show("Договор и факт начала проживания в жилье добавлен", "", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    ResidenceRegistration currentRegistration = dbContext.ResidenceRegistration.First(r => r.TempResidentId == tempResident.IdTempResident && r.HousingFundId == housingFund.IdHousingFund && string.Format("{0:dd.MM.yyyy}", r.DateStartResidence) == string.Format("{0:dd.MM.yyyy}", dateConclusion));
                    
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

                    currentRegistration.DateStartResidence = Convert.ToDateTime(dateConclusion);

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
                dbContext.Agreement.Remove(currentAgreement);
                dbContext.SaveChanges();

                MessageBox.Show("Договор удалён", "", MessageBoxButton.OK, MessageBoxImage.Information);
                MainFrameObj.mainFrame.Navigate(new AgreementsViewPage());
            }
            catch
            {
                MessageBox.Show("Не получилось удалить договор", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
