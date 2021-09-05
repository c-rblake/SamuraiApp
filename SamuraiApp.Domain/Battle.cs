using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Domain
{
    public class Battle 
    // Many to many relationship with Samurai
    {
        public int BattleId{ get; set; }
        public string Name { get; set; }
        public List<Samurai> Samurais { get; set; } // Many
    }
}
