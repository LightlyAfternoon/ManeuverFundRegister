using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Реестр_маневренного_фонда.database.tables_classes
{
    public partial class TempResident
    {
        [Key]
        public int IdTempResident { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string? Patronymic { get; set; }
        public string? Remark { get; set; }

        [NotMapped]
        public string FullName => $"{LastName} {FirstName} {Patronymic}";
    }
}