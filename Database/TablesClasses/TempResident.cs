using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private string _fullName = "tt";
        [NotMapped]
        public string FullName => $"{LastName} {FirstName} {Patronymic}";
    }
}
