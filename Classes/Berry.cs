using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
            Berry[] berries = FetchBerries();
            foreach (Berry b in berries)
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
            Berry berry = new Berry();
            Berry[] berries = FetchBerries();
            berry = berries[number];
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

        public Berry[] FetchBerries()
        {
            Berry[] berries = new Berry[65];
            berries[0] = new Berry();
            using (var reader = new StreamReader(@"C:\Users\mkast\source\repos\PokeParadise\bin\Debug\netcoreapp3.1\berrydata.csv"))
            {
                int cnt = 0;
                while (!reader.EndOfStream)
                {
                    if (cnt >= 1)
                    { 
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        Berry berry = new Berry();
                        berry.number = Convert.ToInt32(values[0]);
                        berry.name = values[1];
                        berry.poffin = values[2];
                        berry.spicyValue = Convert.ToInt32(values[3]);
                        berry.dryValue = Convert.ToInt32(values[4]);
                        berry.sweetValue = Convert.ToInt32(values[5]);
                        berry.bitterValue = Convert.ToInt32(values[6]);
                        berry.sourValue = Convert.ToInt32(values[7]);
                        berry.color = values[8];
                        berry.plusChance = Convert.ToDecimal(values[9]);
                        berry.smoothness = Convert.ToInt32(values[10]);
                        berries[cnt] = berry;
                    }
                    cnt++;
                }
            }
            return berries;
        }
    }
}
