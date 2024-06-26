﻿using System.ComponentModel.DataAnnotations;

namespace Реестр_маневренного_фонда.database.tables_classes
{
    public partial class Street
    {
        [Key]
        public int IdStreet { get; set; }
        public int LocalityId { get; set; }
        public string NameStreet { get; set; }

        public virtual Locality Locality { get; set; }
    }
}