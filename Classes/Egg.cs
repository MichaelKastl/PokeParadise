using Microsoft.VisualBasic.FileIO;
using PokeParadise.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace PokeParadise
{
    [Serializable]
    public class Egg
    {
        public Pokemon pkmn;
        public TimeSpan timeToHatch;
        public TimeSpan timeToBreed;
        public DateTimeOffset obtained;
        public DateTimeOffset bred;
        public bool parentsStay;
        public int eggId;

        public Egg()
        {
            Pokemon pkmn = new Pokemon
            {
                isBreeding = false
            };
            this.pkmn = pkmn;
        }

        public Egg(Pokemon p, DateTimeOffset obtained, int eggId)
        {
            pkmn = p;
            pkmn.isBreeding = false;
            this.obtained = obtained;
            pkmn.isBreeding = false;
            Random rand = new Random();
            pkmn.parent1 = new Pokemon(p.dexNo);
            pkmn.parent2 = RandomFromEggGroup(pkmn);
            int personalityValue = rand.Next(1, 252);
            if (personalityValue >= pkmn.genderThreshhold)
            {
                pkmn.gender = "male";
            }
            else
            {
                pkmn.gender = "female";
            }
            int coinflip = rand.Next(1, 2);
            if (coinflip == 1)
            {
                pkmn.ability = pkmn.speciesAbility1;
            }
            else
            {
                pkmn.ability = pkmn.speciesAbility2;
            }
            int natureChoice = rand.Next(0, 24);
            switch (natureChoice)
            {
                case 0: pkmn.nature = "hardy"; break;
                case 1: pkmn.nature = "lonely"; break;
                case 2: pkmn.nature = "brave"; break;
                case 3: pkmn.nature = "adamant"; break;
                case 4: pkmn.nature = "naughty"; break;
                case 5: pkmn.nature = "bold"; break;
                case 6: pkmn.nature = "docile"; break;
                case 7: pkmn.nature = "relaxed"; break;
                case 8: pkmn.nature = "impish"; break;
                case 9: pkmn.nature = "lax"; break;
                case 10: pkmn.nature = "timid"; break;
                case 11: pkmn.nature = "hasty"; break;
                case 12: pkmn.nature = "serious"; break;
                case 13: pkmn.nature = "jolly"; break;
                case 14: pkmn.nature = "naive"; break;
                case 15: pkmn.nature = "modest"; break;
                case 16: pkmn.nature = "mild"; break;
                case 17: pkmn.nature = "quiet"; break;
                case 18: pkmn.nature = "bashful"; break;
                case 19: pkmn.nature = "rash"; break;
                case 20: pkmn.nature = "calm"; break;
                case 21: pkmn.nature = "gentle"; break;
                case 22: pkmn.nature = "sassy"; break;
                case 23: pkmn.nature = "careful"; break;
                case 24: pkmn.nature = "quirky"; break;
            }
            int cnt = 1;
            while (cnt <= 6)
            {
                switch (cnt)
                {
                    case 1:
                        pkmn.hpIv = rand.Next(0, 31);
                        break;
                    case 2:
                        pkmn.atkIv = rand.Next(0, 31);
                        break;
                    case 3:
                        pkmn.defIv = rand.Next(0, 31);
                        break;
                    case 4:
                        pkmn.spAtkIv = rand.Next(0, 31);
                        break;
                    case 5:
                        pkmn.spDefIv = rand.Next(0, 31);
                        break;
                    case 6:
                        pkmn.spdIv = rand.Next(0, 31);
                        break;
                }
                cnt++;
            }
            decimal atkMult = 1;
            decimal defMult = 1;
            decimal spdMult = 1;
            decimal spAtkMult = 1;
            decimal spDefMult = 1;
            switch (pkmn.nature)
            {
                case "lonely": atkMult = 1.1m; defMult = 0.9m; break;
                case "brave": atkMult = 1.1m; spdMult = 0.9m; break;
                case "adamant": atkMult = 1.1m; spAtkMult = 0.9m; break;
                case "naughty": atkMult = 1.1m; spDefMult = 0.9m; break;
                case "bold": defMult = 1.1m; atkMult = 0.9m; break;
                case "relaxed": defMult = 1.1m; spdMult = 0.9m; break;
                case "impish": defMult = 1.1m; spAtkMult = 0.9m; break;
                case "lax": defMult = 1.1m; spDefMult = 0.9m; break;
                case "timid": spdMult = 1.1m; atkMult = 0.9m; break;
                case "hasty": spdMult = 1.1m; defMult = 0.9m; break;
                case "jolly": spdMult = 1.1m; spAtkMult = 0.9m; break;
                case "naive": spdMult = 1.1m; spDefMult = 0.9m; break;
                case "modest": spAtkMult = 1.1m; atkMult = 0.9m; break;
                case "mild": spAtkMult = 1.1m; defMult = 0.9m; break;
                case "quiet": spAtkMult = 1.1m; spdMult = 0.9m; break;
                case "rash": spAtkMult = 1.1m; spDefMult = 0.9m; break;
                case "calm": spDefMult = 1.1m; atkMult = 0.9m; break;
                case "gentle": spDefMult = 1.1m; defMult = 0.9m; break;
                case "sassy": spDefMult = 1.1m; spdMult = 0.9m; break;
                case "careful": spDefMult = 1.1m; spAtkMult = 0.9m; break;
            }
            pkmn.statHP = Convert.ToInt32(Math.Floor((2 * pkmn.baseHP + pkmn.hpIv) * pkmn.level / 100m) + pkmn.level + 10);
            pkmn.statAtk = Convert.ToInt32(Math.Floor(((2 * pkmn.baseAtk + pkmn.atkIv) * pkmn.level / 100m) + 5) * atkMult);
            pkmn.statDef = Convert.ToInt32(Math.Floor(((2 * pkmn.baseDef + pkmn.defIv) * pkmn.level / 100m) + 5) * defMult);
            pkmn.statSpd = Convert.ToInt32(Math.Floor(((2 * pkmn.baseSpd + pkmn.spdIv) * pkmn.level / 100m) + 5) * spdMult);
            pkmn.statSpAtk = Convert.ToInt32(Math.Floor(((2 * pkmn.baseSpAtk + pkmn.spAtkIv) * pkmn.level / 100m) + 5) * spAtkMult);
            pkmn.statSpDef = Convert.ToInt32(Math.Floor(((2 * pkmn.baseSpDef + pkmn.spDefIv) * pkmn.level / 100m) + 5) * spDefMult);
            this.eggId = eggId;
            int seconds = 0;
            if (pkmn.baseStatTotal <= 200)
            {
                seconds = pkmn.baseStatTotal / 2;
            }
            else if (pkmn.baseStatTotal <= 350)
            {
                seconds = Convert.ToInt32(Math.Round(pkmn.baseStatTotal / 1.25));
            }
            else if (pkmn.baseStatTotal <= 450)
            {
                seconds = pkmn.baseStatTotal;
            }
            else if (pkmn.baseStatTotal <= 500)
            {
                seconds = pkmn.baseStatTotal * 2;
            }
            else if (pkmn.baseStatTotal <= 550)
            {
                seconds = Convert.ToInt32(Math.Round(pkmn.baseStatTotal * 2.25));
            }
            else if (pkmn.baseStatTotal <= 600)
            {
                seconds = Convert.ToInt32(Math.Round(pkmn.baseStatTotal * 2.5));
            }
            else if (pkmn.baseStatTotal <= 650)
            {
                seconds = pkmn.baseStatTotal * 3;
            }
            else if (pkmn.baseStatTotal > 650)
            {
                seconds = pkmn.baseStatTotal * 4;
            }
            timeToHatch = new TimeSpan(0, 0, seconds);
        }

        public Egg(DateTimeOffset obtained, int eggId)
        {
            this.obtained = obtained;
            Pokemon pkmn;
            Random rand = new Random();
            int speciesChoice = rand.Next(1, 891);
            pkmn = new Pokemon(speciesChoice);
            if (pkmn.prevEvo != null && pkmn.prevEvo != "" && pkmn.prevEvo != " ")
            {
                Pokemon prevo = new Pokemon(pkmn.prevEvo);
                if (prevo.prevEvo != null && prevo.prevEvo != "" && prevo.prevEvo != " ")
                {
                    pkmn = new Pokemon(prevo.prevEvo);
                }
                else
                {
                    pkmn = new Pokemon(prevo.dexNo);
                }
            }
            pkmn.isBreeding = false;
            if (pkmn.legendStatus)
            {
                int legendConfirm = rand.Next(1, 3);
                if (legendConfirm == 1)
                {
                    pkmn = new Pokemon(speciesChoice);
                }
                else
                {
                    speciesChoice = rand.Next(1, 891);
                    pkmn = new Pokemon(speciesChoice);
                }
            }
            else if (pkmn.mythicStatus)
            {
                int mythicConfirm = rand.Next(1, 2);
                if (mythicConfirm == 1)
                {
                    pkmn = new Pokemon(speciesChoice);
                }
                else
                {
                    speciesChoice = rand.Next(1, 891);
                    pkmn = new Pokemon(speciesChoice);
                }
            }
            pkmn.parent1 = new Pokemon(speciesChoice);
            pkmn.parent2 = RandomFromEggGroup(pkmn);
            int personalityValue = rand.Next(1, 252);
            if (personalityValue >= pkmn.genderThreshhold)
            {
                pkmn.gender = "male";
            }
            else
            {
                pkmn.gender = "female";
            }
            int coinflip = rand.Next(1, 2);
            if (coinflip == 1)
            {
                pkmn.ability = pkmn.speciesAbility1;
            }
            else
            {
                pkmn.ability = pkmn.speciesAbility2;
            }
            int natureChoice = rand.Next(0, 24);
            switch (natureChoice)
            {
                case 0: pkmn.nature = "hardy"; break;
                case 1: pkmn.nature = "lonely"; break;
                case 2: pkmn.nature = "brave"; break;
                case 3: pkmn.nature = "adamant"; break;
                case 4: pkmn.nature = "naughty"; break;
                case 5: pkmn.nature = "bold"; break;
                case 6: pkmn.nature = "docile"; break;
                case 7: pkmn.nature = "relaxed"; break;
                case 8: pkmn.nature = "impish"; break;
                case 9: pkmn.nature = "lax"; break;
                case 10: pkmn.nature = "timid"; break;
                case 11: pkmn.nature = "hasty"; break;
                case 12: pkmn.nature = "serious"; break;
                case 13: pkmn.nature = "jolly"; break;
                case 14: pkmn.nature = "naive"; break;
                case 15: pkmn.nature = "modest"; break;
                case 16: pkmn.nature = "mild"; break;
                case 17: pkmn.nature = "quiet"; break;
                case 18: pkmn.nature = "bashful"; break;
                case 19: pkmn.nature = "rash"; break;
                case 20: pkmn.nature = "calm"; break;
                case 21: pkmn.nature = "gentle"; break;
                case 22: pkmn.nature = "sassy"; break;
                case 23: pkmn.nature = "careful"; break;
                case 24: pkmn.nature = "quirky"; break;
            }
            int cnt = 1;
            while (cnt <= 6)
            {
                switch (cnt)
                {
                    case 1:
                        pkmn.hpIv = rand.Next(0, 31);
                        break;
                    case 2:
                        pkmn.atkIv = rand.Next(0, 31);
                        break;
                    case 3:
                        pkmn.defIv = rand.Next(0, 31);
                        break;
                    case 4:
                        pkmn.spAtkIv = rand.Next(0, 31);
                        break;
                    case 5:
                        pkmn.spDefIv = rand.Next(0, 31);
                        break;
                    case 6:
                        pkmn.spdIv = rand.Next(0, 31);
                        break;
                }
                cnt++;
            }
            decimal atkMult = 1;
            decimal defMult = 1;
            decimal spdMult = 1;
            decimal spAtkMult = 1;
            decimal spDefMult = 1;
            switch (pkmn.nature)
            {
                case "lonely": atkMult = 1.1m; defMult = 0.9m; break;
                case "brave": atkMult = 1.1m; spdMult = 0.9m; break;
                case "adamant": atkMult = 1.1m; spAtkMult = 0.9m; break;
                case "naughty": atkMult = 1.1m; spDefMult = 0.9m; break;
                case "bold": defMult = 1.1m; atkMult = 0.9m; break;
                case "relaxed": defMult = 1.1m; spdMult = 0.9m; break;
                case "impish": defMult = 1.1m; spAtkMult = 0.9m; break;
                case "lax": defMult = 1.1m; spDefMult = 0.9m; break;
                case "timid": spdMult = 1.1m; atkMult = 0.9m; break;
                case "hasty": spdMult = 1.1m; defMult = 0.9m; break;
                case "jolly": spdMult = 1.1m; spAtkMult = 0.9m; break;
                case "naive": spdMult = 1.1m; spDefMult = 0.9m; break;
                case "modest": spAtkMult = 1.1m; atkMult = 0.9m; break;
                case "mild": spAtkMult = 1.1m; defMult = 0.9m; break;
                case "quiet": spAtkMult = 1.1m; spdMult = 0.9m; break;
                case "rash": spAtkMult = 1.1m; spDefMult = 0.9m; break;
                case "calm": spDefMult = 1.1m; atkMult = 0.9m; break;
                case "gentle": spDefMult = 1.1m; defMult = 0.9m; break;
                case "sassy": spDefMult = 1.1m; spdMult = 0.9m; break;
                case "careful": spDefMult = 1.1m; spAtkMult = 0.9m; break;
            }
            pkmn.statHP = Convert.ToInt32(Math.Floor((2 * pkmn.baseHP + pkmn.hpIv) * pkmn.level / 100m) + pkmn.level + 10);
            pkmn.statAtk = Convert.ToInt32(Math.Floor(((2 * pkmn.baseAtk + pkmn.atkIv) * pkmn.level / 100m) + 5) * atkMult);
            pkmn.statDef = Convert.ToInt32(Math.Floor(((2 * pkmn.baseDef + pkmn.defIv) * pkmn.level / 100m) + 5) * defMult);
            pkmn.statSpd = Convert.ToInt32(Math.Floor(((2 * pkmn.baseSpd + pkmn.spdIv) * pkmn.level / 100m) + 5) * spdMult);
            pkmn.statSpAtk = Convert.ToInt32(Math.Floor(((2 * pkmn.baseSpAtk + pkmn.spAtkIv) * pkmn.level / 100m) + 5) * spAtkMult);
            pkmn.statSpDef = Convert.ToInt32(Math.Floor(((2 * pkmn.baseSpDef + pkmn.spDefIv) * pkmn.level / 100m) + 5) * spDefMult);
            this.pkmn = pkmn;
            this.eggId = eggId;
            int seconds = 0;
            if (pkmn.baseStatTotal <= 200)
            {
                seconds = pkmn.baseStatTotal / 2;
            }
            else if (pkmn.baseStatTotal <= 350)
            {
                seconds = Convert.ToInt32(Math.Round(pkmn.baseStatTotal / 1.25));
            }
            else if (pkmn.baseStatTotal <= 450)
            {
                seconds = pkmn.baseStatTotal;
            }
            else if (pkmn.baseStatTotal <= 500)
            {
                seconds = pkmn.baseStatTotal * 2;
            }
            else if (pkmn.baseStatTotal <= 550)
            {
                seconds = Convert.ToInt32(Math.Round(pkmn.baseStatTotal * 2.25));
            }
            else if (pkmn.baseStatTotal <= 600)
            {
                seconds = Convert.ToInt32(Math.Round(pkmn.baseStatTotal * 2.5));
            }
            else if (pkmn.baseStatTotal <= 650)
            {
                seconds = pkmn.baseStatTotal * 3;
            }
            else if (pkmn.baseStatTotal > 650)
            {
                seconds = pkmn.baseStatTotal * 4;
            }
            timeToHatch = new TimeSpan(0, 0, seconds);
        }

        public Egg(int dexNo, DateTimeOffset obtained)
        {
            this.obtained = obtained;
            Pokemon pkmn;
            Random rand = new Random();
            pkmn = new Pokemon(dexNo)
            {
                parent1 = new Pokemon(dexNo)
            };
            pkmn.isBreeding = false;
            pkmn.parent2 = RandomFromEggGroup(pkmn);
            int coinflip = rand.Next(1, 2);
            if (coinflip == 1)
            {
                pkmn.ability = pkmn.speciesAbility1;
            }
            else
            {
                pkmn.ability = pkmn.speciesAbility2;
            }
            int personalityValue = rand.Next(1, 252);
            if (personalityValue >= pkmn.genderThreshhold)
            {
                pkmn.gender = "male";
            }
            else
            {
                pkmn.gender = "female";
            }
            int natureChoice = rand.Next(0, 24);
            switch (natureChoice)
            {
                case 0: pkmn.nature = "hardy"; break;
                case 1: pkmn.nature = "lonely"; break;
                case 2: pkmn.nature = "brave"; break;
                case 3: pkmn.nature = "adamant"; break;
                case 4: pkmn.nature = "naughty"; break;
                case 5: pkmn.nature = "bold"; break;
                case 6: pkmn.nature = "docile"; break;
                case 7: pkmn.nature = "relaxed"; break;
                case 8: pkmn.nature = "impish"; break;
                case 9: pkmn.nature = "lax"; break;
                case 10: pkmn.nature = "timid"; break;
                case 11: pkmn.nature = "hasty"; break;
                case 12: pkmn.nature = "serious"; break;
                case 13: pkmn.nature = "jolly"; break;
                case 14: pkmn.nature = "naive"; break;
                case 15: pkmn.nature = "modest"; break;
                case 16: pkmn.nature = "mild"; break;
                case 17: pkmn.nature = "quiet"; break;
                case 18: pkmn.nature = "bashful"; break;
                case 19: pkmn.nature = "rash"; break;
                case 20: pkmn.nature = "calm"; break;
                case 21: pkmn.nature = "gentle"; break;
                case 22: pkmn.nature = "sassy"; break;
                case 23: pkmn.nature = "careful"; break;
                case 24: pkmn.nature = "quirky"; break;
            }
            int cnt = 1;
            while (cnt <= 6)
            {
                switch (cnt)
                {
                    case 1:
                        pkmn.hpIv = rand.Next(0, 31);
                        break;
                    case 2:
                        pkmn.atkIv = rand.Next(0, 31);
                        break;
                    case 3:
                        pkmn.defIv = rand.Next(0, 31);
                        break;
                    case 4:
                        pkmn.spAtkIv = rand.Next(0, 31);
                        break;
                    case 5:
                        pkmn.spDefIv = rand.Next(0, 31);
                        break;
                    case 6:
                        pkmn.spdIv = rand.Next(0, 31);
                        break;
                }
                cnt++;
            }
            decimal atkMult = 1;
            decimal defMult = 1;
            decimal spdMult = 1;
            decimal spAtkMult = 1;
            decimal spDefMult = 1;
            switch (pkmn.nature)
            {
                case "lonely": atkMult = 1.1m; defMult = 0.9m; break;
                case "brave": atkMult = 1.1m; spdMult = 0.9m; break;
                case "adamant": atkMult = 1.1m; spAtkMult = 0.9m; break;
                case "naughty": atkMult = 1.1m; spDefMult = 0.9m; break;
                case "bold": defMult = 1.1m; atkMult = 0.9m; break;
                case "relaxed": defMult = 1.1m; spdMult = 0.9m; break;
                case "impish": defMult = 1.1m; spAtkMult = 0.9m; break;
                case "lax": defMult = 1.1m; spDefMult = 0.9m; break;
                case "timid": spdMult = 1.1m; atkMult = 0.9m; break;
                case "hasty": spdMult = 1.1m; defMult = 0.9m; break;
                case "jolly": spdMult = 1.1m; spAtkMult = 0.9m; break;
                case "naive": spdMult = 1.1m; spDefMult = 0.9m; break;
                case "modest": spAtkMult = 1.1m; atkMult = 0.9m; break;
                case "mild": spAtkMult = 1.1m; defMult = 0.9m; break;
                case "quiet": spAtkMult = 1.1m; spdMult = 0.9m; break;
                case "rash": spAtkMult = 1.1m; spDefMult = 0.9m; break;
                case "calm": spDefMult = 1.1m; atkMult = 0.9m; break;
                case "gentle": spDefMult = 1.1m; defMult = 0.9m; break;
                case "sassy": spDefMult = 1.1m; spdMult = 0.9m; break;
                case "careful": spDefMult = 1.1m; spAtkMult = 0.9m; break;
            }
            pkmn.statHP = Convert.ToInt32(Math.Floor((2 * pkmn.baseHP + pkmn.hpIv) * pkmn.level / 100m) + pkmn.level + 10);
            pkmn.statAtk = Convert.ToInt32(Math.Floor(((2 * pkmn.baseAtk + pkmn.atkIv) * pkmn.level / 100m) + 5) * atkMult);
            pkmn.statDef = Convert.ToInt32(Math.Floor(((2 * pkmn.baseDef + pkmn.defIv) * pkmn.level / 100m) + 5) * defMult);
            pkmn.statSpd = Convert.ToInt32(Math.Floor(((2 * pkmn.baseSpd + pkmn.spdIv) * pkmn.level / 100m) + 5) * spdMult);
            pkmn.statSpAtk = Convert.ToInt32(Math.Floor(((2 * pkmn.baseSpAtk + pkmn.spAtkIv) * pkmn.level / 100m) + 5) * spAtkMult);
            pkmn.statSpDef = Convert.ToInt32(Math.Floor(((2 * pkmn.baseSpDef + pkmn.spDefIv) * pkmn.level / 100m) + 5) * spDefMult);
            this.pkmn = pkmn;
            int seconds = 0;
            if (pkmn.baseStatTotal <= 200)
            {
                seconds = pkmn.baseStatTotal / 2;
            }
            else if (pkmn.baseStatTotal <= 350)
            {
                seconds = Convert.ToInt32(Math.Round(pkmn.baseStatTotal / 1.25));
            }
            else if (pkmn.baseStatTotal <= 450)
            {
                seconds = pkmn.baseStatTotal;
            }
            else if (pkmn.baseStatTotal <= 500)
            {
                seconds = pkmn.baseStatTotal * 2;
            }
            else if (pkmn.baseStatTotal <= 550)
            {
                seconds = Convert.ToInt32(Math.Round(pkmn.baseStatTotal * 2.25));
            }
            else if (pkmn.baseStatTotal <= 600)
            {
                seconds = Convert.ToInt32(Math.Round(pkmn.baseStatTotal * 2.5));
            }
            else if (pkmn.baseStatTotal <= 650)
            {
                seconds = pkmn.baseStatTotal * 3;
            }
            else if (pkmn.baseStatTotal > 650)
            {
                seconds = pkmn.baseStatTotal * 4;
            }
            timeToHatch = new TimeSpan(0, 0, seconds);
        }

        public Egg(Pokemon parent1, Pokemon parent2, DateTimeOffset obtained, int eggId)
        {
            this.obtained = obtained;
            Pokemon pkmn = new Pokemon();
            Random rand = new Random();
            pkmn.isBreeding = false;
            int speciesChoice = rand.Next(1, 6);
            switch (speciesChoice)
            {
                case 1:
                    pkmn = new Pokemon(parent1.pkmnName);
                    break;
                case 2:
                    pkmn = new Pokemon(parent1.parent1.pkmnName);
                    break;
                case 3:
                    pkmn = new Pokemon(parent1.parent2.pkmnName);
                    break;
                case 4:
                    pkmn = new Pokemon(parent2.pkmnName);
                    break;
                case 5:
                    pkmn = new Pokemon(parent2.parent1.pkmnName);
                    break;
                case 6:
                    pkmn = new Pokemon(parent2.parent2.pkmnName);
                    break;
            }
            int personalityValue = rand.Next(1, 252);
            if (personalityValue >= pkmn.genderThreshhold)
            {
                pkmn.gender = "male";
            }
            else
            {
                pkmn.gender = "female";
            }
            int natureChoice = rand.Next(1, 6);
            switch (natureChoice)
            {
                case 1:
                    pkmn.nature = parent1.nature;
                    break;
                case 2:
                    if (parent1.parent1 != null && parent1.parent1.nature != null)
                    {
                        pkmn.nature = parent1.parent1.nature;
                    }
                    else
                    {
                        pkmn.nature = parent1.nature;
                    }
                    break;
                case 3:
                    if (parent1.parent2 != null && parent1.parent2.nature != null)
                    {
                        pkmn.nature = parent1.parent2.nature;
                    }
                    else
                    {
                        pkmn.nature = parent1.nature;
                    }
                    break;
                case 4:
                    pkmn.nature = parent2.nature;
                    break;
                case 5:
                    if (parent2.parent1 != null && parent2.parent1.nature != null)
                    {
                        pkmn.nature = parent2.parent1.nature;
                    }
                    else
                    {
                        pkmn.nature = parent2.nature;
                    }
                    break;
                case 6:
                    if (parent2.parent2 != null && parent2.parent2.nature != null)
                    {
                        pkmn.nature = parent2.parent2.nature;
                    }
                    else
                    {
                        pkmn.nature = parent2.nature;
                    }
                    break;
            }
            Pokemon mother;
            Pokemon father;
            if (parent1.gender == "female")
            {
                mother = parent1;
                father = parent2;
            }
            else
            {
                mother = parent2;
                father = parent1;
            }
            int shinyChoice = rand.Next(1, 8192);
            if (shinyChoice == 8192)
            {
                pkmn.isShiny = true;
            }
            int abilityChoice = rand.Next(1, 5);
            if (abilityChoice == 5)
            {
                pkmn.ability = father.ability;
            }
            else
            {
                pkmn.ability = mother.ability;
            }
            int cnt = 1;
            List<int> alreadyChosen = new List<int>();
            while (cnt <= 3)
            {
                int statChoice = 0;
                bool validChoice = false;
                while (!validChoice)
                {
                    bool notInAlreadyChosen = true;
                    statChoice = rand.Next(1, 5);
                    foreach (int i in alreadyChosen)
                    {
                        if (statChoice == i)
                        {
                            notInAlreadyChosen = false;
                        }
                    }
                    if (notInAlreadyChosen)
                    {
                        validChoice = true;
                    }
                }
                int parentChoice = rand.Next(1, 2);
                switch (statChoice)
                {
                    case 1:
                        if (parentChoice == 1)
                        {
                            pkmn.atkIv = parent1.atkIv;
                        }
                        else
                        {
                            pkmn.atkIv = parent2.atkIv;
                        }
                        break;
                    case 2:
                        if (parentChoice == 1)
                        {
                            pkmn.defIv = parent1.defIv;
                        }
                        else
                        {
                            pkmn.defIv = parent2.defIv;
                        }
                        break;
                    case 3:
                        if (parentChoice == 1)
                        {
                            pkmn.spdIv = parent1.spdIv;
                        }
                        else
                        {
                            pkmn.spdIv = parent2.spdIv;
                        }
                        break;
                    case 4:
                        if (parentChoice == 1)
                        {
                            pkmn.spAtkIv = parent1.spAtkIv;
                        }
                        else
                        {
                            pkmn.spAtkIv = parent2.spAtkIv;
                        }
                        break;
                    case 5:
                        if (parentChoice == 1)
                        {
                            pkmn.spDefIv = parent1.spDefIv;
                        }
                        else
                        {
                            pkmn.spDefIv = parent2.spDefIv;
                        }
                        break;
                }
                alreadyChosen.Add(statChoice);
                cnt++;
            }
            decimal atkMult = 1;
            decimal defMult = 1;
            decimal spdMult = 1;
            decimal spAtkMult = 1;
            decimal spDefMult = 1;
            switch (pkmn.nature)
            {
                case "lonely": atkMult = 1.1m; defMult = 0.9m; break;
                case "brave": atkMult = 1.1m; spdMult = 0.9m; break;
                case "adamant": atkMult = 1.1m; spAtkMult = 0.9m; break;
                case "naughty": atkMult = 1.1m; spDefMult = 0.9m; break;
                case "bold": defMult = 1.1m; atkMult = 0.9m; break;
                case "relaxed": defMult = 1.1m; spdMult = 0.9m; break;
                case "impish": defMult = 1.1m; spAtkMult = 0.9m; break;
                case "lax": defMult = 1.1m; spDefMult = 0.9m; break;
                case "timid": spdMult = 1.1m; atkMult = 0.9m; break;
                case "hasty": spdMult = 1.1m; defMult = 0.9m; break;
                case "jolly": spdMult = 1.1m; spAtkMult = 0.9m; break;
                case "naive": spdMult = 1.1m; spDefMult = 0.9m; break;
                case "modest": spAtkMult = 1.1m; atkMult = 0.9m; break;
                case "mild": spAtkMult = 1.1m; defMult = 0.9m; break;
                case "quiet": spAtkMult = 1.1m; spdMult = 0.9m; break;
                case "rash": spAtkMult = 1.1m; spDefMult = 0.9m; break;
                case "calm": spDefMult = 1.1m; atkMult = 0.9m; break;
                case "gentle": spDefMult = 1.1m; defMult = 0.9m; break;
                case "sassy": spDefMult = 1.1m; spdMult = 0.9m; break;
                case "careful": spDefMult = 1.1m; spAtkMult = 0.9m; break;
            }
            pkmn.statHP = Convert.ToInt32(Math.Floor((2 * pkmn.baseHP + pkmn.hpIv) * pkmn.level / 100m) + pkmn.level + 10);
            pkmn.statAtk = Convert.ToInt32(Math.Floor(((2 * pkmn.baseAtk + pkmn.atkIv) * pkmn.level / 100m) + 5) * atkMult);
            pkmn.statDef = Convert.ToInt32(Math.Floor(((2 * pkmn.baseDef + pkmn.defIv) * pkmn.level / 100m) + 5) * defMult);
            pkmn.statSpd = Convert.ToInt32(Math.Floor(((2 * pkmn.baseSpd + pkmn.spdIv) * pkmn.level / 100m) + 5) * spdMult);
            pkmn.statSpAtk = Convert.ToInt32(Math.Floor(((2 * pkmn.baseSpAtk + pkmn.spAtkIv) * pkmn.level / 100m) + 5) * spAtkMult);
            pkmn.statSpDef = Convert.ToInt32(Math.Floor(((2 * pkmn.baseSpDef + pkmn.spDefIv) * pkmn.level / 100m) + 5) * spDefMult);
            this.pkmn = pkmn;
            this.eggId = eggId;
            int seconds = 0;
            if (pkmn.baseStatTotal <= 200)
            {
                seconds = pkmn.baseStatTotal / 2;
            }
            else if (pkmn.baseStatTotal <= 350)
            {
                seconds = Convert.ToInt32(Math.Round(pkmn.baseStatTotal / 1.25));
            }
            else if (pkmn.baseStatTotal <= 450)
            {
                seconds = pkmn.baseStatTotal;
            }
            else if (pkmn.baseStatTotal <= 500)
            {
                seconds = pkmn.baseStatTotal * 2;
            }
            else if (pkmn.baseStatTotal <= 550)
            {
                seconds = Convert.ToInt32(Math.Round(pkmn.baseStatTotal * 2.25));
            }
            else if (pkmn.baseStatTotal <= 600)
            {
                seconds = Convert.ToInt32(Math.Round(pkmn.baseStatTotal * 2.5));
            }
            else if (pkmn.baseStatTotal <= 650)
            {
                seconds = pkmn.baseStatTotal * 3;
            }
            else if (pkmn.baseStatTotal > 650)
            {
                seconds = pkmn.baseStatTotal * 4;
            }
            timeToHatch = new TimeSpan(0, 0, seconds);
        }

        public Egg(Player breeder, int eggId, DateTimeOffset bred, Pokemon parent1, Pokemon parent2, bool parentsStay)
        {
            Egg e = new Egg(parent1, parent2, bred, eggId);
            double mult = 2.0;
            if (parent1.hasPoffin)
            {
                mult += 0.5;
            }
            if (parent2.hasPoffin)
            {
                mult += 0.5;
            }
            double percentage = Convert.ToDouble((breeder.breedingTimerMult / 100) * mult);
            if (breeder.breedingTimerMult == 0)
            {
                percentage = 0;
            }
            timeToBreed = new TimeSpan(0, 0, Convert.ToInt32(Math.Round(e.timeToHatch.TotalSeconds * (1 - percentage))));
            pkmn = e.pkmn;
            timeToHatch = e.timeToHatch;
            this.bred = DateTimeOffset.Now;
            this.eggId = eggId;
            this.parentsStay = parentsStay;
            pkmn.isBreeding = false;
            pkmn.parent1 = parent1;
            pkmn.parent2 = parent2;
    }

        public Pokemon RandomFromEggGroup(Pokemon input)
        {
            Pokemon p;
            Pokemon[] pokedex = FetchPokedex();
            List<Pokemon> group = new List<Pokemon>();
            foreach (Pokemon x in pokedex)
            {

                if (x.eggGroup1 == input.eggGroup1 || x.eggGroup2 == input.eggGroup1)
                {
                    group.Add(x);
                }
                else
                {
                    if (input.eggGroup2 != "" && input.eggGroup2 != " ")
                    {
                        if (x.eggGroup1 == input.eggGroup2 || x.eggGroup2 == input.eggGroup2)
                        {
                            group.Add(x);
                        }
                    }
                }
            }
            Random rand = new Random();
            int indexOfChoice = rand.Next(0, group.Count);
            p = group[indexOfChoice];
            return p;
        }

        public Pokemon[] FetchPokedex()
        {
            Pokemon[] pokedex = new Pokemon[891];
            int pokemonCount = 890;
            Pokemon empty = new Pokemon();
            pokedex[0] = empty;
            int cnt = 0;
            using (var reader = new StreamReader(@"C:\Users\mkast\source\repos\PokeParadise\bin\Debug\netcoreapp3.1\pokedata.csv"))
            {
                while (!reader.EndOfStream && cnt < pokemonCount)
                {
                    Pokemon p = new Pokemon();
                    cnt++;
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    string species = values[0];
                    string firstLet = species.Substring(0, 1).ToUpper();
                    species = firstLet + species.Substring(1);
                    p.pkmnName = species;
                    bool validDexNo = Int32.TryParse(values[1], out p.dexNo);
                    p.type1 = values[2];
                    p.type2 = values[3];
                    p.baseStatTotal = Convert.ToInt32(values[4]);
                    p.baseHP = Convert.ToInt32(values[5]);
                    p.baseAtk = Convert.ToInt32(values[6]);
                    p.baseDef = Convert.ToInt32(values[7]);
                    p.baseSpAtk = Convert.ToInt32(values[8]);
                    p.baseSpDef = Convert.ToInt32(values[9]);
                    p.baseSpd = Convert.ToInt32(values[10]);
                    p.eggGroup1 = values[11];
                    p.eggGroup2 = values[12];
                    if (values[13] != "")
                    {
                        p.evoLevel = Convert.ToInt32(values[13]);
                    }
                    else
                    {
                        p.evoLevel = 999;
                    }
                    p.speciesAbility1 = values[14];
                    p.speciesAbility2 = values[15];
                    p.levelSpeed = values[16];
                    p.genderThreshhold = Convert.ToInt32(values[17]);
                    pokedex[p.dexNo] = p;
                }
            }
            return pokedex;
        }
    }
}
