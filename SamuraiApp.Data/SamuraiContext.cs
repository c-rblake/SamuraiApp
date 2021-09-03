using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    public class SamuraiContext:DbContext
        // Ef Core DbContext does the work against our database/persistent data.
    {
        //Wrappers for Dealing with our Samurai and Qutoes Contexts
        public DbSet<Samurai> Samurai { get; set; }
        
        public DbSet<Quote> Quotes { get; set; }

        // In Ef core the Relationships are infered from the classes Samurai and Quotes.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog=SamuraiAppData"
                );
            //base.OnConfiguring(optionsBuilder);
        }
    }

}
