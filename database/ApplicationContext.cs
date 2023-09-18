using Microsoft.EntityFrameworkCore;
using Реестр_маневренного_фонда.database.tables_classes;

namespace Реестр_маневренного_фонда
{
    public class ApplicationContext : DbContext
    {
        public DbSet<ImprovementDegree> ImprovementDegree { get; set; }
        public DbSet<Locality> Locality { get; set; }
        public DbSet<Street> Street { get; set; }
        public DbSet<TempResident> TempResident { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=C:\\Users\\DNS\\OneDrive\\Рабочий стол\\Visual Studio\\Реестр маневренного фонда\\database\\ManeuverFundRegister.db");
        }
    }
}
