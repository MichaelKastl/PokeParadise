using UnityEngine;
using PokeParadise.Classes;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

namespace PokeParadise
{
    public class ItemUsePkmnHandler : MonoBehaviour
    {
        public int itemUsed;
        public void Use()
        {
            Player player = PlayerData.FetchCurrentPlayerData();
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            Item i = player.inventory.Where(x => x.id == itemUsed).ToList()[0];
            foreach (Pokemon p in player.pokemon)
            {
                if (GameObject.Find("ItemUseSprite" + p.id) != null && !GameObject.Find("ItemUseSprite" + p.id).GetComponent<Button>().interactable)
                {
                    if (i.name == "Rare Candy")
                    {
                        panels["LevelAnnounceWrapper"].transform.GetChild(0).gameObject.SetActive(true);
                        p.level++;
                        GameObject.Find("LevelAnnounceText").GetComponent<Text>().text = $"Your {p.pkmnName} has leveled up! It's now level {p.level}!";
                        int evoDexNo = PlayerData.EvoCheck(p);
                        if (evoDexNo > 0)
                        {
                            Pokemon evolveTo = new Pokemon(evoDexNo);
                            panels["EvoAnnounceWrapper"].transform.GetChild(0).gameObject.SetActive(true);
                            panels["ItemUsePkmnPanel"].transform.GetChild(0).gameObject.SetActive(false);
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
                    else
                    {
                        string itemName = i.name;
                        itemName = itemName.Replace(" ", "-");
                        itemName = itemName.Replace("\'", "");
                        int evoDexNo = PlayerData.EvoCheck(p, itemName.ToLower());
                        if (evoDexNo > 0)
                        {
                            Pokemon evolveTo = new Pokemon(evoDexNo);
                            panels["EvoAnnounceWrapper"].transform.GetChild(0).gameObject.SetActive(true);
                            panels["ItemUsePkmnWrapper"].transform.GetChild(0).gameObject.SetActive(false);
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
                        else
                        {
                            foreach (GameObject obj in panels["ItemUsePkmnWrapper"].transform.GetChild(0).transform)
                            {
                                if (obj.name == "ItemNoEffectPanel")
                                {
                                    obj.SetActive(true);
                                }
                            }
                        }
                    }
                    player.inventory.RemoveAll(x => x.id == itemUsed);
                    PlayerData.SavePlayerData(player);
                }
            }
        }
    }
}