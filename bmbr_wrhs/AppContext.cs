using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace bmbr_wrhs
{
    // Создаем DBContext для наших моделей
    public class AppContext : DbContext
    {

        // DBSet для необходимых операций

        public DbSet<AutoPart> Autoparts { get; set; } = null!;
        public DbSet<Car> Car { get; set; } = null!;
        public DbSet<CarColor> CarColor { get; set; } = null!;
        public DbSet<Color> Color { get; set; } = null!;
        public DbSet<PartType> PartType { get; set; } = null!;
        public DbSet<SoldPart> SoldParts { get; set; } = null!;

        // конструктор по умолчанию для контекста
        
        public AppContext()
        {
            Database.EnsureCreated();
        }

        // метод для конфигурации

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {           
            optionsBuilder.UseSqlite(ConfigurationManager.ConnectionStrings["BumpersDatabase"].ConnectionString);
        }
    }
}
