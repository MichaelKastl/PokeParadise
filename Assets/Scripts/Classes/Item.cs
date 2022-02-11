using System;

namespace PokeParadise.Classes
{
    [Serializable]
    public class Item
    {
        public int id { get; set; }
        public string name { get; set; }
        public Food food { get; set; }
        public Berry berry { get; set; }
        public Egg egg { get; set; }
        public int price { get; set; }
        public int shopId { get; set; }

        public Item(string name) { this.name = name; }
        public Item(int price, string name, int shopId) { this.name = name; this.price = price; this.shopId = shopId; }
        public Item(Food food, int price, int shopId) { this.food = food; name = food.type; this.price = price; this.shopId = shopId; }
        public Item(Food food, int id) { this.food = food; name = food.type; this.id = id; }
        public Item(Berry berry, int price, int shopId) { this.berry = berry; name = berry.name; this.price = price; this.shopId = shopId; }
        public Item(Egg egg, int shopId)
        {
            this.egg = egg;
            int price = 0;
            if (egg.pkmn.baseStatTotal <= 200)
            {
                price = egg.pkmn.baseStatTotal / 2;
            }
            else if (egg.pkmn.baseStatTotal <= 350)
            {
                price = Convert.ToInt32(Math.Round(egg.pkmn.baseStatTotal / 1.25));
            }
            else if (egg.pkmn.baseStatTotal <= 450)
            {
                price = egg.pkmn.baseStatTotal;
            }
            else if (egg.pkmn.baseStatTotal <= 500)
            {
                price = egg.pkmn.baseStatTotal * 2;
            }
            else if (egg.pkmn.baseStatTotal <= 550)
            {
                price = Convert.ToInt32(Math.Round(egg.pkmn.baseStatTotal * 2.25));
            }
            else if (egg.pkmn.baseStatTotal <= 600)
            {
                price = Convert.ToInt32(Math.Round(egg.pkmn.baseStatTotal * 2.5));
            }
            else if (egg.pkmn.baseStatTotal <= 650)
            {
                price = egg.pkmn.baseStatTotal * 3;
            }
            else if (egg.pkmn.baseStatTotal > 650)
            {
                price = egg.pkmn.baseStatTotal * 4;
            }
            this.price = price;
            this.shopId = shopId;
            name = "Egg";
        }
        public Item() { }
    }
}
