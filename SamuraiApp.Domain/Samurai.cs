﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Domain
{
    class Samurai
    {
        public int MyProperty { get; set; }
        public string Name { get; set; }
        public List<Quote> Quotes { get; set; } = new List<Quote>(); // One to MANY
        // Or else it is NULL without new List<Quote>();
    }
}