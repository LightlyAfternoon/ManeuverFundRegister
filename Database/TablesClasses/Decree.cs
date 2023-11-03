using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Реестр_маневренного_фонда
{
    public partial class Decree
    {
        [Key]
        public int IdDecree { get; set; }
        public int NumberDecree { get; set; }
        public DateTime DateDecree { get; set; }
        public int HousingFundId { get; set; }
        public bool Status { get; set; }
        public byte[] File { get; set; }

        public virtual HousingFund HousingFund { get; set; }

        public string getStatusName()
        {
            if (Status == false)
                return "Исключение из фонда";
            else
                return "Включение в фонд";
        }

        [NotMapped]
        public string StatusName => getStatusName();
    }
}
