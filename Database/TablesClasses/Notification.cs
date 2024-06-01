using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows;

namespace Реестр_маневренного_фонда
{
    public partial class Notification
    {
        [Key]
        public int IdNotification { get; set; }
        public int AgreementId { get; set; }
        public DateTime RecievingDate { get; set; }
        public bool IsViewed { get; set; }

        public Agreement Agreement { get; set; }

        [NotMapped]
        public Visibility ViewButton
        { 
            get
            {
                Visibility view;
                if (IsViewed == false)
                {
                    view = Visibility.Visible;
                }
                else
                {
                    view = Visibility.Collapsed;
                }

                return view;
            }
        }
    }
}