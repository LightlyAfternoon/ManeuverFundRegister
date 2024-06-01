using System.ComponentModel.DataAnnotations;

namespace Реестр_маневренного_фонда
{
    public partial class ImprovementDegree
    {
        [Key]
        public int IdImprovementDegree { get; set; }
        public string NameImprovementDegree { get; set; }
    }
}