using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        private string fullName;
        public string FullName
        {
            get => fullName;
            set
            {
                if (!string.IsNullOrWhiteSpace(Patronymic))
                    fullName = $"{LastName} {FirstName} {Patronymic}";
                else
                    fullName = $"{LastName} {FirstName}";
            }
        }
    }
}
