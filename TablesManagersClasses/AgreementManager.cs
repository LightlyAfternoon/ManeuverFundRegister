using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Реестр_маневренного_фонда.database.tables_classes;

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
                    errors += ("Укажите номер договора корректно (только цифры)");
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

            MessageBox.Show(errors);
            errors = string.Empty;
        }

        public void AddAgreement(string? number, TempResident? tempResident, HousingFund? housingFund, DateTime? dateConclusion, DateTime? dateEnd, string? remark)
        {
            Agreement newAgreement = new Agreement();

            if (!string.IsNullOrEmpty(errors))
            {
                showErrors(number, tempResident, housingFund, dateConclusion, dateEnd);
            }
            else
            {
                newAgreement.NumberAgreement = Convert.ToInt32(number);
                newAgreement.TempResident = tempResident;
                newAgreement.HousingFund = housingFund;
                newAgreement.DateConclusionAgreement = Convert.ToDateTime(dateConclusion);
                newAgreement.DateEndAgreement = Convert.ToDateTime(dateEnd);
                if (!string.IsNullOrWhiteSpace(remark))
                {
                    newAgreement.Remark = remark;
                }

                dbContext.Agreement.Add(newAgreement);
                dbContext.SaveChanges();
            }
        }

        public void EditAgreement(Agreement currentAgreement, string? number, TempResident? tempResident, HousingFund? housingFund, DateTime? dateConclusion, DateTime? dateEnd, DateTime? dateTermination, string? remark)
        {
            if (!string.IsNullOrEmpty(errors))
            {
                showErrors(number, tempResident, housingFund, dateConclusion, dateEnd);
            }
            else
            {
                currentAgreement.NumberAgreement = Convert.ToInt32(number);
                currentAgreement.TempResident = tempResident;
                currentAgreement.HousingFund = housingFund;
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
            }
        }

        public void RemoveAgreement(Agreement currentAgreement)
        {
            try
            {
                dbContext.Agreement.Remove(currentAgreement);
                dbContext.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Не получилось удалить договор", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
