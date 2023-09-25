using System.ComponentModel.DataAnnotations;

namespace Реестр_маневренного_фонда.database.tables_classes
{
    public partial class Locality
    {
        [Key]
        public int IdLocality { get; set; }
        public string NameLocality { get; set; }
    }
}
