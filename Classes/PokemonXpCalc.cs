using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PokeParadise.Classes
{
    public class PokemonXpCalc
    {
        public int xpToLevel;
        public int xpRemainingToLevel;
        public int xpAtNextLevel;
        public int baseXpGained;

        public PokemonXpCalc(Pokemon p)
        {
            xpToLevel = XpToLevel(p);
            xpRemainingToLevel = XpRemainingToLevel(p);
            xpAtNextLevel = XpAtNextLevel(p);
            baseXpGained = BaseXpGained(p);
        }

        public int XpToLevel(Pokemon p)
        {
            int xpToLevel = 0;
            using (var reader = new StreamReader(@"C:\Users\mkast\source\repos\PokeParadise\bin\Debug\netcoreapp3.1\xpToLevel.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (p.levelSpeed == values[0])
                    {
                        xpToLevel = Convert.ToInt32(values[p.level]);
                    }
                }
            }
            return xpToLevel;
        }

        public int XpRemainingToLevel(Pokemon p)
        {
            int xpRemainingToLevel = 0;
            using (var reader = new StreamReader(@"C:\Users\mkast\source\repos\PokeParadise\bin\Debug\netcoreapp3.1\xpAtLevel.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (p.levelSpeed == values[0])
                    {
                        xpRemainingToLevel = Convert.ToInt32(values[p.level + 1]) - p.xp;
                    }
                }
            }
            return xpRemainingToLevel;
        }

        public int XpAtNextLevel(Pokemon p)
        {
            int xpAtLevel = 0;
            using (var reader = new StreamReader(@"C:\Users\mkast\source\repos\PokeParadise\bin\Debug\netcoreapp3.1\xpAtLevel.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (p.levelSpeed == values[0])
                    {
                        xpAtLevel = Convert.ToInt32(values[p.level + 1]);
                    }
                }
            }
            return xpAtLevel;
        }

        public int BaseXpGained(Pokemon p)
        {
            int xpGained;
            int xpToLevel = XpToLevel(p);
            double xpPercent = 0;
            if (p.baseStatTotal <= 200)
            {
                xpPercent = .3;
            }
            else if (p.baseStatTotal <= 350)
            {
                xpPercent = .25;
            }
            else if (p.baseStatTotal <= 450)
            {
                xpPercent = .2;
            }
            else if (p.baseStatTotal <= 500)
            {
                xpPercent = .175;
            }
            else if (p.baseStatTotal <= 550)
            {
                xpPercent = .15;
            }
            else if (p.baseStatTotal <= 600)
            {
                xpPercent = .125;
            }
            else if (p.baseStatTotal <= 650)
            {
                xpPercent = .1;
            }
            else if (p.baseStatTotal > 650)
            {
                xpPercent = .075;
            }
            Random rand = new Random();
            double multiplier = 0;
            bool validMult = false;
            while (!validMult)
            {
                multiplier = rand.NextDouble();
                if (multiplier >= 0.025 && multiplier <= xpPercent)
                {
                    validMult = true;
                }
            }
            xpGained = Convert.ToInt32(Math.Ceiling(multiplier * xpToLevel));
            return xpGained;
        }
    }
}
