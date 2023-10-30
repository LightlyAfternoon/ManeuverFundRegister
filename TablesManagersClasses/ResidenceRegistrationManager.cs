using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Реестр_маневренного_фонда.database.tables_classes;

namespace Реестр_маневренного_фонда.TablesManagersClasses
{
    public class ResidenceRegistrationManager
    {
        private ApplicationContext dbContext = ApplicationContext.GetContext();
        
        public void AddResidenceRegistration(Agreement? newAgreement)
        {
            ResidenceRegistration lastRegistration = dbContext.ResidenceRegistration.Last(r => r.HousingFundId == newAgreement.HousingFundId);
            
            ResidenceRegistration newRegistration = new ResidenceRegistration();
            if (dbContext.ResidenceRegistration.Count(r => r.HousingFundId == newAgreement.HousingFundId && lastRegistration.DateEndResidence == null) > 0)
            {
                if (lastRegistration.TempResidentId != newAgreement.TempResidentId)
                {
                    MessageBoxResult boxResult = MessageBox.Show($"Для данного жилья на данный момент проживающим числится {lastRegistration.TempResident.FullName}. Поставить дату окончания его проживания в жилье и назначить сейчас проживающим {newAgreement.TempResident.FullName}?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    switch (boxResult)
                    {
                        case MessageBoxResult.Yes:
                            lastRegistration.DateStartResidence = (DateTime)dateConclusion;
                                    
                            newRegistration.HousingFundId = newAgreement.HousingFundId;
                            newRegistration.TempResidentId = newAgreement.TempResidentId;
                            newRegistration.DateStartResidence = newAgreement.dateConclusion;
                            newRegistration.StartAgreementId = newAgreement.IdAgreement;
                                    
                            dbContext.ResidenceRegistration.Add(newRegistration);
                            break;
                        case MessageBoxResult.No:
                            MessageBox.Show("Необходимо выбрать другое жильё", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            AgreementManager am = new AgreementManager();
                            am.AddError("Необходимо выбрать другое жильё");
                            am.AddAgreement(newAgreement.number, newAgreement.TempResident, newAgreement.HousingFund, newAgreement.DateConclusion, newAgreement.DateEnd, newAgreement.Remark);
                            break;
                    }
                }
            }
            else
            {
                newRegistration.HousingFundId = newAgreement.HousingFundId;
                newRegistration.TempResidentId = newAgreement.TempResidentId;
                newRegistration.DateStartResidence = newAgreement.DateConclusion;
                newRegistration.StartAgreementId = newAgreement.IdAgreement;
                        
                dbContext.ResidenceRegistration.Add(newRegistration);
            }
        }

        public void EditResidenceRegistration(ResidenceRegistration currentResidenceRegistration)
        {
            
        }

        public void RemoveResidenceRegistration(Locality currenResidenceRegistration)
        {

        }
    }
}
