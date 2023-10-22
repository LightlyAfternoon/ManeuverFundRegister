using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Реестр_маневренного_фонда.database.tables_classes;

namespace Реестр_маневренного_фонда.TablesManagersClasses
{
    public class DecreeManager
    {
        private ApplicationContext dbContext = ApplicationContext.GetContext();

        private string errors = string.Empty;

        private void showErrors(string? number, DateTime? dateDecree, HousingFund? housingFund, bool? status)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                errors += ("Неоходимо ввести номер\n");
            }
            else
            {
                try
                {
                    checked
                    {
                        Convert.ToInt32(number);
                    }
                }
                catch
                {
                    errors += ("Укажите номер постановления корректно (только цифры)");
                }
            }
            if (housingFund == null)
            {
                errors += ("Необходимо выбрать жильё\n");
            }
            if (dateDecree == null)
            {
                errors += ("Необходимо выбрать дату постановления\n");
            }

            MessageBox.Show(errors);
            errors = string.Empty;
        }
        public void AddDecree(string? number, DateTime? dateDecree, HousingFund? housingFund, bool? status)
        {
            Decree newDecree = new Decree();

            if (!string.IsNullOrEmpty(errors))
            {
                showErrors(number, dateDecree, housingFund, status);
            }
            else
            {
                newDecree.NumberDecree = Convert.ToInt32(number);
                newDecree.DateDecree = Convert.ToDateTime(dateDecree);
                newDecree.HousingFund = housingFund;
                newDecree.Status = Convert.ToBoolean(status);

                dbContext.Decree.Add(newDecree);
                dbContext.SaveChanges();
            }
        }

        public void EditDecree(Decree currentDecree, string? number, DateTime? dateDecree, HousingFund? housingFund, bool? status)
        {

        }

        public void RemoveDecree(Decree currentDecree)
        {

        }
    }
}
