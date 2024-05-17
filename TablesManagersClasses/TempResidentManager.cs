using System.Windows;
using Реестр_маневренного_фонда.database.tables_classes;
using Реестр_маневренного_фонда.Pages;

namespace Реестр_маневренного_фонда.TablesManagersClasses
{
    public class TempResidentManager
    {
        private ApplicationContext dbContext = ApplicationContext.GetContext();

        private string errors = string.Empty;

        private void showErrors(string? lastName, string? firstName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                errors += ("Необходимо написать фамилию нанимателя\n");
            }
            if (string.IsNullOrWhiteSpace(firstName))
            {
                errors += ("Необходимо написать имя нанимателя");
            }
        }
        public void AddTempResident(string? lastName, string? firstName, string? patronymic, string? remark)
        {
            TempResident newTempResident = new TempResident();
            
            showErrors(lastName, firstName);
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(errors);
                errors = string.Empty;
            }
            else
            {
                try
                {
                    newTempResident.LastName = lastName;
                    newTempResident.FirstName = firstName;
                    if (!string.IsNullOrWhiteSpace(patronymic))
                    {
                        newTempResident.Patronymic = patronymic;
                    }
                    if (!string.IsNullOrWhiteSpace(remark))
                    {
                        newTempResident.Remark = remark;
                    }

                    dbContext.TempResident.Add(newTempResident);
                    dbContext.SaveChanges();

                    MessageBox.Show("Наниматель добавлен", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainFrameObj.mainFrame.Navigate(new TempResidentsViewPage());
                }
                catch
                {
                    MessageBox.Show("Не удалось добавить нанимателя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void EditTempResident(TempResident currentTempResident, string? lastName, string? firstName, string? patronymic, string? remark)
        {
            showErrors(lastName, firstName);
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(errors);
                errors = string.Empty;
            }
            else
            {
                try
                {
                    currentTempResident.LastName = lastName;
                    currentTempResident.FirstName = firstName;
                    if (!string.IsNullOrWhiteSpace(patronymic))
                    {
                        currentTempResident.Patronymic = patronymic;
                    }
                    if (!string.IsNullOrWhiteSpace(remark))
                    {
                        currentTempResident.Remark = remark;
                    }

                    dbContext.TempResident.Update(currentTempResident);
                    dbContext.SaveChanges();

                    MessageBox.Show("Данные нанимателя изменены", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainFrameObj.mainFrame.Navigate(new TempResidentsViewPage());
                }
                catch
                {
                    MessageBox.Show("Не удалось изменить данные нанимателя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        ///////////////////////////////////////////////////////
        public void RemoveTempResident(TempResident currentTempResident)
        {
            try
            {
                MessageBoxResult messageBoxResult = MessageBox.Show($"Удалить нанимателя?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes:
                        dbContext.TempResident.Remove(currentTempResident);
                        dbContext.SaveChanges();

                        MessageBox.Show("Наниматель удалён", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        MainFrameObj.mainFrame.Navigate(new TempResidentsViewPage());
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            catch
            {
                MessageBox.Show("Не получилось удалить нанимателя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}