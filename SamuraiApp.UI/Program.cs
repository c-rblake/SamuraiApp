using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        // EF core Will not Track see in Quick Watch
        private static SamuraiContext _contextNT = new SamuraiContextNoTracking();
        static void Main(string[] args)
        {
            //_context.Database.EnsureCreated(); //Check if Db exists else creates it on the fly
            //GetSamurais("Before Add:");
            //AddSamurai();
            //AddVariousTypes();
            //AddSamuraisByName("Shimada", "Okamoto", "Kikuchio","Hayashida");
            //GetSamurais("After Add:");
            //RetriveAndUpdateSamurai();
            //QueryFilters();
            //RetriveAndDeleteSamurai();
            //ContextAddVariousTypes();
            //InsertNewSamuraiWithAQuote(); // Like an Owner with a Membership
            //QueryAndUpdateBattle_Disconnected();
            //AddQuoteToExistingSamuraiWhileTracked();
            //AddQuoteToExistingSamuraiNotTracked();
            //ForeignKey_AddQuoteToExistingSamuraiNotTracked(); GOOD
            //EagerLoadSamuraiWithQuotes(); //GOOD - ALL OR FILTERED WITH INCLUDE One to Many Many to Many filter.
            //ProjectSomeProperties();
            //ProjectSamuraisWithQuotes(); // Filter, create anynomous class with aggregate(count) Owner + Like Vehicles
            //ExplicitLoadQuotes(); // Load an object then load related Objects (Tables or singles) Owner then Vehicles, Membership. WORK ON ONE OBJECT ONLY
            FilteringWithRelatedData(); // Does not include the table filtered on. 
            //ModifyingRelatedDataWhenTracked(); // And how Nav propreties reference and collections + nested looks like
            //ModifyingRelatedDataWhenNOTTracked();
            Console.Write("Press any key...");
            Console.ReadKey();
            
        }

        private static void ModifyingRelatedDataWhenNOTTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes)
                 .FirstOrDefault(s => s.Id == 3);
            var quote = samurai.Quotes[0];
            quote.Text += "Did you hear that again?";

            using var newContext = new SamuraiContext();
            // newContext.Quotes.Updatequote(); Updates the WHOLE THING
            newContext.Entry(quote).State = EntityState.Modified; // Only modifies this particular Quote.
            newContext.SaveChanges();
        }

        private static void ModifyingRelatedDataWhenTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes) //Extensions on Quotes) .ThenInclude(q=>q.Translations.//Extensions on Translations)
                            .FirstOrDefault(s => s.Id == 2);//Extension on SAMURAI
            samurai.Quotes[0].Text = "Did you hear that?";
            _context.SaveChanges();
        }

        private static void FilteringWithRelatedData()
        {
            //DOES NOT INCLUDE THE QUOTES
            var samurais = _context.Samurais
                .Where(s => s.Quotes.Any(q => q.Text.Contains("happy")))
                .ToList();
        }

        private static void ExplicitLoadQuotes()
        {
            //WORKs ON ONE OBJECT ONLY
            _context.Set<Horse>().Add(new Horse { SamuraiId = 1, Name = "Mr Ed" });
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            //-----------------------------
            var samurai = _context.Samurais.Find(1); // Samurai is now IN MEMORY
            _context.Entry(samurai).Collection(s => s.Quotes).Load();
            _context.Entry(samurai).Reference(s => s.Horse).Load();
        }

        private static void ProjectSamuraisWithQuotes()
        {  // AGGREGATE Owner + Vehicles
            //var somePropsWithQuotes = _context.Samurais
            //    .Select(s => new
            //    {
            //        s.Id, s.Name, s.Quotes.Count 
            //    }).ToList();

            // SOME SAMURAI PROPERTIES SELECTED
            //var somePropsWithQuotes = _context.Samurais 
            //    .Select(s => new { s.Id, s.Name, HappyQuotes = s.Quotes
            //    .Where(q => q.Text.Contains("happy")) }).ToList();

            // ALL SAMURAI SELECTED anynously and Filtered
            var samuraiANDFilteredQuotes = _context.Samurais
                .Select(s => new { Samurai = s,
                    HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))
                }).ToList();

            //var firstsamurai = samuraiANDFilteredQuotes[0].Samurai.Name += " The happiest";
        }

        private static void ProjectSomeProperties()
            //Leverage NEW keyword to get anything
        {
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList(); //Anynomous type
        }


        private static void EagerLoadSamuraiWithQuotes()
        {
            var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList(); // Loads all Samurai + Quotes

            // INCLUDE AND FILTER.
            var filterInclude = _context.Samurais.Include(s => s.Quotes.Where(q =>q.Text.Contains("Thanks"))).ToList();

            var filterSamurai = _context.Samurais.FirstOrDefault(s => s.Id == 1);
            // Get filtered Samurai and Quotes by Quering quotes?
            var filterQuotesSamurai = _context.Quotes.Include(q => q.Samurai).ToList();
            var filterQuoteSamurai = _context.Quotes.Where(q => q.SamuraiId == 1).ToList(); //THere we go..
            // Think of it as a Table with Row of Foreign Keys. Set that Row to True or False == Id
            // Then subset the table on that row.
        }

        private static void ForeignKey_AddQuoteToExistingSamuraiNotTracked(int samuraiId = 2)
        {
            var quote = new Quote { Text = "Thanks for dinner!", SamuraiId = samuraiId };
            using var newContext = new SamuraiContext(); // Disposed when variable is out of scope. LN 43 here.
            newContext.Quotes.Add(quote);
            newContext.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiNotTracked(int id = 2)
        {
            var samurai = _context.Samurais.Find(id);
            samurai.Quotes.Add(new Quote
            { Text = " Now that I have saved you, will you feed me dinner?" });

            using (var newContext = new SamuraiContext()) // Builds and dismantels
            {
                newContext.Samurais.Attach(samurai);
                newContext.SaveChanges();
            }
        }

        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote
            { Text = "I bet you are happy that I have saved you" });

            _context.SaveChanges();
        }

        private static void InsertNewSamuraiWithAQuote()
        {
            var samurai = new Samurai
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>
                {
                    new Quote {Text = "I've come to save you"}
                }
            };
            _context.Samurais.Add(samurai); //Needs to Track this samurai
            _context.SaveChanges();
        }

        private static void QueryAndUpdateBattle_Disconnected()
            //Simulates a Disconnected Scenario. One context load, one context saves.
        {
            List<Battle> disconnectedBattles;
            using (var context1=new SamuraiContext())
            {
                disconnectedBattles = _context.Battles.ToList();
            }
            disconnectedBattles.ForEach(b =>
            {
                b.StartDate = new DateTime(1570, 01, 01);
                b.EndDate = new DateTime(1570, 12, 01);
            });
            using (var context2 = new SamuraiContext())
            {
                context2.UpdateRange(disconnectedBattles);
                context2.SaveChanges();
            }

        }

        private static void QueryFilters()
        {
            // Not Parametrized Search Query
            var samurais = _context.Samurais.Where(s => s.Name == "Sampson").ToList(); // ADD logging or this doesnt work well at ALL.
            // Parametrized Search Query
            var name = "Sampson";
            var samurais2 = _context.Samurais.FirstOrDefault(s => s.Name == name); //SAMURAI OBJECT not list
            //var samurai2 = _context.Samurais.Where(s => s.Name == name).ToList(); // A lot slower as well, Name is indexed maybe?
            //SEARCH
            var samurai3 = _context.Samurais.Where(s => EF.Functions.Like(s.Name, "%J%")).ToList();
            //var samurai3 = _context.Samurais.Where(s => s.Name.Contains("%abc%")); // Same as above. but slower.

            //DB set Method to find ID instead of First or Default on ID
            var samurai4 = _context.Samurais.Find(2); // Finds samurai at index 2.
            Console.WriteLine($"{samurai4.Name} is samurai at index 2");


        }

        public static void RetriveAndUpdateSamurai()
            //CONTEXT tracks changeds. 4 or more becomes a Batch update to the database.
        {
            //var samurai = _context.Samurais.FirstOrDefault();
            //samurai.Name += "San";
            var samurais = _context.Samurais.Skip(1).Take(4).ToList();
            //foreach (var samurai in samurais)
            //{
            //    samurai.Name += "San";
            //}
            samurais.ForEach(s => s.Name += "San");
            _context.Samurais.Add(new Samurai { Name = "Shino" }); //There is still only ONE database Call.
            _context.SaveChanges(); // Save changes
        }

        public static void RetriveAndDeleteSamurai()
        {
            //First DbContext needs to Track what is to be deleted.
            var samurai = _context.Samurais.Find(18);
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();

        }


        //private static void AddSamurai()
        //{
        //    var samurai = new Samurai { Name = "Julie" };
        //    _context.Samurais.Add(samurai);
        //    _context.SaveChanges();
        //}

        private static void AddSamuraisByName(params string[] names)
        {
            foreach (var name in names)
            {
                _context.Samurais.Add(new Samurai { Name = name });
            }
            _context.SaveChanges();
        }
        private static void GetSamurais(string text)
        {
            //var samurais = _context.Samurais.ToList(); //ToList here is a LINQ Query
            var samurais = _context.Samurais
                .TagWith("ConsoleApp.Program.GetSamurais method")
                .ToList();
            Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }
        // Using the Context .AddRange method for adding various things
        // Bulk operation
        private static void AddVariousTypes()
        {
            _context.Samurais.AddRange( // _coontext.AddRange() will also work
                new Samurai { Name = "Shimada" },
                new Samurai { Name = "Okamoto" });
            _context.Battles.AddRange(
               new Battle { Name = "Battle of Anegawa" },
               new Battle { Name = "Battle of Nagashino" });
            _context.SaveChanges();

        }
        private static void ContextAddVariousTypes()
        {
            _context.AddRange(
            new Battle { Name = "Battle Master" },
            new Battle { Name = "Battle of the Kiwi" });
            _context.SaveChanges();
        }




    }
}
