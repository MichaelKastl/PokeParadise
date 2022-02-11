using System.Collections.Generic;

namespace PokeParadise
{
    public class Evolution
    {
        public int evolveToDex;
        public string evolveTo;
        public int evolveFromDex;
        public string evolveFrom;
        public EvoDetail evolutionTrigger;

        public Evolution()
        {
        }

        public Evolution(int evolveToDex, string evolveTo, int evolveFromDex, string evolveFrom, EvoDetail evolutionTrigger)
        {
            this.evolveToDex = evolveToDex;
            this.evolveTo = evolveTo;
            this.evolveFromDex = evolveFromDex;
            this.evolveFrom = evolveFrom;
            this.evolutionTrigger = evolutionTrigger;
        }
    }
}
