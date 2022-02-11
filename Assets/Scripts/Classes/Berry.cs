using System;
using System.IO;

namespace PokeParadise.Classes
{
    [Serializable]
    public class Berry
    {
        public int number { get; set; }
        public string name { get; set; }
        public string poffin { get; set; }
        public int spicyValue { get; set; }
        public int dryValue { get; set; }
        public int sweetValue { get; set; }
        public int bitterValue { get; set; }
        public int sourValue { get; set; }
        public string color { get; set; }
        public decimal plusChance { get; set; }
        public int smoothness { get; set; }
        public int qty { get; set; }

        public Berry() { }

        public Berry(string name)
        {
            Berry berry = new Berry();
            foreach (Berry b in PlayerData.FetchBerries())
            {
                if (b.name == name)
                {
                    berry = b;
                }
            }
            this.name = berry.name;
            number = berry.number;
            plusChance = berry.plusChance;
            poffin = berry.poffin;
            spicyValue = berry.spicyValue;
            dryValue = berry.dryValue;
            sweetValue = berry.sweetValue;
            bitterValue = berry.bitterValue;
            sourValue = berry.sourValue;
            color = berry.color;
        }

        public Berry(int number)
        {
            Berry[] berries = PlayerData.FetchBerries();
            Berry berry = berries[number];
            name = berry.name;
            this.number = berry.number;
            plusChance = berry.plusChance;
            poffin = berry.poffin;
            spicyValue = berry.spicyValue;
            dryValue = berry.dryValue;
            sweetValue = berry.sweetValue;
            bitterValue = berry.bitterValue;
            sourValue = berry.sourValue;
            color = berry.color;
        }
    }
}
