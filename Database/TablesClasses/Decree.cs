using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Windows;
using Реестр_маневренного_фонда.Database.TablesClasses;

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
        public byte[]? File { get; set; }

        public string getStatusName()
        {
            if (Status == false)
                return "Исключение из фонда";
            else
                return "Включение в фонд";
        }

        [NotMapped]
        public string StatusName => getStatusName();
        public string getAllHousingFundAddresses()
        {
            string addresses = string.Empty;
            List<HouseDecree> houseDecrees = ApplicationContext.GetContext().HouseDecree.Where(hd => hd.DecreeId == IdDecree).ToList();
            foreach (HouseDecree houseDecree in houseDecrees)
            {
                addresses += $"{houseDecree.HousingFund.FullAddress}\n";
            }
            return addresses;
        }

        [NotMapped]
        public string AllHousingFundAddresses => getAllHousingFundAddresses();

        [NotMapped]
        public Visibility ShowButton 
        {
            get
            {
                bool existsFile;
                if (File != null)
                    existsFile = true;
                else
                    existsFile = false;
                return (existsFile ? Visibility.Visible : Visibility.Collapsed);
            }
        }
    }
}