using System;
using System.Windows;
using Реестр_маневренного_фонда.Pages;
using Реестр_маневренного_фонда.Pages.ResidenceRegistrations;

namespace Реестр_маневренного_фонда.TablesManagersClasses
{
    public class ResidenceRegistrationManager
    {
        private ApplicationContext dbContext = ApplicationContext.GetContext();

        public void EditResidenceRegistration(ResidenceRegistration currentResidenceRegistration, DateTime? dateEndResidence)
        {
            try
            {
                currentResidenceRegistration.DateEndResidence = dateEndResidence;

                dbContext.ResidenceRegistration.Update(currentResidenceRegistration);
                dbContext.SaveChanges();

                MessageBox.Show("Факт проживания изменён изменён", "", MessageBoxButton.OK, MessageBoxImage.Information);
                MainFrameObj.mainFrame.Navigate(new LocalitiesViewPage());
            }
            catch
            {
                MessageBox.Show("Не удалось изменить факт проживания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RemoveResidenceRegistration(ResidenceRegistration currenResidenceRegistration)
        {
            try
            {
                MessageBoxResult messageBoxResult = MessageBox.Show($"Удалить факт проживания?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes:
                        dbContext.ResidenceRegistration.Remove(currenResidenceRegistration);
                        dbContext.SaveChanges();

                        MessageBox.Show("Факт проживания удалён", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        MainFrameObj.mainFrame.Navigate(new ResidenceRegistrationViewPage());
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            catch
            {
                MessageBox.Show("Не удалось удалить факт проживания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}