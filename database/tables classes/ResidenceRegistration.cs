using System.ComponentModel.DataAnnotations;

namespace Реестр_маневренного_фонда
{
    public class ResidenceRegistration
    {
        [Key]
        public int IdRegistration { get; set; }
        public int HouseId { get; set; }
        public int TempResidentId { get; set; }
        public DateTime DateStartResidence { get; set; }
        public DateTime? DateEndResidence { get; set; }

        public TempResident TempResident { get; set; }
        public HousingFund HousingFund { get; set; }
    }
}
