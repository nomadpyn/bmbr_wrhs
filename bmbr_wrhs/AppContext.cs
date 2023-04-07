using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bmbr_wrhs
{
    public class AppContext : DbContext
    {
        public DbSet<AutoPart> Autoparts { get; set; } = null!;

        public AppContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = bumbers.db");
        }
    }
}
