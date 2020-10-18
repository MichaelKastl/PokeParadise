using System;
using System.Collections.Generic;
using System.Text;

namespace PokeParadise.Classes
{
    [Serializable]
    public class Item
    {
        public string name { get; set; }
        public Food food { get; set; }
        public Berry berry { get; set; }
        public int qty { get; set; }
        public int price { get; set; }
        public int shopId { get; set; }

        public Item(string name, int qty) { this.name = name; this.qty = qty; }
        public Item(int price, string name, int shopId) { this.name = name; this.price = price; this.shopId = shopId; }
        public Item(Food food, int price, int shopId) { this.food = food; name = food.type; this.price = price; this.shopId = shopId; }
        public Item(Berry berry, int price, int shopId) { this.berry = berry; name = berry.name; this.price = price; this.shopId = shopId; }
        public Item() { }
    }
}
