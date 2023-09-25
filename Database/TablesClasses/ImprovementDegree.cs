using System.ComponentModel.DataAnnotations;

namespace Реестр_маневренного_фонда
{
    public class ImprovementDegree
    {
        [Key]
        public int IdImprovementDegree { get; set; }
        public string NameImprovementDegree { get; set; }
    }
}
