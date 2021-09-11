using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamuraiApp.Domain;
using Microsoft.Extensions.Logging;

namespace SamuraiApp.Data
{
    public class SamuraiContext:DbContext
        // Ef Core DbContext does the work against our database/persistent data.
    {
        //Wrappers for Dealing with our Samurai and Qutoes Contexts
        public DbSet<Samurai> Samurais { get; set; }
        
        public DbSet<Quote> Quotes { get; set; }

        public DbSet<Battle> Battles { get; set; }

        // In Ef core the Relationships are infered from the classes Samurai and Quotes.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog=SamuraiAppData" // Not a good way.Use appsettings.
                )
                /*.LogTo(Console.WriteLine);*/ // Very verbose.
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, //, DbLoggerCategory.Database.Transaction.Name },
                LogLevel.Information) // Just the SQL, limits the information from logger.
                .EnableSensitiveDataLogging(); // See parameter names  NOT DEFAULT should be hidden

            //base.OnConfiguring(optionsBuilder);
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Samurai>() // Start with Samurai 
                .HasMany(s => s.Battles) // Has many battles.
                .WithMany(b => b.Samurais) // Battles has many samurais
                .UsingEntity<BattleSamurai>
                (bs => bs.HasOne<Battle>().WithMany(),
                bs => bs.HasOne<Samurai>().WithMany())
                .Property(bs => bs.DateJoined) // Set the property to now time in sql.
                .HasDefaultValueSql("getdate()");
        }
    }

}
