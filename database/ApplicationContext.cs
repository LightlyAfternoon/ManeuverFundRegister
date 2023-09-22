using System;
using Microsoft.EntityFrameworkCore;
using Реестр_маневренного_фонда.database.tables_classes;

namespace Реестр_маневренного_фонда
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Agreement> Agreement { get; set; }
        public DbSet<Decree> Decree { get; set; }
        public DbSet<HousingFund> HousingFund { get; set; }
        public DbSet<ImprovementDegree> ImprovementDegree { get; set; }
        public DbSet<Locality> Locality { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<ResidenceRegistration> ResidenceRegistration { get; set; }
        public DbSet<Street> Street { get; set; }
        public DbSet<TempResident> TempResident { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=" + Environment.CurrentDirectory + @"\database\ManeuverFundRegister.db");
        }
    }
}
