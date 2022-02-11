using System.Collections.Generic;
using UnityEngine;
using PokeParadise.Classes;
using System.Linq;
using System;
using UnityEngine.UI;

namespace PokeParadise
{
    public static class KitchenHandler
    {
        public static void Cook(List<Item> ings, string foodType)
        {
            Player player = PlayerData.FetchCurrentPlayerData();
            List<Berry> ingredients = new List<Berry>();
            foreach (Item i in ings)
            {
                player.inventory.RemoveAll(x => x.id == i.id);
                ingredients.Add(i.berry);
            }
            System.Random rand = new System.Random();
            if (foodType == "Poffin")
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
                int smoothnessTotal = 0;
                int ingTotal = ingredients.Count;
                int foodSmoothness;
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
                    smoothnessTotal += ing.smoothness;
                }
                if (berryNames.Distinct().Count() == 4)
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
                else if (flavors == 2)
                {
                    flavor = "Rich";
                }
                else if (flavors == 3)
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
                foodSmoothness = (smoothnessTotal / ingTotal) - ingTotal;
                Food poffin = new Food("Poffin", flavor, flavorLevel, flavorProfile, foodSmoothness, ingredients, 1);
                int id = 1;
                if (player.inventory.Count > 0)
                {
                    id = player.inventory.Max(x => x.id) + 1;
                }
                player.inventory.Add(new Item(poffin, id));
                PlayerData.SavePlayerData(player);
                Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
                panels["CookingResultWrapper"].transform.GetChild(0).gameObject.SetActive(true);
                panels["KitchenWrapper"].transform.GetChild(0).gameObject.SetActive(false);
                ClearFood();
                foreach (Transform obj in panels["CookingResultWrapper"].transform.GetChild(0).transform)
                {
                    if (obj.gameObject.name == "CookingResultText")
                    {
                        obj.gameObject.GetComponent<Text>().text = "You've made a " + poffin.type + " " + poffin.category + "!";
                    }
                }
            }
            else if (foodType == "Pokeblock")
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
                int smoothnessTotal = 0;
                int ingTotal = ingredients.Count;
                int foodSmoothness;
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
                    smoothnessTotal += ing.smoothness;
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
                else
                {
                    color = "Rainbow";
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
                foodSmoothness = (smoothnessTotal / ingTotal) - ingTotal;
                decimal rollForPlus = Convert.ToDecimal(rand.NextDouble());
                if (rollForPlus <= highestPercentage) { pokeblockType += "+"; }
                Food pokeblock = new Food("Pokeblock", pokeblockType, 0, flavorProfile, foodSmoothness, ingredients, 1);
                int id = 1;
                if (player.inventory.Count > 0)
                {
                    id = player.inventory.Max(x => x.id) + 1;
                }
                player.inventory.Add(new Item(pokeblock, id));
                PlayerData.SavePlayerData(player);
                Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
                panels["CookingResultWrapper"].transform.GetChild(0).gameObject.SetActive(true);
                panels["KitchenWrapper"].transform.GetChild(0).gameObject.SetActive(false);
                ClearFood();
                foreach (Transform obj in panels["CookingResultWrapper"].transform.GetChild(0).transform)
                {
                    if (obj.gameObject.name == "CookingResultText")
                    {
                        obj.gameObject.GetComponent<Text>().text = "You've made a " + pokeblock.type + " " + pokeblock.category + "!";
                    }
                }
            }
            else if (foodType == "Pokepuff")
            {
                // Sweet = sweet, Mint = bitter, Citrus = sour, Mocha = dry, and Spice = spicy. Basic, frosted, fancy, deluxe, supreme = puff types.
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
                    flavorTotals.Remove(primaryFlavor);
                    flavor = primaryFlavor;
                    if (flavorTotals.Values.Max() > 0)
                    {
                        foreach (KeyValuePair<string, int> kvp in flavorTotals)
                            if (kvp.Value == flavorTotals.Values.Max()) { flavor += "-" + kvp.Key; }
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
                Food pokepuff = new Food("Pokepuff", pokepuffType, 0, flavorProfile, ingredients, 1);
                int id = 1;
                if (player.inventory.Count > 0)
                {
                    id = player.inventory.Max(x => x.id) + 1;
                }
                player.inventory.Add(new Item(pokepuff, id));
                PlayerData.SavePlayerData(player);
                Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
                panels["CookingResultWrapper"].transform.GetChild(0).gameObject.SetActive(true);
                panels["KitchenWrapper"].transform.GetChild(0).gameObject.SetActive(false);
                ClearFood();
                foreach (Transform obj in panels["CookingResultWrapper"].transform.GetChild(0).transform)
                {
                    if (obj.gameObject.name == "CookingResultText")
                    {
                        obj.gameObject.GetComponent<Text>().text = "You've made a " + pokepuff.type + " " + pokepuff.category + "!";
                    }
                }
            }
        }

        public static void ClearFood()
        {
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x.name.Contains("BerrySprite")))
            {
                UnityEngine.Object.Destroy(obj);
            }
        }
    }
}