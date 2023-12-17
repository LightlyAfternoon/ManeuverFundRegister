using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Реестр_маневренного_фонда.database.tables_classes;
using System.Windows.Media;
using Реестр_маневренного_фонда.Database.TablesClasses;

namespace Реестр_маневренного_фонда
{
    public partial class HousingFund
    {
        [Key]
        public int IdHousingFund { get; set; }
        public int? LocalityId { get; set; }
        public int? StreetId { get; set; }
        public int? ImprovementDegreeId { get; set; }
        public string HouseNumber { get; set; }
        public int? ApartmentNumber { get; set; }
        public int? EntranceNumber { get; set; }
        public int? FloorNumber { get; set; }
        public string? RoomNumber { get; set; }
        public double? DecreeArea { get; set; }
        public double? RegisterArea { get; set; }
        public string? Remark { get; set; }
        public bool? CWS { get; set; }
        public bool? HWS { get; set; }
        public bool? CG { get; set; }
        public bool? BG { get; set; }
        public bool? SH { get; set; }
        public bool? CH { get; set; }
        public bool? Electricity { get; set; }
        public bool? KeysAvailability { get; set; }
        public bool? Sewerage { get; set; }

        public virtual ImprovementDegree ImprovementDegree { get; set; }
        public virtual Locality Locality { get; set; }
        public virtual Street Street { get; set; }

        public string getFullAddress()
        {
            string StreetName = string.Empty;
            ApplicationContext.GetContext().Locality.Load();
            ApplicationContext.GetContext().Street.Load();
            string LocalityName = Locality.NameLocality;
            if (StreetId != null)
            {
                StreetName = Street.NameStreet;
            }

            if (StreetName != string.Empty)
            {
                if (ApartmentNumber == null)
                    return $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}";
                else if (RoomNumber == null)
                    return $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}, кв. {ApartmentNumber}";
                else if (EntranceNumber == null)
                    return $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}, кв. {ApartmentNumber}, ком. {RoomNumber}";
                else if (FloorNumber == null)
                    return $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}, кв. {ApartmentNumber}, ком. {RoomNumber}, подъезд {EntranceNumber}";
                else
                    return $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}, кв. {ApartmentNumber}, ком. {RoomNumber}, подъезд {EntranceNumber}, этаж {FloorNumber}";
            }
            else
            {
                if (ApartmentNumber == null)
                    return $"{LocalityName}, д. {HouseNumber}";
                else if (RoomNumber == null)
                    return $"{LocalityName}, д. {HouseNumber}, кв. {ApartmentNumber}";
                else if (EntranceNumber == null)
                    return $"{LocalityName}, д. {HouseNumber}, кв. {ApartmentNumber}, ком. {RoomNumber}";
                else if (FloorNumber == null)
                    return $"{LocalityName}, д. {HouseNumber}, кв. {ApartmentNumber}, ком. {RoomNumber}, подъезд {EntranceNumber}";
                else
                    return $"{LocalityName}, д. {HouseNumber}, кв. {ApartmentNumber}, ком. {RoomNumber}, подъезд {EntranceNumber}, этаж {FloorNumber}";
            }
        }
        [NotMapped]
        public string FullAddress => getFullAddress();

        [NotMapped]
        public string Freedom
        {
            get
            {
                string freedomStatus = "Исключено";
                if (ApplicationContext.GetContext().ResidenceRegistration.Count(r => r.HousingFundId == IdHousingFund) > 0 && ApplicationContext.GetContext().HouseDecree.Count(r => r.HousingFundId == IdHousingFund) > 0)
                {
                    ResidenceRegistration lastRegistration = ApplicationContext.GetContext().ResidenceRegistration.OrderBy(t => t.DateStartResidence).Last(r => r.HousingFundId == IdHousingFund);
                    HouseDecree lastDecree = ApplicationContext.GetContext().HouseDecree.OrderBy(t => t.Decree.DateDecree).Last(r => r.HousingFundId == IdHousingFund);
                    if (lastRegistration.DateEndResidence == null && lastDecree.Decree.Status == true)
                    {
                        freedomStatus = "Занято";
                    }
                    else if (lastRegistration.DateEndResidence != null && lastDecree.Decree.Status == true)
                    {
                        freedomStatus = "Свободно";
                    }
                }
                else if (ApplicationContext.GetContext().HouseDecree.Count(r => r.HousingFundId == IdHousingFund) > 0)
                {
                    HouseDecree lastDecree = ApplicationContext.GetContext().HouseDecree.OrderBy(t => t.Decree.DateDecree).Last(r => r.HousingFundId == IdHousingFund);
                    if (lastDecree.Decree.Status == true)
                    {
                        freedomStatus = "Свободно";
                    }
                }
                else
                {
                    freedomStatus = "Нет постановления";
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
                if (ApplicationContext.GetContext().ResidenceRegistration.Count(r => r.HousingFundId == IdHousingFund) > 0 && ApplicationContext.GetContext().HouseDecree.Count(r => r.HousingFundId == IdHousingFund) > 0)
                {
                    ResidenceRegistration lastRegistration = ApplicationContext.GetContext().ResidenceRegistration.OrderBy(t => t.DateStartResidence).Last(r => r.HousingFundId == IdHousingFund);
                    HouseDecree lastDecree = ApplicationContext.GetContext().HouseDecree.OrderBy(t => t.Decree.DateDecree).Last(r => r.HousingFundId == IdHousingFund);
                    if (lastRegistration.DateEndResidence == null && lastDecree.Decree.Status == true)
                    {
                        color = Brushes.PaleVioletRed;
                    }
                    else if (lastRegistration.DateEndResidence != null && lastDecree.Decree.Status == true)
                    {
                        color = Brushes.LightGreen;
                    }
                }
                else if (ApplicationContext.GetContext().HouseDecree.Count(r => r.HousingFundId == IdHousingFund) > 0)
                {
                    HouseDecree lastDecree = ApplicationContext.GetContext().HouseDecree.OrderBy(t => t.Decree.DateDecree).Last(r => r.HousingFundId == IdHousingFund);
                    if (lastDecree.Decree.Status == true)
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
        [NotMapped]
        public int listNum
        {
            get
            {
                int num = 4;
                if (ApplicationContext.GetContext().ResidenceRegistration.Count(r => r.HousingFundId == IdHousingFund) > 0 && ApplicationContext.GetContext().HouseDecree.Count(r => r.HousingFundId == IdHousingFund) > 0)
                {
                    ResidenceRegistration lastRegistration = ApplicationContext.GetContext().ResidenceRegistration.OrderBy(t => t.DateStartResidence).Last(r => r.HousingFundId == IdHousingFund);
                    HouseDecree lastDecree = ApplicationContext.GetContext().HouseDecree.OrderBy(t => t.Decree.DateDecree).Last(r => r.HousingFundId == IdHousingFund);
                    if (lastRegistration.DateEndResidence == null && lastDecree.Decree.Status == true)
                    {
                        num = 3;
                    }
                    else if (lastRegistration.DateEndResidence != null && lastDecree.Decree.Status == true)
                    {
                        num = 2;
                    }
                }
                else if (ApplicationContext.GetContext().HouseDecree.Count(r => r.HousingFundId == IdHousingFund) > 0)
                {
                    HouseDecree lastDecree = ApplicationContext.GetContext().HouseDecree.OrderBy(t => t.Decree.DateDecree).Last(r => r.HousingFundId == IdHousingFund);
                    if (lastDecree.Decree.Status == true)
                    {
                        num = 2;
                    }
                }
                else
                {
                    num = 1;
                }
                return num;
            }
        }
    }
}
