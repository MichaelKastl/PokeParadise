using System;
using System.Collections.Generic;
using System.Text;

namespace PokeParadise.Classes
{
    public class Environment
    {
        public string weather;
        public string timeOfDay;
        public DateTimeOffset lastSet;
        public DateTimeOffset lastChecked;

        public Environment() { lastSet = DateTimeOffset.MinValue; lastChecked = lastSet; }
    }
}
