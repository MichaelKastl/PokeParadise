using System;
using System.Collections.Generic;
using System.Text;

namespace PokeParadise.Classes
{
    [Serializable]
    public class Dusty
    {
        public List<Item> inventory;
        public DateTimeOffset lastRestock;

        public Dusty() { }
        public Dusty(List<Item> inventory, DateTimeOffset lastRestock) { this.inventory = inventory; this.lastRestock = lastRestock; }
    }
}
