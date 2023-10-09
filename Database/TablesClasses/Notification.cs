using System;
using System.ComponentModel.DataAnnotations;

namespace Реестр_маневренного_фонда
{
    public partial class Notification
    {
        [Key]
        public int IdNotification { get; set; }
        public int AgreementId { get; set; }
        public DateTime RecievingDate { get; set; }

        public Agreement Agreement { get; set; }
    }
}
