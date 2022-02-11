using PokeParadise.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

namespace PokeParadise
{
    public class WalkGoButtonHandler : MonoBehaviour
    {
        public string walkLocation;
        public int id;
        public void Walk()
        {
            string[] typesArray = new string[] { "bug", "dark", "dragon", "electric", "fairy", "fighting", "fire", "flying", "ghost", "grass", "ground", "ice", "normal", "poison", "psychic", "rock", "steel", "water" };
            string[] locations = new string[18];
            GameObject dropdown = GameObject.Find("WalkDropdown");
            int index = dropdown.GetComponent<Dropdown>().value;
            int countr = 0;
            foreach (OptionData option in dropdown.GetComponent<Dropdown>().options)
            {
                locations[countr] = option.text;
                countr++;
            }
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            PlayerData.ClosePanels();
            panels["WalkWrapper"].transform.GetChild(0).gameObject.SetActive(false);
            panels["WalkResultsWrapper"].transform.GetChild(0).gameObject.SetActive(true);
            System.Random rand = new System.Random();
            GameObject panel = GameObject.Find("WalkResultsItemsPanel");
            Player player = PlayerData.FetchCurrentPlayerData();
            Pokemon p = player.pokemon.Find(x => x.id == id);
            List<Item> earned = new List<Item>();
            int maxItems = 3;
            int minItems = 1;
            if (typesArray[index] == p.type1.ToLower() || typesArray[index] == p.type2.ToLower())
            {
                maxItems = 5;
                minItems = 3;
            }
            if (p.friendship >= 25)
            {
                double itemsDouble = maxItems * (1 + (p.friendship / 100));
                maxItems = Convert.ToInt32(Math.Round(itemsDouble));
                minItems = Convert.ToInt32(Math.Round(itemsDouble));
            }
            int amtEarned = rand.Next(minItems, maxItems);
            for (int cnt = 1; cnt <= amtEarned; cnt++)
            {
                int itemChoice = rand.Next(1, 4);
                if (itemChoice == 1)
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
                    Item i = new Item(b, 0, cnt);
                    earned.Add(i);
                }
                else if (itemChoice == 2)
                {
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
                            break;
                        case 2:
                            food = ExternalCookingHandler(foodType, ings[0], ings[1]);
                            break;
                        case 3:
                            food = ExternalCookingHandler(foodType, ings[0], ings[1], ings[2]);
                            break;
                        case 4:
                            food = ExternalCookingHandler(foodType, ings[0], ings[1], ings[2], ings[3]);
                            break;
                    }
                    int id = earned.Count + cnt;
                    Item i = new Item(food, 0, id);
                    earned.Add(i);
                }
                else if (itemChoice == 3)
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
                    Item i = new Item(stone);
                    earned.Add(i);
                }
            }
            int cntr = 1;
            Vector2 centerAnchor = new Vector2(0.5f, 0.5f);
            float x = -125.5f;
            float y = 40f;
            Vector3 initialPosition = new Vector3(x, y);
            foreach (Item i in earned)
            {
                if (GameObject.Find("Earned" + cntr + "Sprite") != null)
                {
                    Destroy(GameObject.Find("Earned" + cntr + "Sprite"));
                }
                GameObject item = new GameObject
                {
                    name = "Earned" + cntr + "Sprite"
                };
                item.transform.SetParent(GameObject.Find("WalkResultsItemsPanel").transform);
                string name = "";
                string itemName = "";
                if (i.food?.category == "Pokeblock")
                {
                    if (i.name.Contains("+"))
                    {
                        name = i.food.category.ToLower() + "_" + i.name.ToLower().Replace("+", "_plus");
                    }
                    else
                    {
                        name = i.food.category.ToLower() + "_" + i.name.ToLower();
                    }
                    item.AddComponent<Image>().sprite = Resources.Load<Sprite>("Food/" + name);
                    itemName = i.food.type + " " + i.food.category;
                }
                else if (i.food?.category == "Poffin")
                {
                    name = i.food.category + "_" + i.name;
                    item.AddComponent<Image>().sprite = Resources.Load<Sprite>("Food/" + name);
                    itemName = i.food.type + " " + i.food.category;
                }
                else if (i.food?.category == "Pokepuff")
                {
                    name = i.food.category.ToLower() + "_" + i.name.Substring(0, i.name.IndexOf(" ")).ToLower();
                    string flavors = i.name.Substring(i.name.IndexOf(" ") + 1, i.name.Length - (i.name.Substring(0, i.name.IndexOf(" ")).Length + 1));
                    if (i.name.Contains("-"))
                    {
                        name += "_" + flavors.Substring(0, flavors.IndexOf("-")).ToLower();
                    }
                    else
                    {
                        name += "_" + flavors.ToLower();
                    }
                    item.AddComponent<Image>().sprite = Resources.Load<Sprite>("Food/" + name);
                    itemName = i.food.type + " " + i.food.category;
                }
                else if (i.berry != null)
                {
                    name = i.name.ToLower();
                    item.AddComponent<Image>().sprite = Resources.Load<Sprite>("Food/" + name);
                    itemName = i.name + " Berry";
                }
                else
                {
                    name = "pokeball";
                    item.AddComponent<Image>().sprite = Resources.Load<Sprite>(name);
                    itemName = i.name;
                }
                item.transform.SetParent(panel.transform, false);
                item.transform.localScale = new Vector3(0.3f, 0.3f);
                RectTransform itemRect = item.GetComponent<RectTransform>();
                itemRect.anchorMin = centerAnchor;
                itemRect.anchorMax = centerAnchor;
                item.transform.localPosition = new Vector3(x, y);
                item.AddComponent<BoxCollider2D>().size = new Vector2(100f, 100f);
                item.AddComponent<WalkItemHoverHandler>().tooltipString = itemName;
                if (cntr == 1)
                {
                    item.transform.localPosition = initialPosition;
                    x += 35;
                }
                else if (cntr % 8 == 0)
                {
                    item.transform.localPosition = new Vector3(x, y);
                    x = 20f;
                    y -= 35;
                }
                else
                {
                    item.transform.localPosition = new Vector3(x, y);
                    x += 35;
                }
                cntr++;
                if (player.inventory.Count == 0)
                {
                    i.id = 1;
                }
                else
                {
                    i.id = player.inventory.Max(x => x.id) + 1;
                }
                player.inventory.Add(i);
            }
            foreach (Pokemon pkmn in player.pokemon)
            {
                if (pkmn.id == id)
                {
                    if (typesArray[index] == p.type1.ToLower() || typesArray[index] == p.type2.ToLower())
                    {
                        pkmn.friendship += 3;
                    }
                    else
                    {
                        pkmn.friendship++;
                    }
                    if (string.IsNullOrEmpty(pkmn.lastWalked))
                    {
                        pkmn.lastWalked = DateTimeOffset.MinValue.ToString();
                    }
                    if (DateTimeOffset.Now - DateTimeOffset.Parse(pkmn.lastWalked) >= new TimeSpan(1, 0, 0))
                    {
                        pkmn.lastWalked = DateTimeOffset.Now.ToString();
                    }
                    int atNext = PlayerData.XpAtNextLevel(pkmn);
                    pkmn.xp += PlayerData.BaseXpGained(pkmn);
                    if (pkmn.xp >= atNext)
                    {
                        panels["LevelAnnounceWrapper"].transform.GetChild(0).gameObject.SetActive(true);
                        pkmn.level++;
                        GameObject.Find("LevelAnnounceText").GetComponent<Text>().text = $"Your {pkmn.pkmnName} has leveled up! It's now level {pkmn.level}!";
                        int evoDexNo = PlayerData.EvoCheck(pkmn, "", locations[index]);
                        if (evoDexNo > 0)
                        {
                            Pokemon evolveTo = new Pokemon(evoDexNo);
                            panels["EvoAnnounceWrapper"].transform.GetChild(0).gameObject.SetActive(true);
                            GameObject.Find("EvoAnnounceText").GetComponent<Text>().text = $"Your {pkmn.pkmnName} has evolved into a {evolveTo.pkmnName}!";
                            pkmn.dexNo = evolveTo.dexNo;
                            pkmn.pkmnName = evolveTo.pkmnName;
                            pkmn.type1 = evolveTo.type1;
                            pkmn.type2 = evolveTo.type2;
                            pkmn.speciesAbility1 = evolveTo.speciesAbility1;
                            pkmn.speciesAbility2 = evolveTo.speciesAbility2;
                            pkmn.baseStatTotal = evolveTo.baseStatTotal;
                            pkmn.baseHP = evolveTo.baseHP;
                            pkmn.baseAtk = evolveTo.baseAtk;
                            pkmn.baseDef = evolveTo.baseDef;
                            pkmn.baseSpAtk = evolveTo.baseSpAtk;
                            pkmn.baseSpDef = evolveTo.baseSpDef;
                            pkmn.baseSpd = evolveTo.baseSpd;
                            pkmn.legendStatus = evolveTo.legendStatus;
                            pkmn.mythicStatus = evolveTo.mythicStatus;
                            pkmn.eggGroup1 = evolveTo.eggGroup1;
                            pkmn.eggGroup2 = evolveTo.eggGroup2;
                            pkmn.levelSpeed = evolveTo.levelSpeed;
                            pkmn.genderThreshhold = evolveTo.genderThreshhold;
                            pkmn.blurb = evolveTo.blurb;
                            pkmn.evolutions = evolveTo.evolutions;
                            foreach (Pokemon pk in player.shownPokemon)
                            {
                                if (pk != null && pk.id == pkmn.id)
                                {
                                    pk.dexNo = evolveTo.dexNo;
                                    pk.pkmnName = evolveTo.pkmnName;
                                    pk.type1 = evolveTo.type1;
                                    pk.type2 = evolveTo.type2;
                                    pk.speciesAbility1 = evolveTo.speciesAbility1;
                                    pk.speciesAbility2 = evolveTo.speciesAbility2;
                                    pk.baseStatTotal = evolveTo.baseStatTotal;
                                    pk.baseHP = evolveTo.baseHP;
                                    pk.baseAtk = evolveTo.baseAtk;
                                    pk.baseDef = evolveTo.baseDef;
                                    pk.baseSpAtk = evolveTo.baseSpAtk;
                                    pk.baseSpDef = evolveTo.baseSpDef;
                                    pk.baseSpd = evolveTo.baseSpd;
                                    pk.legendStatus = evolveTo.legendStatus;
                                    pk.mythicStatus = evolveTo.mythicStatus;
                                    pk.eggGroup1 = evolveTo.eggGroup1;
                                    pk.eggGroup2 = evolveTo.eggGroup2;
                                    pk.levelSpeed = evolveTo.levelSpeed;
                                    pk.genderThreshhold = evolveTo.genderThreshhold;
                                    pk.blurb = evolveTo.blurb;
                                    pk.evolutions = evolveTo.evolutions;
                                }
                            }
                            PlayerData.LoadPokemonMenuSprites(player.shownPokemon);
                        }
                    }
                    pkmn.walksThisHour++;
                }
            }
            PlayerData.SavePlayerData(player);
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
                return new Food("Pokepuff", pokepuffType, 0, flavorProfile, ingredients, 1);
            }
            return food;
        }
    }
}
