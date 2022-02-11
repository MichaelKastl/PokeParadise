using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public class PetHandler : MonoBehaviour
    {
        public int id;

        public void OnMouseUp()
        {
            Player player = PlayerData.FetchCurrentPlayerData();
            Pokemon pkmn = player.pokemon.Find(x => x.id == id);
            System.Random rand = new System.Random();
            if (string.IsNullOrEmpty(pkmn.lastPetCycle))
            {
                pkmn.lastPetCycle = DateTimeOffset.MinValue.ToString();
            }
            if (pkmn.lastPetCycle?.Length == 0 || DateTimeOffset.Parse(pkmn.lastPetCycle) == DateTimeOffset.MinValue)
            {
                pkmn.lastPetCycle = DateTimeOffset.Now.ToString();
            }
            if (DateTimeOffset.Now - DateTimeOffset.Parse(pkmn.lastPetCycle) >= new TimeSpan(1, 0, 0))
            {
                pkmn.lastPetCycle = DateTimeOffset.Now.ToString();
                pkmn.petsBeforeAnger = 0;
                pkmn.petsThisHour = 0;
            }
            else if (pkmn.petsBeforeAnger == 0)
            {
                pkmn.petsBeforeAnger = rand.Next(3, 6);
            }
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            pkmn.petsThisHour++;
            int atNext = PlayerData.XpAtNextLevel(pkmn);
            pkmn.xp += PlayerData.BaseXpGained(pkmn);
            if (pkmn.xp >= atNext)
            {
                panels["LevelAnnounceWrapper"].transform.GetChild(0).gameObject.SetActive(true);
                pkmn.level++;
                GameObject.Find("LevelAnnounceText").GetComponent<Text>().text = $"Your {pkmn.pkmnName} has leveled up! It's now level {pkmn.level}!";
                int evoDexNo = PlayerData.EvoCheck(pkmn);
                if (evoDexNo > 0)
                {
                    Pokemon evolveTo = new Pokemon(evoDexNo);
                    panels["EvoAnnounceWrapper"].transform.GetChild(0).gameObject.SetActive(true);
                    string pkmnPrevoName = pkmn.pkmnName;
                    pkmn = PlayerData.EvolutionHandler(pkmn, evoDexNo);
                    foreach (Pokemon pk in player.shownPokemon)
                    {
                        if (pk != null && pk.id == pkmn.id)
                        {
                            pk.dexNo = pkmn.dexNo;
                            pk.pkmnName = pkmn.pkmnName;
                            pk.type1 = pkmn.type1;
                            pk.type2 = pkmn.type2;
                            pk.speciesAbility1 = pkmn.speciesAbility1;
                            pk.speciesAbility2 = pkmn.speciesAbility2;
                            pk.baseStatTotal = pkmn.baseStatTotal;
                            pk.baseHP = pkmn.baseHP;
                            pk.baseAtk = pkmn.baseAtk;
                            pk.baseDef = pkmn.baseDef;
                            pk.baseSpAtk = pkmn.baseSpAtk;
                            pk.baseSpDef = pkmn.baseSpDef;
                            pk.baseSpd = pkmn.baseSpd;
                            pk.legendStatus = pkmn.legendStatus;
                            pk.mythicStatus = pkmn.mythicStatus;
                            pk.eggGroup1 = pkmn.eggGroup1;
                            pk.eggGroup2 = pkmn.eggGroup2;
                            pk.levelSpeed = pkmn.levelSpeed;
                            pk.genderThreshhold = pkmn.genderThreshhold;
                            pk.blurb = pkmn.blurb;
                            pk.evolutions = pkmn.evolutions;
                        }
                    }
                    PlayerData.LoadPokemonMenuSprites(player.shownPokemon);
                    GameObject.Find("EvoAnnounceText").GetComponent<Text>().text = $"Your {pkmnPrevoName} has evolved into a {pkmn.pkmnName}!";
                }
            }
            GameObject icon = new GameObject();
            if (pkmn.petsThisHour < pkmn.petsBeforeAnger - 1 || pkmn.petsBeforeAnger == 0)
            {
                foreach (Transform obj in panels["CareWrapper"].transform.GetChild(0).transform)
                {
                    if (obj.gameObject.name == "LoveIcon")
                    {
                        Destroy(icon);
                        icon = obj.gameObject;
                        break;
                    }
                }
                pkmn.friendship++;
                icon.SetActive(true);
                icon.GetComponent<Animator>().Play("Love");
            }
            else if (pkmn.petsThisHour == pkmn.petsBeforeAnger - 1)
            {
                foreach (Transform obj in panels["CareWrapper"].transform.GetChild(0).transform)
                {
                    if (obj.gameObject.name == "FrustrationIcon")
                    {
                        Destroy(icon);
                        icon = obj.gameObject;
                        break;
                    }
                }
                pkmn.friendship++;
                icon.SetActive(true);
                icon.GetComponent<Animator>().Play("Frustration");
            }
            else
            {
                foreach (Transform obj in panels["CareWrapper"].transform.GetChild(0).transform)
                {
                    if (obj.gameObject.name == "AngerIcon")
                    {
                        Destroy(icon);
                        icon = obj.gameObject;
                        break;
                    }
                }
                pkmn.friendship--;
                icon.SetActive(true);
                icon.GetComponent<Animator>().Play("Anger");
            }
            Invoke(nameof(CloseAnimation), 1.75f);
            if (pkmn.friendship > 100)
            {
                pkmn.friendship = 100;
            }
            PlayerData.SavePlayerData(player);
        }

        public void CloseAnimation()
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            foreach (Transform obj in panels["CareWrapper"].transform.GetChild(0).transform)
            {
                if (obj.gameObject.name == "LoveIcon")
                {
                    obj.gameObject.SetActive(false);
                }
                else if (obj.gameObject.name == "AngerIcon")
                {
                    obj.gameObject.SetActive(false);
                }
                else if (obj.gameObject.name == "FrustrationIcon")
                {
                    obj.gameObject.SetActive(false);
                }
            }
        }
    }
}
