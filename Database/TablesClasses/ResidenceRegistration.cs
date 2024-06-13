using System;
using System.ComponentModel.DataAnnotations;
using Реестр_маневренного_фонда.database.tables_classes;

namespace Реестр_маневренного_фонда
{
    public partial class ResidenceRegistration
    {
        [Key]
        public int IdRegistration { get; set; }
        public int HousingFundId { get; set; }
        public int TempResidentId { get; set; }
        public DateTime DateStartResidence { get; set; }
        public DateTime? DateEndResidence { get; set; }
        public int AgreementId { get; set; }

        public virtual TempResident TempResident { get; set; }
        public virtual HousingFund HousingFund { get; set; }
        public virtual Agreement Agreement { get; set; }

        public int[] getStartYear()
        {
            int dd;
            if (DateEndResidence != null)
                dd = DateEndResidence.Value.Year - DateStartResidence.Year + 1;
            else
                dd = DateTime.Now.Year - DateStartResidence.Year + 1;

            int[] years = new int[dd];
            int thisStartYear = DateStartResidence.Year;

            for (int i = 0; i < dd; i++)
            {
                years[i] = thisStartYear;
                thisStartYear++;
            }

            return years;
        }

        public int[] StartYear => getStartYear();
    }
}