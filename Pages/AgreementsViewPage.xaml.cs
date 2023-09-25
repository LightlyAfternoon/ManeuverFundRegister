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

namespace Реестр_маневренного_фонда
{
    /// <summary>
    /// Interaction logic for AgreementsViewPage.xaml
    /// </summary>
    public partial class AgreementsViewPage : Page
    {
        ApplicationContext db = new ApplicationContext();

        public AgreementsViewPage()
        {
            InitializeComponent();

            db.Agreement.Load();
            dgAgreements.ItemsSource = dg.Agreement.Local.ToObservableCollection();
        }
    }
}