using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeParadise
{
    public class MoveInfo
    {
        public int id { get; set; }
        public int learnedLevel { get; set; }

        public MoveInfo(int id, int learnedLevel)
        {
            this.id = id;
            this.learnedLevel = learnedLevel;
        }
        public MoveInfo()
        {

        }
    }
}
