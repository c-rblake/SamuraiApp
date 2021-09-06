using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Domain
{
    public class Horse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SamuraiId { get; set; } // Default is that horses have to have Samurai
        //public int? SamuraiId { get; set; } On way to make Samurai optional. Another is fluent API.
    }
}
