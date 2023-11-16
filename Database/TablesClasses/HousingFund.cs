using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Media;
using System.Linq;
using Реестр_маневренного_фонда.database.tables_classes;
using System.Windows.Media;

namespace Реестр_маневренного_фонда
{
    public partial class HousingFund
    {
        [Key]
        public int IdHousingFund { get; set; }
        public int StreetId { get; set; }
        public string HouseNumber { get; set; }
        public int? ApartmentNumber { get; set; }
        public int? RoomNumber { get; set; }
        public int ImprovementDegreeId { get; set; }
        public double? DecreeArea { get; set; }
        public double? RegisterArea { get; set; }
        public string? Remark { get; set; }

        public virtual Street Street { get; set; }
        public virtual ImprovementDegree ImprovementDegree { get; set; }

        public string getFullAddress()
        {
            ApplicationContext.GetContext().Locality.Load();
            ApplicationContext.GetContext().Street.Load();
            string LocalityName = Street.Locality.NameLocality;
            string StreetName = Street.NameStreet;

            if (ApartmentNumber == null)
                return $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}";
            else if (RoomNumber == null)
                return $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}, кв. {ApartmentNumber}";
            else
                return $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}, кв. {ApartmentNumber}, ком. {RoomNumber}";
        }
        [NotMapped]
        public string FullAddress => getFullAddress();

        [NotMapped]
        public string Freedom
        {
            get
            {
                string freedomStatus = "Исключено";
                if (ApplicationContext.GetContext().ResidenceRegistration.Count(r => r.HousingFundId == IdHousingFund) > 0)
                {
                    ResidenceRegistration lastRegistration = ApplicationContext.GetContext().ResidenceRegistration.OrderBy(t => t.DateStartResidence).Last(r => r.HousingFundId == IdHousingFund);
                    Decree lastDecree = ApplicationContext.GetContext().Decree.OrderBy(t => t.DateDecree).Last(r => r.HousingFundId == IdHousingFund);
                    if (lastRegistration.DateEndResidence == null && lastDecree.Status == true)
                    {
                        freedomStatus = "Занято";
                    }
                    else if (lastRegistration.DateEndResidence != null && lastDecree.Status == true)
                    {
                        freedomStatus = "Свободно";
                    }
                }
                else if (ApplicationContext.GetContext().Decree.Count(r => r.HousingFundId == IdHousingFund) > 0)
                {
                    Decree lastDecree = ApplicationContext.GetContext().Decree.OrderBy(t => t.DateDecree).Last(r => r.HousingFundId == IdHousingFund);
                    if (lastDecree.Status == true)
                    {
                        freedomStatus = "Свободно";
                    }
                }
                else
                {
                    freedomStatus = string.Empty;
                }
                return freedomStatus;
            }
        }

        [NotMapped]
        public Brush BackColor
        {
            get
            {
                Brush color = Brushes.LightGray;
                if (ApplicationContext.GetContext().ResidenceRegistration.Count(r => r.HousingFundId == IdHousingFund) > 0)
                {
                    ResidenceRegistration lastRegistration = ApplicationContext.GetContext().ResidenceRegistration.OrderBy(t => t.DateStartResidence).Last(r => r.HousingFundId == IdHousingFund);
                    Decree lastDecree = ApplicationContext.GetContext().Decree.OrderBy(t => t.DateDecree).Last(r => r.HousingFundId == IdHousingFund);
                    if (lastRegistration.DateEndResidence == null && lastDecree.Status == true)
                    {
                        color = Brushes.PaleVioletRed;
                    }
                    else
                    {
                        color = Brushes.LightGreen;
                    }
                }
                else if (ApplicationContext.GetContext().Decree.Count(r => r.HousingFundId == IdHousingFund) > 0)
                {
                    Decree lastDecree = ApplicationContext.GetContext().Decree.OrderBy(t => t.DateDecree).Last(r => r.HousingFundId == IdHousingFund);
                    if (lastDecree.Status == true)
                    {
                        color = Brushes.LightGreen;
                    }
                }
                else
                {
                    color = Brushes.Transparent;
                }
                return color;
            }
        }
    }
}