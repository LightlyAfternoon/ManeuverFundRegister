using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Реестр_маневренного_фонда
{
    public class ApplicationContext : DbContext
    {
        public DbSet<ImprovementDegree> ImprovementDegree { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=C:\\Users\\DNS\\OneDrive\\Рабочий стол\\Visual Studio\\Реестр маневренного фонда\\database\\ManeuverFundRegister.db");
        }
    }
}
