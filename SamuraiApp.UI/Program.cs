﻿using Microsoft.EntityFrameworkCore;
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
            //InsertNewSamuraiWithAQuote();
            //QueryAndUpdateBattle_Disconnected();
            //AddQuoteToExistingSamuraiWhileTracked();
            //AddQuoteToExistingSamuraiNotTracked();
            Console.Write("Press any key...");
            Console.ReadKey();
            
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
