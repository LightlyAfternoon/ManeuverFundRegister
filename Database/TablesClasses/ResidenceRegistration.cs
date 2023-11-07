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
    }
}