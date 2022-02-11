using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static System.Environment;
using PokeParadise.Classes;

namespace PokeParadise
{
    public class CheckEggs : MonoBehaviour
    {
        private Player p;
        private GameVariables gv;
        // Start is called before the first frame update
        public void Start()
        {
            /*p = PlayerData.FetchCurrentPlayerData();
            Move[] movedex = PlayerData.FetchMoveDex();
            foreach (Pokemon pkmn in p.pokemon)
            {
                if (pkmn.learnset.Count == 0)
                {
                    pkmn.learnset = PlayerData.FetchPokedex()[pkmn.dexNo].learnset;
                }
                pkmn.moves = null;
                if (pkmn.moves == null)
                {
                    pkmn.moves = new Move[4];
                    List<int> learnsetIDs = pkmn.learnset.Select(x => x.id).ToList();
                    MoveInfo[] movesLearnable = pkmn.learnset.Where(x => x.learnedLevel <= pkmn.level && x.learnedLevel != 0).ToArray();
                    System.Random rand = new System.Random();
                    int randIndex = rand.Next(0, movesLearnable.Length);
                    pkmn.moves[0] = movedex[movesLearnable[randIndex].id];
                    pkmn.moves[0].learnedLevel = movesLearnable[randIndex].learnedLevel;
                }
            }
            foreach (Pokemon p in p.shownPokemon)
            {
                if (p.learnset.Count == 0)
                {
                    p.learnset = Pokemon.SpeciesData(p.dexNo).learnset;
                }
                p.moves = null;
                if (p.moves == null)
                {
                    p.moves = new Move[4];
                    List<Move> movesLearnable = new List<Move>();
                    foreach (MoveInfo mv in p.learnset)
                    {
                        Move move = movedex[mv.id];
                        if (move.learnedLevel > 0 && move.learnedLevel <= p.level)
                        {
                            movesLearnable.Add(move);
                        }
                    }
                    System.Random rand = new System.Random();
                    int randIndex = rand.Next(1, movesLearnable.Count + 1);
                    int i = 1;
                    foreach (Move m in movesLearnable)
                    {
                        if (i == randIndex)
                        {
                            p.moves[0] = m;
                        }
                        i++;
                    }
                }
            }
            PlayerData.SavePlayerData(p);*/
            /*p = PlayerData.FetchCurrentPlayerData();
            foreach (Pokemon p in p.pokemon)
            {
                if (p.level == 0)
                {
                    p.level = 1;
                }
            }
            foreach (Pokemon p in p.shownPokemon)
            {
                if (p.level == 0)
                {
                    p.level = 1;
                }
            }
            PlayerData.SavePlayerData(p);*/
            /*int cnt = 1;
            p = PlayerData.FetchCurrentPlayerData();
            foreach (Item i in p.inventory)
            {
                i.id = cnt;
                cnt++;
            }
            Pokemon[] dex = PlayerData.FetchPokedex();
            foreach (Pokemon pkmn in p.pokemon)
            {
                pkmn.parent1 = new Pokemon(pkmn.dexNo);
                pkmn.parent2 = RandomFromEggGroup(pkmn);
                pkmn.evolutions = dex[pkmn.dexNo].evolutions;
            }
            foreach (Pokemon pkmn in p.shownPokemon)
            {
                pkmn.parent1 = new Pokemon(pkmn.dexNo);
                pkmn.parent2 = RandomFromEggGroup(pkmn);
                pkmn.evolutions = dex[pkmn.dexNo].evolutions;
            }
            PlayerData.SavePlayerData(p);*/
            InvokeRepeating(nameof(LoadPlayer), 0f, 15.0f);
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

        // Update is called once per frame
        public void Update()
        {
            if (p == null)
            {
                p = PlayerData.FetchCurrentPlayerData();
            }

            if (gv == null)
            {
                gv = GameVariables.FetchGameVariables();
            }

            if (gv.lastCheck == null)
            {
                gv.lastCheck = DateTimeOffset.Now.ToString();
                gv.SaveGameVariables();
            }
            else if ((DateTimeOffset.Now - DateTimeOffset.Parse(gv.lastCheck)) >= new TimeSpan(0, 5, 0))
            {
                Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
                int coins = 0;
                foreach (Pokemon pkmn in p.pokemon)
                {
                    int c = 0;
                    if (pkmn.baseStatTotal <= 200)
                    {
                        c = pkmn.baseStatTotal / 2;
                    }
                    else if (pkmn.baseStatTotal <= 350)
                    {
                        c = Convert.ToInt32(Math.Round(pkmn.baseStatTotal / 1.25));
                    }
                    else if (pkmn.baseStatTotal <= 450)
                    {
                        c = pkmn.baseStatTotal;
                    }
                    else if (pkmn.baseStatTotal <= 500)
                    {
                        c = pkmn.baseStatTotal * 2;
                    }
                    else if (pkmn.baseStatTotal <= 550)
                    {
                        c = Convert.ToInt32(Math.Round(pkmn.baseStatTotal * 2.25));
                    }
                    else if (pkmn.baseStatTotal <= 600)
                    {
                        c = Convert.ToInt32(Math.Round(pkmn.baseStatTotal * 2.5));
                    }
                    else if (pkmn.baseStatTotal <= 650)
                    {
                        c = pkmn.baseStatTotal * 3;
                    }
                    else if (pkmn.baseStatTotal > 650)
                    {
                        c = pkmn.baseStatTotal * 4;
                    }
                    c /= 2;
                    int percent = 10;
                    if (pkmn.friendship >= 10)
                    {
                        percent = ((int)Math.Round(pkmn.friendship / 10.0)) * 10;
                    }
                    coins += (int)Math.Round(c * (percent / 100d));
                }
                coins *= (int)Math.Floor((DateTimeOffset.Now - DateTimeOffset.Parse(gv.lastCheck)).TotalMinutes) / 20;
                p.coins += coins;
                panels["SaveWrapper"].transform.GetChild(0).gameObject.SetActive(true);
                PlayerData.SavePlayerData(p);
                panels["SaveWrapper"].transform.GetChild(0).gameObject.SetActive(false);
                gv.lastCheck = DateTimeOffset.Now.ToString();
                gv.SaveGameVariables();
                string appData = Application.persistentDataPath;
                GameObject.Find("CurrencyTotal").GetComponent<Text>().text = "$" + p.coins;
                if (!Directory.Exists($"{appData}/PokeParadise_Saves/Backups"))
                {
                    Directory.CreateDirectory($"{appData}/PokeParadise_Saves/Backups");
                }
                File.Delete($"{appData}/PokeParadise_Saves/Backups/{p.name}-backup.xml");
                File.Copy($"{appData}/PokeParadise_Saves/{p.name}-savedata.xml", $"{appData}/PokeParadise_Saves/Backups/{p.name}-backup.xml");
            }

            if (gv.lastEggCheck == null)
            {
                gv.lastEggCheck = DateTimeOffset.Now.ToString();
                gv.SaveGameVariables();
            }
            else if ((DateTimeOffset.Now - DateTimeOffset.Parse(gv.lastEggCheck)) >= new TimeSpan(0, 0, 30))
            {
                Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
                List<Egg> hatched = new List<Egg>();
                List<Egg> laid = new List<Egg>();
                foreach (Egg e in p.eggs)
                {
                    if ((e.obtained != "" || e.obtained != null) && !e.isIncubating)
                    {
                        TimeSpan diff = DateTimeOffset.Now - DateTimeOffset.Parse(e.obtained);
                        if (diff >= TimeSpan.Parse(e.timeToHatch))
                        {
                            hatched.Add(e);
                        }
                    }
                    else if (e.isIncubating)
                    {
                        TimeSpan diff = DateTimeOffset.Now - DateTimeOffset.Parse(e.bred);
                        if (diff >= TimeSpan.Parse(e.timeToBreed))
                        {
                            laid.Add(e);
                        }
                    }
                }
                Vector3 initialPosition = new Vector3(-150, 0);
                int x = -225;
                const int y = 0;
                int cnt = 1;
                Sprite starterSprite = Resources.Load<Sprite>("1_menu");
                int totalEarned = 0;
                if (laid.Count > 0)
                {
                    foreach (Egg e in laid)
                    {
                        e.obtained = DateTimeOffset.Now.ToString();
                        e.isIncubating = false;
                        foreach (Pokemon pkmn in p.pokemon)
                        {
                            if (pkmn.id == e.pkmn.parent1.id || pkmn.id == e.pkmn.parent2.id)
                            {
                                pkmn.isBreeding = false;
                            }
                        }
                        foreach (Pokemon pkmn in p.shownPokemon)
                        {
                            if (pkmn != null)
                            {
                                if (pkmn.id == e.pkmn.parent1.id || pkmn.id == e.pkmn.parent2.id)
                                {
                                    pkmn.isBreeding = false;
                                }
                            }
                        }
                    }
                    panels["LaidWrapper"].transform.GetChild(0).gameObject.SetActive(true);
                    panels["SaveWrapper"].transform.GetChild(0).gameObject.SetActive(true);
                    PlayerData.SavePlayerData(p);
                    panels["SaveWrapper"].transform.GetChild(0).gameObject.SetActive(false);
                    PlayerData.LoadPokemonMenuSprites(p.shownPokemon);
                }
                if (hatched.Count > 0)
                {
                    GameObject panel = panels["EggHatchWrapper"].transform.GetChild(0).gameObject;
                    panel.SetActive(true);
                    foreach (Transform obj in panel.transform)
                    {
                        if (obj.gameObject.name != "Accept" && obj.gameObject.name != "HatchPanelText" && obj.gameObject.name != "CurrencyEarned")
                        {
                            Destroy(obj.gameObject);
                        }
                    }
                    GameObject hatchPanelText = GameObject.Find("HatchPanelText");
                    GameObject earnedPanel = GameObject.Find("CurrencyEarned");
                    foreach (Egg e in hatched)
                    {
                        p.eggs.Remove(e);
                        if (p.pokemon.Count == 0)
                        {
                            e.pkmn.id = 1;
                        }
                        else
                        {
                            e.pkmn.id = p.pokemon.Max(x => x.id) + 1;
                        }
                        e.pkmn.level = 1;
                        p.pokemon.Add(e.pkmn);
                        if (p.shownPokemon[20] == null)
                        {
                            for (int i = 0; i < p.shownPokemon.Length; i++)
                            {
                                if (p.shownPokemon[i] == null)
                                {
                                    p.shownPokemon[i] = e.pkmn;
                                    break;
                                }
                            }
                        }
                        if (cnt <= 8)
                        {
                            if (GameObject.Find("HatchSprite_" + cnt) != null)
                            {
                                Destroy(GameObject.Find("HatchSprite_" + cnt));
                            }
                            GameObject sprite = new GameObject
                            {
                                name = "HatchSprite_" + cnt
                            };
                            SpriteRenderer rndr = sprite.AddComponent<SpriteRenderer>();
                            rndr.sortingLayerName = "UI - Overlay";
                            rndr.sprite = starterSprite;
                            sprite.AddComponent<SpriteSwap>().SpriteSheetName = e.pkmn.dexNo.ToString() + "_menu";
                            sprite.transform.parent = panel.transform;
                            sprite.transform.localScale = new Vector3(175, 175);
                            if (cnt == 0)
                            {
                                sprite.transform.localPosition = initialPosition;
                            }
                            else
                            {
                                x += 50;
                                sprite.transform.localPosition = new Vector3(x, y);
                            }
                        }
                        cnt++;
                        totalEarned += Convert.ToInt32(Math.Round(TimeSpan.Parse(e.timeToHatch).TotalSeconds * 0.5));
                    }
                    if (hatched.Count > 1)
                    {
                        hatchPanelText.GetComponent<Text>().text = "Your Eggs have hatched!";
                        p.coins += totalEarned;
                        earnedPanel.GetComponent<Text>().text = "You have earned $" + totalEarned + "!";
                        panel.SetActive(true);
                        panels["SaveWrapper"].transform.GetChild(0).gameObject.SetActive(true);
                        PlayerData.SavePlayerData(p);
                        panels["SaveWrapper"].transform.GetChild(0).gameObject.SetActive(false);
                        p = null;
                    }
                    else if (hatched.Count == 1)
                    {
                        hatchPanelText.GetComponent<Text>().text = "Your Egg has hatched!";
                        p.coins += totalEarned;
                        earnedPanel.GetComponent<Text>().text = "You have earned $" + totalEarned + "!";
                        panel.SetActive(true);
                        panels["SaveWrapper"].transform.GetChild(0).gameObject.SetActive(true);
                        PlayerData.SavePlayerData(p);
                        panels["SaveWrapper"].transform.GetChild(0).gameObject.SetActive(false);
                        p = null;
                    }
                }
            }
        }

        public void CloseHatchPanel()
        {
            for (int cnt = 0; cnt <= 10; cnt++)
            {
                Destroy(GameObject.Find("HatchSprite_" + cnt));
            }
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            panels["EggHatchWrapper"].transform.GetChild(0).gameObject.SetActive(false);
            PlayerData.LoadPokemonMenuSprites(PlayerData.FetchCurrentPlayerData().shownPokemon);
        }

        public void LoadPlayer()
        {
            p = PlayerData.FetchCurrentPlayerData();
        }
    }
}