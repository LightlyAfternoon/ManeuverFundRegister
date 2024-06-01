using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Реестр_маневренного_фонда.database.tables_classes;
using Реестр_маневренного_фонда.Database.TablesClasses;

namespace Реестр_маневренного_фонда
{
    public partial class ApplicationContext : DbContext
    {
        private static ApplicationContext? _context;
        public static ApplicationContext GetContext()
        {
            if (_context == null)
                _context = new ApplicationContext();

            return _context;
        }

        public virtual DbSet<Agreement> Agreement { get; set; }
        public virtual DbSet<Decree> Decree { get; set; }
        public virtual DbSet<HousingFund> HousingFund { get; set; }
        public virtual DbSet<Locality> Locality { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<ResidenceRegistration> ResidenceRegistration { get; set; }
        public virtual DbSet<Street> Street { get; set; }
        public virtual DbSet<TempResident> TempResident { get; set; }
        public virtual DbSet<HouseDecree> HouseDecree { get; set; }

        //переопределение метода OnConfiguring для установления параметров подключения к бд
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=" + Directory.GetParent(Assembly.GetExecutingAssembly().Location.ToString()) + "\\Database\\ManeuverFundRegister.db");
        }
    }
}