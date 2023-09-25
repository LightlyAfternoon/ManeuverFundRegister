using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Реестр_маневренного_фонда.Pages
{
    /// <summary>
    /// Логика взаимодействия для AgreementsViewPage.xaml
    /// </summary>
    public partial class AgreementsViewPage : Page
    {
        ApplicationContext db = new ApplicationContext();

        public AgreementsViewPage()
        {
            InitializeComponent();

            db.Agreement.Load();
            db.Locality.Load();
            db.Street.Load();
            db.TempResident.Load();
            dg_Agreements.ItemsSource = ApplicationContext.GetContext().Agreement.ToList();
        }
    }
}
