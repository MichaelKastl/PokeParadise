using UnityEngine;
using PokeParadise.Classes;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace PokeParadise
{
    public class FoodFeedHandler : MonoBehaviour
    {
        public Item item;
        public int id;
        public void Feed()
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            Player player = PlayerData.FetchCurrentPlayerData();
            int startingItemCount = player.inventory.Count;
            foreach (Pokemon p in player.pokemon)
            {
                if (p.id == id)
                {
                    double mult = 1;
                    switch (p.favoriteFlavor)
                    {
                        case "spicy":
                            if (item.food.flavorProfile[0] == item.food.flavorProfile.Max())
                            {
                                mult = 2;
                            }
                            else if (item.food.flavorProfile[0] == item.food.flavorProfile.OrderByDescending(r => r).Skip(1).FirstOrDefault())
                            {
                                mult = 1.5;
                            }
                            break;
                        case "sour":
                            if (item.food.flavorProfile[1] == item.food.flavorProfile.Max())
                            {
                                mult = 2;
                            }
                            else if (item.food.flavorProfile[1] == item.food.flavorProfile.OrderByDescending(r => r).Skip(1).FirstOrDefault())
                            {
                                mult = 1.5;
                            }
                            break;
                        case "bitter":
                            if (item.food.flavorProfile[2] == item.food.flavorProfile.Max())
                            {
                                mult = 2;
                            }
                            else if (item.food.flavorProfile[2] == item.food.flavorProfile.OrderByDescending(r => r).Skip(1).FirstOrDefault())
                            {
                                mult = 1.5;
                            }
                            break;
                        case "sweet":
                            if (item.food.flavorProfile[3] == item.food.flavorProfile.Max())
                            {
                                mult = 2;
                            }
                            else if (item.food.flavorProfile[3] == item.food.flavorProfile.OrderByDescending(r => r).Skip(1).FirstOrDefault())
                            {
                                mult = 1.5;
                            }
                            break;
                        case "dry":
                            if (item.food.flavorProfile[4] == item.food.flavorProfile.Max())
                            {
                                mult = 2;
                            }
                            else if (item.food.flavorProfile[4] == item.food.flavorProfile.OrderByDescending(r => r).Skip(1).FirstOrDefault())
                            {
                                mult = 1.5;
                            }
                            break;
                        case "none":
                            break;
                    }
                    switch (p.hatedFlavor)
                    {
                        case "spicy":
                            if (item.food.flavorProfile[0] == item.food.flavorProfile.Max())
                            {
                                mult--;
                            }
                            else if (item.food.flavorProfile[0] == item.food.flavorProfile.OrderBy(r => r).Skip(1).FirstOrDefault())
                            {
                                mult -= .5;
                            }
                            break;
                        case "sour":
                            if (item.food.flavorProfile[1] == item.food.flavorProfile.Max())
                            {
                                mult--;
                            }
                            else if (item.food.flavorProfile[1] == item.food.flavorProfile.OrderBy(r => r).Skip(1).FirstOrDefault())
                            {
                                mult -= .5;
                            }
                            break;
                        case "bitter":
                            if (item.food.flavorProfile[2] == item.food.flavorProfile.Max())
                            {
                                mult--;
                            }
                            else if (item.food.flavorProfile[2] == item.food.flavorProfile.OrderBy(r => r).Skip(1).FirstOrDefault())
                            {
                                mult -= .5;
                            }
                            break;
                        case "sweet":
                            if (item.food.flavorProfile[3] == item.food.flavorProfile.Max())
                            {
                                mult--;
                            }
                            else if (item.food.flavorProfile[3] == item.food.flavorProfile.OrderBy(r => r).Skip(1).FirstOrDefault())
                            {
                                mult -= .5;
                            }
                            break;
                        case "dry":
                            if (item.food.flavorProfile[4] == item.food.flavorProfile.Max())
                            {
                                mult--;
                            }
                            else if (item.food.flavorProfile[4] == item.food.flavorProfile.OrderBy(r => r).Skip(1).FirstOrDefault())
                            {
                                mult -= .5;
                            }
                            break;
                        case "none":
                            break;
                    }
                    if (string.IsNullOrEmpty(p.lastFed))
                    {
                        p.lastFed = DateTimeOffset.Now.ToString();
                    }
                    if (DateTimeOffset.Now - DateTimeOffset.Parse(p.lastFed) >= new TimeSpan(1, 0, 0))
                    {
                        p.lastFed = DateTimeOffset.Now.ToString();
                        p.feedsThisHour = 0;
                    }
                    p.feedsThisHour++;
                    if (p.feedsThisHour > 3)
                    {
                        mult = -1;
                    }
                    if (item.food.category == "Pokepuff")
                    {
                        p.friendship += Convert.ToInt32(Math.Round(5 * mult));
                        if (p.friendship > 100)
                        {
                            p.friendship = 100;
                        }
                    }
                    else if (item.food.category == "Pokeblock")
                    {
                        switch (item.food.type)
                        {
                            case "Red":
                                if (p.coolness * 0.3 < 1)
                                {
                                    p.coolness++;
                                }
                                else
                                {
                                    p.coolness += Convert.ToInt32(Math.Round(0.3 * p.coolness));
                                }
                                break;
                            case "Blue":
                                if (p.beauty * 0.3 < 1)
                                {
                                    p.beauty++;
                                }
                                else
                                {
                                    p.beauty += Convert.ToInt32(Math.Round(0.3 * p.beauty));
                                }
                                break;
                            case "Green":
                                if (p.cleverness * 0.3 < 1)
                                {
                                    p.cleverness++;
                                }
                                else
                                {
                                    p.cleverness += Convert.ToInt32(Math.Round(0.3 * p.cleverness));
                                }
                                break;
                            case "Yellow":
                                if (p.toughness * 0.3 < 1)
                                {
                                    p.toughness++;
                                }
                                else
                                {
                                    p.toughness += Convert.ToInt32(Math.Round(0.3 * p.toughness));
                                }
                                break;
                            case "Pink":
                                if (p.cuteness * 0.3 < 1)
                                {
                                    p.cuteness++;
                                }
                                else
                                {
                                    p.cuteness += Convert.ToInt32(Math.Round(0.3 * p.cuteness));
                                }
                                break;
                            case "Rainbow":
                                if (p.coolness * 0.3 < 1)
                                {
                                    p.coolness++;
                                }
                                else
                                {
                                    p.coolness += Convert.ToInt32(Math.Round(0.3 * p.coolness));
                                }
                                if (p.beauty * 0.3 < 1)
                                {
                                    p.beauty++;
                                }
                                else
                                {
                                    p.beauty += Convert.ToInt32(Math.Round(0.3 * p.beauty));
                                }
                                if (p.cleverness * 0.3 < 1)
                                {
                                    p.cleverness++;
                                }
                                else
                                {
                                    p.cleverness += Convert.ToInt32(Math.Round(0.3 * p.cleverness));
                                }
                                if (p.toughness * 0.3 < 1)
                                {
                                    p.toughness++;
                                }
                                else
                                {
                                    p.toughness += Convert.ToInt32(Math.Round(0.3 * p.toughness));
                                }
                                if (p.cuteness * 0.3 < 1)
                                {
                                    p.cuteness++;
                                }
                                else
                                {
                                    p.cuteness += Convert.ToInt32(Math.Round(0.3 * p.cuteness));
                                }
                                break;
                            case "Red+":
                                if (p.coolness * 0.6 < 1)
                                {
                                    p.coolness++;
                                }
                                else
                                {
                                    p.coolness += Convert.ToInt32(Math.Round(0.6 * p.coolness));
                                }
                                break;
                            case "Blue+":
                                if (p.beauty * 0.6 < 1)
                                {
                                    p.beauty++;
                                }
                                else
                                {
                                    p.beauty += Convert.ToInt32(Math.Round(0.6 * p.beauty));
                                }
                                break;
                            case "Green+":
                                if (p.cleverness * 0.6 < 1)
                                {
                                    p.cleverness++;
                                }
                                else
                                {
                                    p.cleverness += Convert.ToInt32(Math.Round(0.6 * p.cleverness));
                                }
                                break;
                            case "Yellow+":
                                if (p.toughness * 0.6 < 1)
                                {
                                    p.toughness++;
                                }
                                else
                                {
                                    p.toughness += Convert.ToInt32(Math.Round(0.6 * p.toughness));
                                }
                                break;
                            case "Pink+":
                                if (p.cuteness * 0.6 < 1)
                                {
                                    p.cuteness++;
                                }
                                else
                                {
                                    p.cuteness += Convert.ToInt32(Math.Round(0.6 * p.cuteness));
                                }
                                break;
                            case "Rainbow+":
                                if (p.coolness * 0.6 < 1)
                                {
                                    p.coolness++;
                                }
                                else
                                {
                                    p.coolness += Convert.ToInt32(Math.Round(0.6 * p.coolness));
                                }
                                if (p.beauty * 0.6 < 1)
                                {
                                    p.beauty++;
                                }
                                else
                                {
                                    p.beauty += Convert.ToInt32(Math.Round(0.6 * p.beauty));
                                }
                                if (p.cleverness * 0.6 < 1)
                                {
                                    p.cleverness++;
                                }
                                else
                                {
                                    p.cleverness += Convert.ToInt32(Math.Round(0.6 * p.cleverness));
                                }
                                if (p.toughness * 0.6 < 1)
                                {
                                    p.toughness++;
                                }
                                else
                                {
                                    p.toughness += Convert.ToInt32(Math.Round(0.6 * p.toughness));
                                }
                                if (p.cuteness * 0.6 < 1)
                                {
                                    p.cuteness++;
                                }
                                else
                                {
                                    p.cuteness += Convert.ToInt32(Math.Round(0.6 * p.cuteness));
                                }
                                break;
                        }
                        p.friendship += Convert.ToInt32(Math.Round(3 * mult));
                        if (p.friendship > 100)
                        {
                            p.friendship = 100;
                        }
                    }
                    else if (item.food.category == "Poffin")
                    {
                        int atNext = PlayerData.XpAtNextLevel(p);
                        p.xp += PlayerData.BaseXpGained(p);
                        if (p.xp >= atNext)
                        {
                            panels["LevelAnnounceWrapper"].transform.GetChild(0).gameObject.SetActive(true);
                            p.level++;
                            GameObject.Find("LevelAnnounceText").GetComponent<Text>().text = $"Your {p.pkmnName} has leveled up! It's now level {p.level}!";
                            int evoDexNo = PlayerData.EvoCheck(p);
                            if (evoDexNo > 0)
                            {
                                Pokemon evolveTo = new Pokemon(evoDexNo);
                                panels["EvoAnnounceWrapper"].transform.GetChild(0).gameObject.SetActive(true);
                                GameObject.Find("EvoAnnounceText").GetComponent<Text>().text = $"Your {p.pkmnName} has evolved into a {evolveTo.pkmnName}!";
                                p.dexNo = evolveTo.dexNo;
                                p.pkmnName = evolveTo.pkmnName;
                                p.type1 = evolveTo.type1;
                                p.type2 = evolveTo.type2;
                                p.speciesAbility1 = evolveTo.speciesAbility1;
                                p.speciesAbility2 = evolveTo.speciesAbility2;
                                p.baseStatTotal = evolveTo.baseStatTotal;
                                p.baseHP = evolveTo.baseHP;
                                p.baseAtk = evolveTo.baseAtk;
                                p.baseDef = evolveTo.baseDef;
                                p.baseSpAtk = evolveTo.baseSpAtk;
                                p.baseSpDef = evolveTo.baseSpDef;
                                p.baseSpd = evolveTo.baseSpd;
                                p.legendStatus = evolveTo.legendStatus;
                                p.mythicStatus = evolveTo.mythicStatus;
                                p.eggGroup1 = evolveTo.eggGroup1;
                                p.eggGroup2 = evolveTo.eggGroup2;
                                p.levelSpeed = evolveTo.levelSpeed;
                                p.genderThreshhold = evolveTo.genderThreshhold;
                                p.blurb = evolveTo.blurb;
                                p.evolutions = evolveTo.evolutions;
                                foreach (Pokemon pk in player.shownPokemon)
                                {
                                    if (pk != null && pk.id == p.id)
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
                        p.friendship += Convert.ToInt32(Math.Round(3 * mult));
                        if (p.friendship > 100)
                        {
                            p.friendship = 100;
                        }
                    }
                    GameObject icon = new GameObject();
                    switch (mult)
                    {
                        case var expression when mult >= 1.5 && mult <= 2:
                            Destroy(icon);
                            foreach (Transform obj in panels["FeedWrapper"].transform.GetChild(0).transform)
                            {
                                if (obj.gameObject.name == "SingingIcon")
                                {
                                    icon = obj.gameObject;
                                }
                            }
                            icon.SetActive(true);
                            icon.GetComponent<Animator>().Play("Singing");
                            break;
                        case var expression when mult >= 1 && mult < 1.5:
                            Destroy(icon);
                            foreach (Transform obj in panels["FeedWrapper"].transform.GetChild(0).transform)
                            {
                                if (obj.gameObject.name == "LoveIcon")
                                {
                                    icon = obj.gameObject;
                                }
                            }
                            icon.SetActive(true);
                            icon.GetComponent<Animator>().Play("Love");
                            break;
                        case var expression when mult >= 0.5 && mult < 1:
                            Destroy(icon);
                            foreach (Transform obj in panels["FeedWrapper"].transform.GetChild(0).transform)
                            {
                                if (obj.gameObject.name == "SadnessIcon")
                                {
                                    icon = obj.gameObject;
                                }
                            }
                            icon.SetActive(true);
                            icon.GetComponent<Animator>().Play("Sadness");
                            break;
                        case -1:
                            Destroy(icon);
                            foreach (Transform obj in panels["FeedWrapper"].transform.GetChild(0).transform)
                            {
                                if (obj.gameObject.name == "SickIcon")
                                {
                                    icon = obj.gameObject;
                                }
                            }
                            icon.SetActive(true);
                            icon.GetComponent<Animator>().Play("Sick");
                            break;
                    }
                    break;
                }
            }
            List<Item> toRemove = new List<Item>();
            foreach (Item i in player.inventory)
            {
                if (i.id == item.id)
                {
                    toRemove.Add(i);
                }
            }
            foreach (Item i in toRemove)
            {
                player.inventory.Remove(i);
            }
            GameObject panel = GameObject.Find("FeedItemsPanel");
            GameObject.Find("FeedPanelImage").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Artwork/" + player.pokemon.Find(x => x.id == id).dexNo);
            if (player.inventory.FindAll(x => x.food != null && x.food.category != "").Count >= 8)
            {
                panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (35 * player.inventory.FindAll(x => x.food != null && x.food.category != "").Count) + 15);
            }
            Vector2 centerAnchor = new Vector2(0.5f, 0.5f);
            const float x = 27f;
            float y = -20f;
            Vector3 initialPosition = new Vector3(x, y);
            foreach (Transform obj in panel.transform)
            {
                Destroy(obj.gameObject);
            }
            int cnt = 1;
            foreach (Item i in player.inventory)
            {
                if (GameObject.Find("FeedItem" + cnt + "Sprite") != null)
                {
                    Destroy(GameObject.Find("FeedItem" + cnt + "Sprite"));
                }
                if (i.food != null && i.food.category != "")
                {
                    GameObject item = new GameObject
                    {
                        name = "FeedItem" + cnt + "Sprite"
                    };
                    string name = "";
                    if (i.food.category == "Pokeblock")
                    {
                        if (i.name.Contains("+"))
                        {
                            name = i.food.category.ToLower() + "_" + i.name.ToLower().Replace("+", "_plus");
                        }
                        else
                        {
                            name = i.food.category.ToLower() + "_" + i.name.ToLower();
                        }
                    }
                    else if (i.food.category == "Poffin")
                    {
                        name = i.food.category + "_" + i.name;
                    }
                    else if (i.food.category == "Pokepuff")
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
                    }
                    item.AddComponent<Image>().sprite = Resources.Load<Sprite>("Food/" + name);
                    item.transform.SetParent(panel.transform, false);
                    item.transform.localScale = new Vector3(0.3f, 0.3f);
                    RectTransform itemRect = item.GetComponent<RectTransform>();
                    itemRect.anchorMin = centerAnchor;
                    itemRect.anchorMax = centerAnchor;
                    item.transform.localPosition = new Vector3(x, y);
                    item.AddComponent<BoxCollider2D>();
                    FoodClickHandler handler = item.AddComponent<FoodClickHandler>();
                    handler.item = i;
                    handler.toFeed = id;
                    y -= 35;
                    cnt++;
                }
            }
            Invoke(nameof(CloseAnimation), 1.75f);
            panels["SaveWrapper"].transform.GetChild(0).gameObject.SetActive(true);
            PlayerData.SavePlayerData(player);
            panels["SaveWrapper"].transform.GetChild(0).gameObject.SetActive(false);
        }

        public void CloseAnimation()
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            foreach (Transform obj in panels["FeedWrapper"].transform.GetChild(0).transform)
            {
                if (obj.gameObject.name == "LoveIcon")
                {
                    obj.gameObject.SetActive(false);
                }
                else if (obj.gameObject.name == "SickIcon")
                {
                    obj.gameObject.SetActive(false);
                }
                else if (obj.gameObject.name == "SingingIcon")
                {
                    obj.gameObject.SetActive(false);
                }
                else if (obj.gameObject.name == "SadnessIcon")
                {
                    obj.gameObject.SetActive(false);
                }
                else if (obj.gameObject.name == "FoodContextPanel")
                {
                    obj.gameObject.SetActive(false);
                }
            }
        }
    }
}
