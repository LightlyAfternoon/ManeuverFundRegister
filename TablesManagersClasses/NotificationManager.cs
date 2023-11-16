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

        public void AddNotification()
        {
            dbContext.Agreement.Load();
            dbContext.Notification.Load();

            listAgreements = dbContext.Agreement.ToList();
            listNotifications = dbContext.Notification.ToList();

            foreach (Agreement agreement in listAgreements)
            {
                if (agreement.DateEndAgreement <= DateTime.Now.AddMonths(6) && dbContext.Notification.Count(n => n.AgreementId == agreement.IdAgreement) < 1 && string.IsNullOrWhiteSpace(agreement.DateTerminationAgreement.ToString()))
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
        }

        public void RemoveNotification()
        {
            dbContext.Agreement.Load();
            dbContext.Notification.Load();

            listAgreements = dbContext.Agreement.ToList();
            listNotifications = dbContext.Notification.ToList();

            foreach (Notification notification in listNotifications)
            {
                if (notification.RecievingDate > DateTime.Now.AddMonths(6))
                {
                    dbContext.Remove(notification);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}