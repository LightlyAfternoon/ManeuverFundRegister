using System;
using System.ComponentModel.DataAnnotations;

namespace Реестр_маневренного_фонда
{
    public partial class Decree
    {
        [Key]
        public int IdDecree { get; set; }
        public int NumberDecree { get; set; }
        public DateTime DateDecree { get; set; }
        public int HouseId { get; set; }
        public bool Status { get; set; }

        public virtual HousingFund HousingFund { get; set; }
    }
}
