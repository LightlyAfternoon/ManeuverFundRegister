using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Реестр_маневренного_фонда.database.tables_classes;

namespace Реестр_маневренного_фонда
{
    public partial class HousingFund
    {
        [Key]
        public int IdHousingFund { get; set; }
        public static int StreetId { get; set; }
        public int HouseNumber { get; set; }
        public int? ApartmentNumber { get; set; }
        public int? RoomNumber { get; set; }
        public int ImprovementDegreeId { get; set; }
        public float? DecreeArea { get; set; }
        public float? RegisterArea { get; set; }
        public string? Remark { get; set; }

        public virtual Street Street { get; set; }
        public virtual ImprovementDegree ImprovementDegree { get; set; }

        ApplicationContext db = new ApplicationContext();

        [NotMapped]
        private string LocalityName;
        [NotMapped]
        private string StreetName;

        [NotMapped]
        private string _fullAddress;

        [NotMapped]
        public string FullAddress
        {
            get { return _fullAddress; }
            set
            {
                db.Street.Load();
                db.Locality.Load();

                LocalityName = db.Street.First(s => s.IdStreet == StreetId).Locality.NameLocality;
                StreetName = db.Street.First(s => s.IdStreet == StreetId).NameStreet;

                if (ApartmentNumber == null)
                    _fullAddress = $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}";
                else if (RoomNumber == null)
                    _fullAddress = $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}, кв. {ApartmentNumber}";
                else
                    _fullAddress = $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}, кв. {ApartmentNumber}, ком. {RoomNumber}";
            }
        }
        //public string getFullAddress() 
        //{
        //    string LocalityName = ApplicationContext.GetContext().Street.First(s => s.IdStreet == StreetId).Locality.NameLocality;
        //    string StreetName = ApplicationContext.GetContext().Street.First(s => s.IdStreet == StreetId).NameStreet;
            
        //    if (ApartmentNumber == null)
        //        return $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}";
        //    else if (RoomNumber == null)
        //        return $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}, кв. {ApartmentNumber}";
        //    else
        //        return $"{LocalityName}, ул. {StreetName}, д. {HouseNumber}, кв. {ApartmentNumber}, ком. {RoomNumber}";
        //}
    }
}
