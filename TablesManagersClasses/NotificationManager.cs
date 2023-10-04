using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Реестр_маневренного_фонда.TablesManagersClasses
{
    public class NotificationManager
    {
        ApplicationContext dbContext = ApplicationContext.GetContext();

        List<Agreement> listAgreements;
        List<Notification> listNotifications;

        void AddNotification()
        {
            dbContext.Agreement.Load();
            dbContext.Notification.Load();

            List<Agreement> listAgreements = dbContext.Agreement.ToList();
            List<Notification> listNotifications = dbContext.Notification.ToList();

            foreach (Agreement agreement in listAgreements)
            {
                if (agreement.DateEndAgreement <= DateTime.Now.AddMonths(1) && dbContext.Notification.Count(n => n.AgreementId == agreement.IdAgreement) < 1)
                {
                    Notification newNotification = new Notification
                    {
                        AgreementId = agreement.IdAgreement,
                        RecievingDate = DateTime.Now
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

        void RemoveNotification()
        {
            dbContext.Agreement.Load();
            dbContext.Notification.Load();

            List<Agreement> listAgreements = dbContext.Agreement.ToList();
            List<Notification> listNotifications = dbContext.Notification.ToList();

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