using System.ComponentModel.DataAnnotations;

namespace Реестр_маневренного_фонда
{
    public class Agreement
    {
        [Key]
        public int IdAgreement { get; set; }
        public int NumberAgreement { get; set; }
        public int TempResidentId { get; set; }

    }
}