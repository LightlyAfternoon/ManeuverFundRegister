using System.ComponentModel.DataAnnotations;

namespace Реестр_маневренного_фонда.Database.TablesClasses
{
    public partial class HouseDecree
    {
        [Key]
        public int IdHouseDecree { get; set; }
        public int DecreeId { get; set; }
        public int HousingFundId { get; set; }

        public virtual Decree Decree { get; set; }
        public virtual HousingFund HousingFund { get; set; }
    }
}