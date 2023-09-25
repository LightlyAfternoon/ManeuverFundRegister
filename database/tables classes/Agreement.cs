using System.ComponentModel.DataAnnotations;

namespace Реестр_маневренного_фонда
{
    public class Agreement
    {
        [Key]
        public int IdAgreement { get; set; }
        public int NumberAgreement { get; set; }
        public int TempResidentId { get; set; }
        public int HouseId { get; set; }
        public DateTime DateConclusionAgreement { get; set; }
        public DateTime DateEndAgreement { get; set; }
        public DateTime? DateTerminationAgreement { get; set; }
        public string Remark { get; set; }

        public TempResident TempResident { get; set; }
        public HousingFund HousingFund { get; set; }
    }
}
