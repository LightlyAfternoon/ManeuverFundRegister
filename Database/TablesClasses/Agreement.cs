using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows;
using Реестр_маневренного_фонда.database.tables_classes;

namespace Реестр_маневренного_фонда
{
    public partial class Agreement
    {
        [Key]
        public int IdAgreement { get; set; }
        public int NumberAgreement { get; set; }
        public int TempResidentId { get; set; }
        public int HousingFundId { get; set; }
        public DateTime DateConclusionAgreement { get; set; }
        public DateTime DateEndAgreement { get; set; }
        public DateTime? DateTerminationAgreement { get; set; }
        public string? Remark { get; set; }
        public byte[]? File { get; set; }

        public virtual TempResident TempResident { get; set; }
        public virtual HousingFund HousingFund { get; set; }

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

        [NotMapped]
        public string numAgreementAndDataConcl
        {
            get
            {
                string agreementInfo = $"№{NumberAgreement} от {DateConclusionAgreement}";

                return agreementInfo;
            }
        }
    }
}