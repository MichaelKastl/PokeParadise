using PokeParadise.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using static System.Environment;

namespace PokeParadise
{
    public class Dusty
    {
        public List<Item> inventory;
        public string lastRestock;

        public Dusty()
        {
        }

        public Dusty(List<Item> inventory)
        {
            this.inventory = inventory;
            lastRestock = DateTimeOffset.Now.ToString();
        }

        public void SaveShop()
        {
            string appData = Application.persistentDataPath;
            if (!Directory.Exists($"{appData}/PokeParadise_Shop"))
            {
                Directory.CreateDirectory($"{appData}/PokeParadise_Shop");
            }
            XmlSerializer x = new XmlSerializer(GetType());
            File.WriteAllText($"{appData}/PokeParadise_Shop/currentShop.xml", "");
            using FileStream fs = new FileStream($"{appData}/PokeParadise_Shop/currentShop.xml", FileMode.OpenOrCreate);
            x.Serialize(fs, this);
        }

        public static Dusty LoadShop()
        {
            Dusty dusty = new Dusty();
            XmlSerializer x = new XmlSerializer(dusty.GetType());
            string appData = Application.persistentDataPath;
            if (!Directory.Exists($"{appData}/PokeParadise_Shop"))
            {
                Directory.CreateDirectory($"{appData}/PokeParadise_Shop");
            }
            if (File.Exists($"{ appData }/PokeParadise_Shop/currentShop.xml"))
            {
                using FileStream fs = new FileStream($"{ appData }/PokeParadise_Shop/currentShop.xml", FileMode.OpenOrCreate);
                dusty = (Dusty)x.Deserialize(fs);
            }
            TimeSpan dustyCooldown = new TimeSpan(0, 3, 0, 0);
            const bool forceReset = false;
            if (dusty.inventory == null || DateTimeOffset.Now - DateTimeOffset.Parse(dusty.lastRestock) > dustyCooldown || forceReset)
            {
                dusty.inventory = new List<Item>();
                System.Random rand = new System.Random();
                dusty.inventory.Add(new Item(new Egg(DateTimeOffset.Now, -1), 1));
                int amtBerries = rand.Next(1, 5);
                int amtFood = rand.Next(1, 3);
                int amtItems = rand.Next(1, 2);
                int cnt = 1;
                while (cnt <= amtBerries)
                {
                    Berry b = new Berry(rand.Next(1, 64));
                    List<int> flavorValues = new List<int>
                    {
                        b.sourValue,
                        b.sweetValue,
                        b.bitterValue,
                        b.dryValue,
                        b.spicyValue
                    };
                    int berryPrice = 10 * flavorValues.Max();
                    Item i = new Item(b, berryPrice, cnt);
                    dusty.inventory.Add(i);
                    cnt++;
                }
                cnt = 1;
                while (cnt <= amtFood)
                {
                    int price = 0;
                    int foodChoice = rand.Next(1, 4);
                    string foodType = null;
                    switch (foodChoice)
                    {
                        case 1:
                            foodType = "Poffin";
                            break;
                        case 2:
                            foodType = "Pokeblock";
                            break;
                        case 3:
                            foodType = "Pokepuff";
                            break;
                    }
                    int ingRoll = rand.Next(1, 13);
                    int ingAmt = 0;
                    switch (ingRoll)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                            ingAmt = 1;
                            break;
                        case 7:
                        case 8:
                        case 9:
                            ingAmt = 2;
                            break;
                        case 10:
                        case 11:
                            ingAmt = 3;
                            break;
                        case 12:
                            ingAmt = 1;
                            break;
                    }
                    int ingCnt = 1;
                    List<string> ings = new List<string>();
                    while (ingCnt <= ingAmt)
                    {
                        Berry b = new Berry(rand.Next(1, 65));
                        ings.Add(b.name);
                        ingCnt++;
                    }
                    Food food = new Food();
                    switch (ings.Count)
                    {
                        case 1:
                            food = ExternalCookingHandler(foodType, ings[0]);
                            if (food.flavorProfile.Max() > 0)
                            {
                                price = 10 * food.flavorProfile.Max();
                            }
                            else
                            { price = 10; }
                            break;
                        case 2:
                            food = ExternalCookingHandler(foodType, ings[0], ings[1]);
                            price = 15 * food.flavorProfile.Max();
                            break;
                        case 3:
                            food = ExternalCookingHandler(foodType, ings[0], ings[1], ings[2]);
                            price = 25 * food.flavorProfile.Max();
                            break;
                        case 4:
                            food = ExternalCookingHandler(foodType, ings[0], ings[1], ings[2], ings[3]);
                            price = 50 * food.flavorProfile.Max();
                            break;
                    }
                    int id = dusty.inventory.Count + cnt;
                    Item i = new Item(food, price, id);
                    dusty.inventory.Add(i);
                    cnt++;
                }
                cnt = 1;
                while (cnt <= amtItems)
                {
                    int stoneChoice = rand.Next(1, 33);
                    string stone = null;
                    switch (stoneChoice)
                    {
                        case 1: stone = "Dawn Stone"; break;
                        case 2: stone = "Dusk Stone"; break;
                        case 3: stone = "Fire Stone"; break;
                        case 4: stone = "Ice Stone"; break;
                        case 5: stone = "Leaf Stone"; break;
                        case 6: stone = "Moon Stone"; break;
                        case 7: stone = "Shiny Stone"; break;
                        case 8: stone = "Sun Stone"; break;
                        case 9: stone = "Thunder Stone"; break;
                        case 10: stone = "Water Stone"; break;
                        case 11: stone = "Prism Scale"; break;
                        case 12: stone = "Deep Sea Scale"; break;
                        case 13: stone = "Deep Sea Tooth"; break;
                        case 14: stone = "Dragon Scale"; break;
                        case 15: stone = "Dubious Disc"; break;
                        case 16: stone = "Electirizer"; break;
                        case 17: stone = "King's Rock"; break;
                        case 18: stone = "Magmarizer"; break;
                        case 19: stone = "Metal Coat"; break;
                        case 20: stone = "Oval Stone"; break;
                        case 21: stone = "Prism Scale"; break;
                        case 22: stone = "Protector"; break;
                        case 23: stone = "Razor Claw"; break;
                        case 24: stone = "Razor Fang"; break;
                        case 25: stone = "Reaper Cloth"; break;
                        case 26: stone = "Sachet"; break;
                        case 27: stone = "Upgrade"; break;
                        case 28: stone = "Whipped Dream"; break;
                        case 29: stone = "Upside-Down Button"; break;
                        case 30: stone = "Rare Candy"; break;
                        case 31: stone = "Sachet"; break;
                        case 32: stone = "Rainmaker"; break;
                    }
                    int id = dusty.inventory.Count + cnt;
                    Item i = new Item(3000, stone, id);
                    dusty.inventory.Add(i);
                    cnt++;
                }
                dusty.lastRestock = DateTimeOffset.Now.ToString();
                dusty.SaveShop();
            }
            return dusty;
        }

        public static Food ExternalCookingHandler(string cookingType, string ingredient1, string ingredient2 = null, string ingredient3 = null, string ingredient4 = null)
        {
            Food food = new Food();
            ingredient1 = ingredient1.Substring(0, 1).ToUpper() + ingredient1.Substring(1);
            if (ingredient2 != null)
            {
                ingredient2 = ingredient2.Substring(0, 1).ToUpper() + ingredient2.Substring(1);
            }
            if (ingredient3 != null)
            {
                ingredient3 = ingredient3.Substring(0, 1).ToUpper() + ingredient3.Substring(1);
            }
            if (ingredient4 != null)
            {
                ingredient4 = ingredient4.Substring(0, 1).ToUpper() + ingredient4.Substring(1);
            }
            List<Berry> ingredients = new List<Berry>();
            Berry b = new Berry(ingredient1);
            ingredients.Add(b);
            if (ingredient2 != null)
            {
                b = new Berry(ingredient2);
                ingredients.Add(b);
            }
            if (ingredient3 != null)
            {
                b = new Berry(ingredient3);
                ingredients.Add(b);
            }
            if (ingredient4 != null)
            {
                b = new Berry(ingredient4);
                ingredients.Add(b);
            }
            System.Random rand = new System.Random();
            if (cookingType == "Poffin")
            {
                string poffinType = null;
                int sweetCount = 0;
                int spicyCount = 0;
                int bitterCount = 0;
                int sourCount = 0;
                int dryCount = 0;
                int spicyTotal = 0;
                int sourTotal = 0;
                int bitterTotal = 0;
                int sweetTotal = 0;
                int dryTotal = 0;
                int penalty = 0;
                List<string> berryNames = new List<string>();
                foreach (Berry ing in ingredients)
                {
                    sweetCount += ing.sweetValue;
                    spicyCount += ing.spicyValue;
                    bitterCount += ing.bitterValue;
                    sourCount += ing.sourValue;
                    dryCount += ing.dryValue;
                    spicyTotal = spicyCount;
                    sourTotal = sourCount;
                    bitterTotal = bitterCount;
                    sweetTotal = sweetCount;
                    dryTotal = dryCount;
                    berryNames.Add(ing.name);
                }
                if (berryNames.Count == berryNames.Distinct().Count())
                {
                    poffinType = "Foul";
                }
                spicyTotal -= dryCount;
                if (spicyTotal < 0) { penalty++; }
                sourTotal -= spicyCount;
                if (sourTotal < 0) { penalty++; }
                bitterTotal -= sourCount;
                if (bitterTotal < 0) { penalty++; }
                sweetTotal -= bitterCount;
                if (sweetTotal < 0) { penalty++; }
                dryTotal -= sweetCount;
                if (dryTotal < 0) { penalty++; }
                spicyTotal -= penalty;
                if (spicyTotal < 0) { spicyTotal = 0; }
                sourTotal -= penalty;
                if (sourTotal < 0) { sourTotal = 0; }
                bitterTotal -= penalty;
                if (bitterTotal < 0) { bitterTotal = 0; }
                sweetTotal -= penalty;
                if (sweetTotal < 0) { sweetTotal = 0; }
                dryTotal -= penalty;
                if (dryTotal < 0) { dryTotal = 0; }
                Dictionary<string, int> flavorTotals = new Dictionary<string, int>();
                int[] flavorProfile = new int[5];
                flavorTotals.Add("Spicy", spicyTotal);
                flavorTotals.Add("Sour", sourTotal);
                flavorTotals.Add("Bitter", bitterTotal);
                flavorTotals.Add("Sweet", sweetTotal);
                flavorTotals.Add("Dry", dryTotal);
                int flavorLevel = flavorTotals.Values.Max();
                int flavors = 0;
                int cnt = 0;
                foreach (KeyValuePair<string, int> flavorTotal in flavorTotals)
                {
                    if (flavorTotal.Value > 0)
                    {
                        flavors++;
                    }
                    flavorProfile[cnt] = flavorTotal.Value;
                    cnt++;
                }
                string flavor = null;
                if (flavors <= 2)
                {
                    string primaryFlavor = null;
                    foreach (KeyValuePair<string, int> kvp in flavorTotals)
                    {
                        if (kvp.Value == flavorLevel) { primaryFlavor = kvp.Key; }
                    }
                    flavorTotals.Remove(flavorTotals.Keys.Max());
                    flavor = primaryFlavor;
                    if (flavorTotals.Values.Max() > 0)
                    {
                        foreach (KeyValuePair<string, int> kvp in flavorTotals)
                        {
                            if (kvp.Value == flavorLevel) { flavor = "-" + kvp.Key; }
                        }
                    }
                }
                else if (flavors == 3)
                {
                    flavor = "Rich";
                }
                else if (flavors == 4)
                {
                    flavor = "Overripe";
                }
                if (flavorLevel >= 50)
                {
                    flavor = "Mild";
                }
                if (poffinType == "Foul")
                {
                    flavor = "Foul";
                }
                return new Food("Poffin", flavor, flavorLevel, flavorProfile, ingredients, 1);
            }
            else if (cookingType == "Pokeblock")
            {
                int[] flavorProfile = new int[5];
                string pokeblockType;
                int redCount = 0;
                int blueCount = 0;
                int greenCount = 0;
                int yellowCount = 0;
                int pinkCount = 0;
                int sweetCount = 0;
                int sourCount = 0;
                int dryCount = 0;
                int bitterCount = 0;
                int spicyCount = 0;
                Dictionary<string, int> colorCounts = new Dictionary<string, int>();
                foreach (Berry ing in ingredients)
                {
                    switch (ing.color)
                    {
                        case "Red": redCount++; break;
                        case "Blue": blueCount++; break;
                        case "Green": greenCount++; break;
                        case "Yellow": yellowCount++; break;
                        case "Pink": pinkCount++; break;
                    }
                    sweetCount += ing.sweetValue;
                    sourCount += ing.sourValue;
                    bitterCount += ing.bitterValue;
                    dryCount += ing.dryValue;
                    spicyCount += ing.spicyValue;
                }
                int spicyTotal = spicyCount;
                int bitterTotal = bitterCount;
                int dryTotal = dryCount;
                int sourTotal = sourCount;
                int sweetTotal = sweetCount;
                int penalty = 0;
                spicyTotal -= dryCount;
                if (spicyTotal < 0) { penalty++; }
                sourTotal -= spicyCount;
                if (sourTotal < 0) { penalty++; }
                bitterTotal -= sourCount;
                if (bitterTotal < 0) { penalty++; }
                sweetTotal -= bitterCount;
                if (sweetTotal < 0) { penalty++; }
                dryTotal -= sweetCount;
                if (dryTotal < 0) { penalty++; }
                for (int cnt = 1; cnt <= penalty; cnt++)
                {
                    spicyTotal -= penalty;
                    if (spicyTotal < 0) { spicyTotal = 0; }
                    sweetTotal -= penalty;
                    if (sweetTotal < 0) { sweetTotal = 0; }
                    sourTotal -= penalty;
                    if (sourTotal < 0) { sourTotal = 0; }
                    dryTotal -= penalty;
                    if (dryTotal < 0) { dryTotal = 0; }
                    bitterTotal -= penalty;
                    if (bitterTotal < 0) { bitterTotal = 0; }
                }
                flavorProfile[0] = spicyTotal;
                flavorProfile[1] = sourTotal;
                flavorProfile[2] = bitterTotal;
                flavorProfile[3] = sweetTotal;
                flavorProfile[4] = dryTotal;
                colorCounts.Add("Red", redCount);
                colorCounts.Add("Blue", blueCount);
                colorCounts.Add("Green", greenCount);
                colorCounts.Add("Yellow", yellowCount);
                colorCounts.Add("Pink", pinkCount);
                int distinctColors = 0;
                foreach (KeyValuePair<string, int> kvp in colorCounts)
                { if (kvp.Value > 0) { distinctColors++; } }
                string color = null;
                if (distinctColors <= 3)
                {
                    Dictionary<string, int> mostColors = new Dictionary<string, int>();
                    foreach (KeyValuePair<string, int> kvp in colorCounts)
                    { if (kvp.Value == colorCounts.Values.Max()) { mostColors.Add(kvp.Key, kvp.Value); } }
                    if (mostColors.Count == 1)
                    { color = mostColors.First().Key; }
                    else
                    {
                        int selected = rand.Next(1, mostColors.Count);
                        KeyValuePair<string, int> selection = mostColors.ElementAt(selected - 1);
                        color = selection.Key;
                    }
                }
                decimal highestPercentage = 0;
                foreach (Berry ing in ingredients)
                {
                    if (ing.color == color)
                    {
                        if (ing.plusChance > highestPercentage) { highestPercentage = ing.plusChance; }
                    }
                }
                pokeblockType = color;
                decimal rollForPlus = Convert.ToDecimal(rand.NextDouble());
                if (rollForPlus <= highestPercentage) { pokeblockType += "+"; }
                return new Food("Pokeblock", pokeblockType, 0, flavorProfile, ingredients, 1);
            }
            else if (cookingType == "Pokepuff")
            {
                string pokepuffType;
                int sweetCount = 0;
                int spicyCount = 0;
                int bitterCount = 0;
                int sourCount = 0;
                int dryCount = 0;
                int spicyTotal = 0;
                int sourTotal = 0;
                int bitterTotal = 0;
                int sweetTotal = 0;
                int dryTotal = 0;
                int penalty = 0;
                foreach (Berry ing in ingredients)
                {
                    sweetCount += ing.sweetValue;
                    spicyCount += ing.spicyValue;
                    bitterCount += ing.bitterValue;
                    sourCount += ing.sourValue;
                    dryCount += ing.dryValue;
                    spicyTotal = spicyCount;
                    sourTotal = sourCount;
                    bitterTotal = bitterCount;
                    sweetTotal = sweetCount;
                    dryTotal = dryCount;
                }
                spicyTotal -= dryCount;
                if (spicyTotal < 0) { penalty++; }
                sourTotal -= spicyCount;
                if (sourTotal < 0) { penalty++; }
                bitterTotal -= sourCount;
                if (bitterTotal < 0) { penalty++; }
                sweetTotal -= bitterCount;
                if (sweetTotal < 0) { penalty++; }
                dryTotal -= sweetCount;
                if (dryTotal < 0) { penalty++; }
                spicyTotal -= penalty;
                if (spicyTotal < 0) { spicyTotal = 0; }
                sourTotal -= penalty;
                if (sourTotal < 0) { sourTotal = 0; }
                bitterTotal -= penalty;
                if (bitterTotal < 0) { bitterTotal = 0; }
                sweetTotal -= penalty;
                if (sweetTotal < 0) { sweetTotal = 0; }
                dryTotal -= penalty;
                if (dryTotal < 0) { dryTotal = 0; }
                Dictionary<string, int> flavorTotals = new Dictionary<string, int>();
                int[] flavorProfile = new int[5];
                flavorTotals.Add("Spicy", spicyTotal);
                flavorTotals.Add("Sour", sourTotal);
                flavorTotals.Add("Bitter", bitterTotal);
                flavorTotals.Add("Sweet", sweetTotal);
                flavorTotals.Add("Dry", dryTotal);
                int flavorLevel = flavorTotals.Values.Max();
                int flavors = 0;
                int cnt = 0;
                foreach (KeyValuePair<string, int> flavorTotal in flavorTotals)
                {
                    if (flavorTotal.Value > 0)
                    {
                        flavors++;
                    }
                    flavorProfile[cnt] = flavorTotal.Value;
                    cnt++;
                }
                string flavor = null;
                if (flavors <= 2)
                {
                    string primaryFlavor = null;
                    foreach (KeyValuePair<string, int> kvp in flavorTotals)
                    {
                        if (kvp.Value == flavorLevel) { primaryFlavor = kvp.Key; }
                    }
                    foreach (string f in flavorTotals.Keys.Where(x => x == primaryFlavor).ToList())
                    {
                        flavorTotals.Remove(f);
                    }
                    flavor = primaryFlavor;
                    if (flavorTotals.Values.Max() > 0)
                    {
                        foreach (KeyValuePair<string, int> kvp in flavorTotals)
                        {
                            if (kvp.Value == flavorLevel) { flavor += "-" + kvp.Key; }
                        }
                    }
                }
                else if (flavors == 3) { flavor = "Robust"; }
                else if (flavors == 4) { flavor = "Overwhelming"; }
                string level = null;
                if (flavorLevel < 10) { level = "Basic"; }
                else if (flavorLevel < 25) { level = "Frosted"; }
                else if (flavorLevel < 35) { level = "Fancy"; }
                else if (flavorLevel < 50) { level = "Deluxe"; }
                else if (flavorLevel >= 50) { level = "Supreme"; }
                pokepuffType = level + " " + flavor;
                return new Food("Pokepuff", pokepuffType, 0, flavorProfile, ingredients, 1);
            }
            return food;
        }
    }
}