using System.ComponentModel.DataAnnotations;

namespace Реестр_маневренного_фонда
{
    public class ResidenceRegistration
    {
        [Key]
        public int IdRegistration { get; set; }
    }
}