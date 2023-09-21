using System.ComponentModel.DataAnnotations;

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

        public string getFullAddress() 
        {
            string LocalityName = "";
            string StreetName = "";
            
            if (ApartmentNumber == null)
                return $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}"
            else if (RoomNumber == null)
                return $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}, кв. {ApartmentNumber}"
            else
                return $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}, кв. {ApartmentNumber}, ком. {RoomNumber}"
        }
    }
}
