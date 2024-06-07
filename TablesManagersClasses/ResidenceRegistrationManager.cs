using System;
using System.Windows;
using Реестр_маневренного_фонда.Pages.ResidenceRegistrations;

namespace Реестр_маневренного_фонда.TablesManagersClasses
{
    public class ResidenceRegistrationManager
    {
        private ApplicationContext dbContext = ApplicationContext.GetContext();
        private string errors = string.Empty;

        private void showErrors(ResidenceRegistration currentResidenceRegistration, DateTime? dateEndResidence)
        {
            if (dateEndResidence != null && currentResidenceRegistration.DateStartResidence > dateEndResidence)
            {
                errors += ("Дата окончания проживания не может быть позже даты начала");
            }
        }

        public void EditResidenceRegistration(ResidenceRegistration currentResidenceRegistration, DateTime? dateEndResidence)
        {
            showErrors(currentResidenceRegistration, dateEndResidence);
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(errors);
                errors = string.Empty;
            }
            else
            {
                try
                {
                    currentResidenceRegistration.DateEndResidence = dateEndResidence;

                    dbContext.ResidenceRegistration.Update(currentResidenceRegistration);
                    dbContext.SaveChanges();

                    MessageBox.Show("Факт проживания изменён", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainFrameObj.mainFrame.Navigate(new ResidenceRegistrationViewPage());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось изменить факт проживания\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void RemoveResidenceRegistration(ResidenceRegistration currenResidenceRegistration)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show($"Удалить факт проживания?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        dbContext.ResidenceRegistration.Remove(currenResidenceRegistration);
                        dbContext.SaveChanges();

                        MessageBox.Show("Факт проживания удалён", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        MainFrameObj.mainFrame.Navigate(new ResidenceRegistrationViewPage());
                    }
                    catch
                    {
                        MessageBox.Show("Не удалось удалить факт проживания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
    }
}