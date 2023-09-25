using System.ComponentModel.DataAnnotations;
using System.Linq;
using Реестр_маневренного_фонда.database.tables_classes;

namespace Реестр_маневренного_фонда
{
    public class HousingFund
    {
        [Key]
        public int IdHouse { get; set; }
        public int StreetId { get; set; }
        public int HouseNumber { get; set; }
        public int? ApartmentNumber { get; set; }
        public int? RoomNumber { get; set; }
        public int ImprovementDegreeId { get; set; }
        public float? DecreeArea { get; set; }
        public float? RegisterArea { get; set; }
        public string? Remark { get; set; }

        public Street Street { get; set; }
        public ImprovementDegree ImprovementDegree { get; set; }

        ApplicationContext db = new ApplicationContext();

        private string fullAddress;
        public string FullAddress
        {
            string LocalityName = db.Street.First(s => s.IdStreet == StreetId).Locality.NameLocality;
            string StreetName = db.Street.First(s => s.IdStreet == StreetId).NameStreet;

            get => fullAddress;
            set =>
                if (ApartmentNumber == null)
                    fullAddress = $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}";
                else if (RoomNumber == null)
                    fullAddress = $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}, кв. {ApartmentNumber}";
                else
                    fullAddress = $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}, кв. {ApartmentNumber}, ком. {RoomNumber}";

        }
        /*public string getFullAddress() 
        {
            string LocalityName = db.Street.First(s => s.IdStreet == StreetId).Locality.NameLocality;
            string StreetName = db.Street.First(s => s.IdStreet == StreetId).NameStreet;
            
            if (ApartmentNumber == null)
                return $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}";
            else if (RoomNumber == null)
                return $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}, кв. {ApartmentNumber}";
            else
                return $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}, кв. {ApartmentNumber}, ком. {RoomNumber}";
        }*/
    }
}
