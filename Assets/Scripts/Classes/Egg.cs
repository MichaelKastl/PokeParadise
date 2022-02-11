using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PokeParadise
{
    [Serializable]
    public class Egg
    {
        public Pokemon pkmn;
        public string timeToHatch;
        public string timeToBreed;
        public bool isIncubating;
        public string obtained;
        public string bred;
        public bool parentsStay;
        public int eggId;

        public Egg()
        {
            pkmn = new Pokemon
            {
                isBreeding = false
            };
        }

        public Egg(Pokemon p, DateTimeOffset obtained, int eggId)
        {
            pkmn = p;
            this.obtained = obtained.ToString();
            pkmn.isBreeding = false;
            System.Random rand = new System.Random();
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
            switch (rand.Next(0, 24))
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
            for (int cnt = 1; cnt <= 6; cnt++)
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
            switch (pkmn.nature)
            {
                case "bashful":
                    pkmn.favoriteFlavor = "none";
                    pkmn.hatedFlavor = "none";
                    break;
                case "adamant":
                    pkmn.favoriteFlavor = "spicy";
                    pkmn.hatedFlavor = "dry";
                    break;
                case "brave":
                    pkmn.favoriteFlavor = "spicy";
                    pkmn.hatedFlavor = "sweet";
                    break;
                case "naughty":
                    pkmn.favoriteFlavor = "spicy";
                    pkmn.hatedFlavor = "bitter";
                    break;
                case "lonely":
                    pkmn.favoriteFlavor = "spicy";
                    pkmn.hatedFlavor = "sour";
                    break;
                case "modest":
                    pkmn.favoriteFlavor = "dry";
                    pkmn.hatedFlavor = "spicy";
                    break;
                case "docile":
                    pkmn.favoriteFlavor = "none";
                    pkmn.hatedFlavor = "none";
                    break;
                case "quiet":
                    pkmn.favoriteFlavor = "dry";
                    pkmn.hatedFlavor = "sweet";
                    break;
                case "rash":
                    pkmn.favoriteFlavor = "dry";
                    pkmn.hatedFlavor = "bitter";
                    break;
                case "mild":
                    pkmn.favoriteFlavor = "dry";
                    pkmn.hatedFlavor = "sour";
                    break;
                case "timid":
                    pkmn.favoriteFlavor = "sweet";
                    pkmn.hatedFlavor = "spicy";
                    break;
                case "jolly":
                    pkmn.favoriteFlavor = "sweet";
                    pkmn.hatedFlavor = "dry";
                    break;
                case "hardy":
                    pkmn.favoriteFlavor = "none";
                    pkmn.hatedFlavor = "none";
                    break;
                case "naive":
                    pkmn.favoriteFlavor = "sweet";
                    pkmn.hatedFlavor = "bitter";
                    break;
                case "hasty":
                    pkmn.favoriteFlavor = "sweet";
                    pkmn.hatedFlavor = "sour";
                    break;
                case "calm":
                    pkmn.favoriteFlavor = "bitter";
                    pkmn.hatedFlavor = "spicy";
                    break;
                case "careful":
                    pkmn.favoriteFlavor = "bitter";
                    pkmn.hatedFlavor = "dry";
                    break;
                case "sassy":
                    pkmn.favoriteFlavor = "bitter";
                    pkmn.hatedFlavor = "sweet";
                    break;
                case "quirky":
                    pkmn.favoriteFlavor = "none";
                    pkmn.hatedFlavor = "none";
                    break;
                case "gentle":
                    pkmn.favoriteFlavor = "bitter";
                    pkmn.hatedFlavor = "sour";
                    break;
                case "bold":
                    pkmn.favoriteFlavor = "sour";
                    pkmn.hatedFlavor = "sweet";
                    break;
                case "impish":
                    pkmn.favoriteFlavor = "sour";
                    pkmn.hatedFlavor = "dry";
                    break;
                case "relaxed":
                    pkmn.favoriteFlavor = "sour";
                    pkmn.hatedFlavor = "sweet";
                    break;
                case "lax":
                    pkmn.favoriteFlavor = "sour";
                    pkmn.hatedFlavor = "bitter";
                    break;
                case "serious":
                    pkmn.favoriteFlavor = "none";
                    pkmn.hatedFlavor = "none";
                    break;
            }
            pkmn.statHP = Convert.ToInt32(Math.Floor(((2 * pkmn.baseHP) + pkmn.hpIv) * pkmn.level / 100m) + pkmn.level + 10);
            pkmn.statAtk = Convert.ToInt32(Math.Floor((((2 * pkmn.baseAtk) + pkmn.atkIv) * pkmn.level / 100m) + 5) * atkMult);
            pkmn.statDef = Convert.ToInt32(Math.Floor((((2 * pkmn.baseDef) + pkmn.defIv) * pkmn.level / 100m) + 5) * defMult);
            pkmn.statSpd = Convert.ToInt32(Math.Floor((((2 * pkmn.baseSpd) + pkmn.spdIv) * pkmn.level / 100m) + 5) * spdMult);
            pkmn.statSpAtk = Convert.ToInt32(Math.Floor((((2 * pkmn.baseSpAtk) + pkmn.spAtkIv) * pkmn.level / 100m) + 5) * spAtkMult);
            pkmn.statSpDef = Convert.ToInt32(Math.Floor((((2 * pkmn.baseSpDef) + pkmn.spDefIv) * pkmn.level / 100m) + 5) * spDefMult);
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
            timeToHatch = new TimeSpan(0, 0, seconds).ToString();
        }

        public Egg(DateTimeOffset obtained, int eggId)
        {
            this.obtained = obtained.ToString();
            Pokemon pkmn;
            System.Random rand = new System.Random();
            int speciesChoice = rand.Next(1, 810);
            while (speciesChoice == 0)
            {
                speciesChoice = rand.Next(1, 810);
            }
            pkmn = new Pokemon(speciesChoice);
            if (pkmn.evolutions.Count > 0)
            {
                bool hasPrevo = false;
                Pokemon prevo = new Pokemon();
                foreach (Evolution evo in pkmn.evolutions)
                {
                    if (!string.IsNullOrEmpty(evo.evolveFrom))
                    {
                        hasPrevo = true;
                        prevo = new Pokemon(evo.evolveFrom);
                    }
                }
                if (hasPrevo)
                {
                    bool prevoHasPrevo = false;
                    foreach (Evolution evo in prevo.evolutions)
                    {
                        if (!string.IsNullOrEmpty(evo.evolveFrom))
                        {
                            prevoHasPrevo = true;
                            pkmn = new Pokemon(evo.evolveFrom);
                        }
                    }
                    if (!prevoHasPrevo)
                    {
                        pkmn = new Pokemon(prevo.dexNo);
                    }
                }
                else
                {
                    pkmn = new Pokemon(pkmn.dexNo);
                }
            }
            else
            {
                pkmn = new Pokemon(pkmn.dexNo);
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
                    speciesChoice = rand.Next(1, 810);
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
                    speciesChoice = rand.Next(1, 810);
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
            switch (rand.Next(0, 24))
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
            for (int cnt = 1; cnt <= 6; cnt++)
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
            switch (pkmn.nature)
            {
                case "bashful":
                    pkmn.favoriteFlavor = "none";
                    pkmn.hatedFlavor = "none";
                    break;
                case "adamant":
                    pkmn.favoriteFlavor = "spicy";
                    pkmn.hatedFlavor = "dry";
                    break;
                case "brave":
                    pkmn.favoriteFlavor = "spicy";
                    pkmn.hatedFlavor = "sweet";
                    break;
                case "naughty":
                    pkmn.favoriteFlavor = "spicy";
                    pkmn.hatedFlavor = "bitter";
                    break;
                case "lonely":
                    pkmn.favoriteFlavor = "spicy";
                    pkmn.hatedFlavor = "sour";
                    break;
                case "modest":
                    pkmn.favoriteFlavor = "dry";
                    pkmn.hatedFlavor = "spicy";
                    break;
                case "docile":
                    pkmn.favoriteFlavor = "none";
                    pkmn.hatedFlavor = "none";
                    break;
                case "quiet":
                    pkmn.favoriteFlavor = "dry";
                    pkmn.hatedFlavor = "sweet";
                    break;
                case "rash":
                    pkmn.favoriteFlavor = "dry";
                    pkmn.hatedFlavor = "bitter";
                    break;
                case "mild":
                    pkmn.favoriteFlavor = "dry";
                    pkmn.hatedFlavor = "sour";
                    break;
                case "timid":
                    pkmn.favoriteFlavor = "sweet";
                    pkmn.hatedFlavor = "spicy";
                    break;
                case "jolly":
                    pkmn.favoriteFlavor = "sweet";
                    pkmn.hatedFlavor = "dry";
                    break;
                case "hardy":
                    pkmn.favoriteFlavor = "none";
                    pkmn.hatedFlavor = "none";
                    break;
                case "naive":
                    pkmn.favoriteFlavor = "sweet";
                    pkmn.hatedFlavor = "bitter";
                    break;
                case "hasty":
                    pkmn.favoriteFlavor = "sweet";
                    pkmn.hatedFlavor = "sour";
                    break;
                case "calm":
                    pkmn.favoriteFlavor = "bitter";
                    pkmn.hatedFlavor = "spicy";
                    break;
                case "careful":
                    pkmn.favoriteFlavor = "bitter";
                    pkmn.hatedFlavor = "dry";
                    break;
                case "sassy":
                    pkmn.favoriteFlavor = "bitter";
                    pkmn.hatedFlavor = "sweet";
                    break;
                case "quirky":
                    pkmn.favoriteFlavor = "none";
                    pkmn.hatedFlavor = "none";
                    break;
                case "gentle":
                    pkmn.favoriteFlavor = "bitter";
                    pkmn.hatedFlavor = "sour";
                    break;
                case "bold":
                    pkmn.favoriteFlavor = "sour";
                    pkmn.hatedFlavor = "sweet";
                    break;
                case "impish":
                    pkmn.favoriteFlavor = "sour";
                    pkmn.hatedFlavor = "dry";
                    break;
                case "relaxed":
                    pkmn.favoriteFlavor = "sour";
                    pkmn.hatedFlavor = "sweet";
                    break;
                case "lax":
                    pkmn.favoriteFlavor = "sour";
                    pkmn.hatedFlavor = "bitter";
                    break;
                case "serious":
                    pkmn.favoriteFlavor = "none";
                    pkmn.hatedFlavor = "none";
                    break;
            }
            pkmn.statHP = Convert.ToInt32(Math.Floor(((2 * pkmn.baseHP) + pkmn.hpIv) * pkmn.level / 100m) + pkmn.level + 10);
            pkmn.statAtk = Convert.ToInt32(Math.Floor((((2 * pkmn.baseAtk) + pkmn.atkIv) * pkmn.level / 100m) + 5) * atkMult);
            pkmn.statDef = Convert.ToInt32(Math.Floor((((2 * pkmn.baseDef) + pkmn.defIv) * pkmn.level / 100m) + 5) * defMult);
            pkmn.statSpd = Convert.ToInt32(Math.Floor((((2 * pkmn.baseSpd) + pkmn.spdIv) * pkmn.level / 100m) + 5) * spdMult);
            pkmn.statSpAtk = Convert.ToInt32(Math.Floor((((2 * pkmn.baseSpAtk) + pkmn.spAtkIv) * pkmn.level / 100m) + 5) * spAtkMult);
            pkmn.statSpDef = Convert.ToInt32(Math.Floor((((2 * pkmn.baseSpDef) + pkmn.spDefIv) * pkmn.level / 100m) + 5) * spDefMult);
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
            timeToHatch = new TimeSpan(0, 0, seconds).ToString();
        }

        public Egg(int dexNo, DateTimeOffset obtained)
        {
            this.obtained = obtained.ToString();
            Pokemon pkmn;
            System.Random rand = new System.Random();
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
            switch (rand.Next(0, 24))
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
            for (int cnt = 1; cnt <= 6; cnt++)
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
            switch (pkmn.nature)
            {
                case "bashful":
                    pkmn.favoriteFlavor = "none";
                    pkmn.hatedFlavor = "none";
                    break;
                case "adamant":
                    pkmn.favoriteFlavor = "spicy";
                    pkmn.hatedFlavor = "dry";
                    break;
                case "brave":
                    pkmn.favoriteFlavor = "spicy";
                    pkmn.hatedFlavor = "sweet";
                    break;
                case "naughty":
                    pkmn.favoriteFlavor = "spicy";
                    pkmn.hatedFlavor = "bitter";
                    break;
                case "lonely":
                    pkmn.favoriteFlavor = "spicy";
                    pkmn.hatedFlavor = "sour";
                    break;
                case "modest":
                    pkmn.favoriteFlavor = "dry";
                    pkmn.hatedFlavor = "spicy";
                    break;
                case "docile":
                    pkmn.favoriteFlavor = "none";
                    pkmn.hatedFlavor = "none";
                    break;
                case "quiet":
                    pkmn.favoriteFlavor = "dry";
                    pkmn.hatedFlavor = "sweet";
                    break;
                case "rash":
                    pkmn.favoriteFlavor = "dry";
                    pkmn.hatedFlavor = "bitter";
                    break;
                case "mild":
                    pkmn.favoriteFlavor = "dry";
                    pkmn.hatedFlavor = "sour";
                    break;
                case "timid":
                    pkmn.favoriteFlavor = "sweet";
                    pkmn.hatedFlavor = "spicy";
                    break;
                case "jolly":
                    pkmn.favoriteFlavor = "sweet";
                    pkmn.hatedFlavor = "dry";
                    break;
                case "hardy":
                    pkmn.favoriteFlavor = "none";
                    pkmn.hatedFlavor = "none";
                    break;
                case "naive":
                    pkmn.favoriteFlavor = "sweet";
                    pkmn.hatedFlavor = "bitter";
                    break;
                case "hasty":
                    pkmn.favoriteFlavor = "sweet";
                    pkmn.hatedFlavor = "sour";
                    break;
                case "calm":
                    pkmn.favoriteFlavor = "bitter";
                    pkmn.hatedFlavor = "spicy";
                    break;
                case "careful":
                    pkmn.favoriteFlavor = "bitter";
                    pkmn.hatedFlavor = "dry";
                    break;
                case "sassy":
                    pkmn.favoriteFlavor = "bitter";
                    pkmn.hatedFlavor = "sweet";
                    break;
                case "quirky":
                    pkmn.favoriteFlavor = "none";
                    pkmn.hatedFlavor = "none";
                    break;
                case "gentle":
                    pkmn.favoriteFlavor = "bitter";
                    pkmn.hatedFlavor = "sour";
                    break;
                case "bold":
                    pkmn.favoriteFlavor = "sour";
                    pkmn.hatedFlavor = "sweet";
                    break;
                case "impish":
                    pkmn.favoriteFlavor = "sour";
                    pkmn.hatedFlavor = "dry";
                    break;
                case "relaxed":
                    pkmn.favoriteFlavor = "sour";
                    pkmn.hatedFlavor = "sweet";
                    break;
                case "lax":
                    pkmn.favoriteFlavor = "sour";
                    pkmn.hatedFlavor = "bitter";
                    break;
                case "serious":
                    pkmn.favoriteFlavor = "none";
                    pkmn.hatedFlavor = "none";
                    break;
            }
            pkmn.statHP = Convert.ToInt32(Math.Floor(((2 * pkmn.baseHP) + pkmn.hpIv) * pkmn.level / 100m) + pkmn.level + 10);
            pkmn.statAtk = Convert.ToInt32(Math.Floor((((2 * pkmn.baseAtk) + pkmn.atkIv) * pkmn.level / 100m) + 5) * atkMult);
            pkmn.statDef = Convert.ToInt32(Math.Floor((((2 * pkmn.baseDef) + pkmn.defIv) * pkmn.level / 100m) + 5) * defMult);
            pkmn.statSpd = Convert.ToInt32(Math.Floor((((2 * pkmn.baseSpd) + pkmn.spdIv) * pkmn.level / 100m) + 5) * spdMult);
            pkmn.statSpAtk = Convert.ToInt32(Math.Floor((((2 * pkmn.baseSpAtk) + pkmn.spAtkIv) * pkmn.level / 100m) + 5) * spAtkMult);
            pkmn.statSpDef = Convert.ToInt32(Math.Floor((((2 * pkmn.baseSpDef) + pkmn.spDefIv) * pkmn.level / 100m) + 5) * spDefMult);
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
            timeToHatch = new TimeSpan(0, 0, seconds).ToString();
        }

        public Egg(Pokemon parent1, Pokemon parent2, DateTimeOffset obtained, int eggId)
        {
            this.obtained = obtained.ToString();
            Pokemon pkmn = new Pokemon();
            System.Random rand = new System.Random();
            pkmn.isBreeding = false;
            switch (rand.Next(1, 6))
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
            switch (rand.Next(1, 6))
            {
                case 1:
                    pkmn.nature = parent1.nature;
                    break;
                case 2:
                    if (parent1.parent1?.nature != null)
                    {
                        pkmn.nature = parent1.parent1.nature;
                    }
                    else
                    {
                        pkmn.nature = parent1.nature;
                    }
                    break;
                case 3:
                    if (parent1.parent2?.nature != null)
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
                    if (parent2.parent1?.nature != null)
                    {
                        pkmn.nature = parent2.parent1.nature;
                    }
                    else
                    {
                        pkmn.nature = parent2.nature;
                    }
                    break;
                case 6:
                    if (parent2.parent2?.nature != null)
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
            switch (pkmn.nature)
            {
                case "bashful":
                    pkmn.favoriteFlavor = "none";
                    pkmn.hatedFlavor = "none";
                    break;
                case "adamant":
                    pkmn.favoriteFlavor = "spicy";
                    pkmn.hatedFlavor = "dry";
                    break;
                case "brave":
                    pkmn.favoriteFlavor = "spicy";
                    pkmn.hatedFlavor = "sweet";
                    break;
                case "naughty":
                    pkmn.favoriteFlavor = "spicy";
                    pkmn.hatedFlavor = "bitter";
                    break;
                case "lonely":
                    pkmn.favoriteFlavor = "spicy";
                    pkmn.hatedFlavor = "sour";
                    break;
                case "modest":
                    pkmn.favoriteFlavor = "dry";
                    pkmn.hatedFlavor = "spicy";
                    break;
                case "docile":
                    pkmn.favoriteFlavor = "none";
                    pkmn.hatedFlavor = "none";
                    break;
                case "quiet":
                    pkmn.favoriteFlavor = "dry";
                    pkmn.hatedFlavor = "sweet";
                    break;
                case "rash":
                    pkmn.favoriteFlavor = "dry";
                    pkmn.hatedFlavor = "bitter";
                    break;
                case "mild":
                    pkmn.favoriteFlavor = "dry";
                    pkmn.hatedFlavor = "sour";
                    break;
                case "timid":
                    pkmn.favoriteFlavor = "sweet";
                    pkmn.hatedFlavor = "spicy";
                    break;
                case "jolly":
                    pkmn.favoriteFlavor = "sweet";
                    pkmn.hatedFlavor = "dry";
                    break;
                case "hardy":
                    pkmn.favoriteFlavor = "none";
                    pkmn.hatedFlavor = "none";
                    break;
                case "naive":
                    pkmn.favoriteFlavor = "sweet";
                    pkmn.hatedFlavor = "bitter";
                    break;
                case "hasty":
                    pkmn.favoriteFlavor = "sweet";
                    pkmn.hatedFlavor = "sour";
                    break;
                case "calm":
                    pkmn.favoriteFlavor = "bitter";
                    pkmn.hatedFlavor = "spicy";
                    break;
                case "careful":
                    pkmn.favoriteFlavor = "bitter";
                    pkmn.hatedFlavor = "dry";
                    break;
                case "sassy":
                    pkmn.favoriteFlavor = "bitter";
                    pkmn.hatedFlavor = "sweet";
                    break;
                case "quirky":
                    pkmn.favoriteFlavor = "none";
                    pkmn.hatedFlavor = "none";
                    break;
                case "gentle":
                    pkmn.favoriteFlavor = "bitter";
                    pkmn.hatedFlavor = "sour";
                    break;
                case "bold":
                    pkmn.favoriteFlavor = "sour";
                    pkmn.hatedFlavor = "sweet";
                    break;
                case "impish":
                    pkmn.favoriteFlavor = "sour";
                    pkmn.hatedFlavor = "dry";
                    break;
                case "relaxed":
                    pkmn.favoriteFlavor = "sour";
                    pkmn.hatedFlavor = "sweet";
                    break;
                case "lax":
                    pkmn.favoriteFlavor = "sour";
                    pkmn.hatedFlavor = "bitter";
                    break;
                case "serious":
                    pkmn.favoriteFlavor = "none";
                    pkmn.hatedFlavor = "none";
                    break;
            }
            pkmn.statHP = Convert.ToInt32(Math.Floor(((2 * pkmn.baseHP) + pkmn.hpIv) * pkmn.level / 100m) + pkmn.level + 10);
            pkmn.statAtk = Convert.ToInt32(Math.Floor((((2 * pkmn.baseAtk) + pkmn.atkIv) * pkmn.level / 100m) + 5) * atkMult);
            pkmn.statDef = Convert.ToInt32(Math.Floor((((2 * pkmn.baseDef) + pkmn.defIv) * pkmn.level / 100m) + 5) * defMult);
            pkmn.statSpd = Convert.ToInt32(Math.Floor((((2 * pkmn.baseSpd) + pkmn.spdIv) * pkmn.level / 100m) + 5) * spdMult);
            pkmn.statSpAtk = Convert.ToInt32(Math.Floor((((2 * pkmn.baseSpAtk) + pkmn.spAtkIv) * pkmn.level / 100m) + 5) * spAtkMult);
            pkmn.statSpDef = Convert.ToInt32(Math.Floor((((2 * pkmn.baseSpDef) + pkmn.spDefIv) * pkmn.level / 100m) + 5) * spDefMult);
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
            timeToHatch = new TimeSpan(0, 0, seconds).ToString();
        }

        public Egg(Player breeder, int eggId, DateTimeOffset bred, Pokemon parent1, Pokemon parent2, bool parentsStay)
        {
            Egg e = new Egg(parent1, parent2, bred, eggId)
            {
                isIncubating = true
            };
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
            timeToBreed = new TimeSpan(0, 0, Convert.ToInt32(Math.Round(TimeSpan.Parse(e.timeToHatch).TotalSeconds * (1 - percentage)))).ToString();
            pkmn = e.pkmn;
            timeToHatch = e.timeToHatch;
            this.bred = DateTimeOffset.Now.ToString();
            this.eggId = eggId;
            this.parentsStay = parentsStay;
            pkmn.isBreeding = false;
            pkmn.parent1 = parent1;
            pkmn.parent2 = parent2;
        }

        public Pokemon RandomFromEggGroup(Pokemon input)
        {
            List<Pokemon> group = new List<Pokemon>();
            foreach (Pokemon x in PlayerData.FetchPokedex())
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
            System.Random rand = new System.Random();
            int indexOfChoice = rand.Next(0, group.Count);
            return group[indexOfChoice];
        }
    }
}
