using System;
using System.Collections.Generic;
using System.Text;

namespace PokeParadise.Classes
{
    [Serializable]
    public class Food
    {
        public string category { get; set; }
        public string type { get; set; }
        public Dictionary<string, int> flavorProfile;
        public int smoothness { get; set; }
        public List<Berry> ingredients;
        public int level { get; set; }
        public int qty;

        public Food(string category, string type, int level, Dictionary<string, int> flavorProfile, List<Berry> ingredients, int qty)
        {
            this.category = category;
            this.type = type;
            this.level = level;
            this.flavorProfile = flavorProfile;
            smoothness = 0;
            this.qty = qty;
            this.ingredients = ingredients;
        }

        public Food(string category, string type, int level, Dictionary<string, int> flavorProfile, int smoothness, List<Berry> ingredients, int qty)
        {
            this.category = category;
            this.type = type;
            this.level = level;
            this.flavorProfile = flavorProfile;
            this.smoothness = smoothness;
            this.qty = qty;
            this.ingredients = ingredients;
        }

        public Food() { }
    }
}
