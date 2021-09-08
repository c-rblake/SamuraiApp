using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Linq;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        static void Main(string[] args)
        {
            _context.Database.EnsureCreated(); //Check if Db exists else creates it on the fly
            GetSamurais("Before Add:");
            //AddSamurai();
            //AddVariousTypes();
            //AddSamuraisByName("Shimada", "Okamoto", "Kikuchio","Hayashida");
            //GetSamurais("After Add:");
            QueryFilters();
            Console.Write("Press any key...");
            Console.ReadKey();
            
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
        //private static void AddVariousTypes()
        //{
        //    _context.Samurais.AddRange( // _coontext.AddRange() will also work
        //        new Samurai { Name = "Shimada" },
        //        new Samurai { Name = "Okamoto" });
        //    _context.Battles.AddRange(
        //       new Battle { Name = "Battle of Anegawa" },
        //       new Battle { Name = "Battle of Nagashino" });
        //    _context.SaveChanges();

        //}



    }
}
