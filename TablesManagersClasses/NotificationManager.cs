using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Реестр_маневренного_фонда.TablesManagersClasses
{
    public class NotificationManager
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();

        List<Agreement> listAgreements;
        List<Notification> listNotifications;
        List<ResidenceRegistration> listResidenceRegistration;

        public void AddNotification()
        {
            dbContext.Agreement.Load();
            dbContext.Notification.Load();

            listAgreements = dbContext.Agreement.OrderBy(a => a.DateConclusionAgreement).OrderBy(a => a.HousingFundId).Where(a => a.DateTerminationAgreement == null).ToList();
            listNotifications = dbContext.Notification.ToList();
            listResidenceRegistration = dbContext.ResidenceRegistration.OrderBy(r => r.DateStartResidence).OrderBy(r => r.HousingFundId).ToList();
            foreach (ResidenceRegistration residenceRegistration in listResidenceRegistration)
            {
                foreach (Agreement agreement in listAgreements)
                {
                    if (listAgreements.SkipWhile(a => a.IdAgreement != agreement.IdAgreement).Skip(1).FirstOrDefault() != null && listResidenceRegistration.SkipWhile(a => a.IdRegistration != residenceRegistration.IdRegistration).Skip(1).FirstOrDefault() != null)
                    {
                        if (listResidenceRegistration.SkipWhile(a => a.IdRegistration != residenceRegistration.IdRegistration).Skip(1).FirstOrDefault().AgreementId == listAgreements.SkipWhile(a => a.IdAgreement != agreement.IdAgreement).Skip(1).FirstOrDefault().IdAgreement
                            && residenceRegistration.DateEndResidence == null)
                        {
                            addNotifications(agreement);
                        }
                    }
                    else if (residenceRegistration.DateEndResidence == null && agreement == listAgreements.Last() && residenceRegistration == listResidenceRegistration.Last())
                    {
                        addNotifications(agreement);
                    }
                }
            }
        }

        private void addNotifications(Agreement agreement)
        {
            if (agreement.DateEndAgreement <= DateTime.Now.AddMonths(1) && dbContext.Notification.Count(n => n.AgreementId == agreement.IdAgreement) < 1)
            {
                Notification newNotification = new Notification
                {
                    AgreementId = agreement.IdAgreement,
                    RecievingDate = DateTime.Now,
                    IsViewed = false
                };

                dbContext.Notification.Add(newNotification);
                dbContext.SaveChanges();

                new ToastContentBuilder()
                    .AddArgument("action", "viewConversation")
                    .AddArgument("conversationId", 9813)
                    .AddText($"Срок договора №{agreement.NumberAgreement} от {string.Format("{0:dd.MM.yyyy}", agreement.DateConclusionAgreement)} заканчивается {string.Format("{0:dd.MM.yyyy}", agreement.DateEndAgreement)}")
                    .Show();
            }
        }

        public void RemoveNotification()
        {
            dbContext.Agreement.Load();
            dbContext.Notification.Load();

            listAgreements = dbContext.Agreement.ToList();
            listNotifications = dbContext.Notification.ToList();

            foreach (Notification notification in listNotifications)
            {
                if (notification.RecievingDate > DateTime.Now.AddMonths(1))
                {
                    dbContext.Remove(notification);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}