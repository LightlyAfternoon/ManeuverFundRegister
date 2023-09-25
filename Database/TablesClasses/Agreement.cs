using System;
using System.ComponentModel.DataAnnotations;
using Реестр_маневренного_фонда.database.tables_classes;

namespace Реестр_маневренного_фонда
{
    public partial class Agreement
    {
        [Key]
        public int IdAgreement { get; set; }
        public int NumberAgreement { get; set; }
        public int TempResidentId { get; set; }
        public int HousingFundId { get; set; }
        public DateTime DateConclusionAgreement { get; set; }
        public DateTime DateEndAgreement { get; set; }
        public DateTime? DateTerminationAgreement { get; set; }
        public string? Remark { get; set; }

        public virtual TempResident TempResident { get; set; }
        public virtual HousingFund HousingFund { get; set; }
    }
}
