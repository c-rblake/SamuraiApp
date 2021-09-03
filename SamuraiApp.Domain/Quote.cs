using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Domain
{
    class Quote
    {
        public int MyProperty { get; set; }
        public string Text { get; set; }
        public Samurai Samurai { get; set; } // One to One relationship REFERENCE PROPERTY
        public int SamuraiId { get; set; } // Foreign Key
    }
}
