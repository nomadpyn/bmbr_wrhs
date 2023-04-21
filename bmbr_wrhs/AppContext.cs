using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace bmbr_wrhs
{
    public class AppContext : DbContext
    {
        public DbSet<AutoPart> Autoparts { get; set; } = null!;
        public DbSet<Car> Car { get; set; } = null!;
        public DbSet<CarColor> CarColor { get; set; } = null!;
        public DbSet<Color> Color { get; set; } = null!;
        public DbSet<PartType> PartType { get; set; } = null!;
        public DbSet<SoldPart> SoldParts { get; set; } = null!;

        public AppContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {           
            optionsBuilder.UseSqlite(ConfigurationManager.ConnectionStrings["BumpersDatabase"].ConnectionString);
        }
    }
}
