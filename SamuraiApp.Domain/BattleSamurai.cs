using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Domain
{
    public class BattleSamurai
        // A specified class for connecting Battle And Samurai many to many.
    {
        public int SamuraiId { get; set; }
        public int BattleId { get; set; }
        public DateTime DateJoined { get; set; } // This extra data is called a "Payload"
    }
}
