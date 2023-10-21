using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Реестр_маневренного_фонда.database.tables_classes;

namespace Реестр_маневренного_фонда.TablesManagersClasses
{
    public class AgreementManager
    {
        private List<string> addErrors(string number, TempResident tempResident, HousingFund housingFund, DateTime dateConclusion, DateTime dateEnd, DateTime? dateTermination, string? remark)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(number))
            {
                errors.Add("Неоходимо ввести номер");
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
                    errors.Add("Укажите номер договора корректно (только цифры)");
                }
            }


            return errors;
        }

        public void AddAgreement(string number, TempResident tempResident, HousingFund housingFund, DateTime dateConclusion, DateTime dateEnd, string? remark)
        {
            Agreement newAgreement = new Agreement();

            addErrors(number, tempResident, housingFund, dateConclusion, dateEnd, null, remark);
        }

        public void EditAgreement(Agreement currentAgreement, string number, TempResident tempResident, HousingFund housingFund, DateTime dateConclusion, DateTime dateEnd, DateTime? dateTermination, string? remark)
        {
            addErrors(number, tempResident, housingFund, dateConclusion, dateEnd, dateTermination, remark);
        }

        public void RemoveAgreement(Agreement currentAgreement)
        {

        }
    }
}
