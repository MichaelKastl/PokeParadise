using PokeParadise.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static System.Environment;

namespace PokeParadise
{
    public static class PlayerData
    {
        public static bool isRunning;
        public static string name { get; set; }
        public static int level { get; set; }
        public static int currency { get; set; }
        public static int experience { get; set; }
        public static Player player { get; set; }
        public static UnityAction action { get; set; }
        public static Pokemon[] pokedex { get; set; }
        public static Move[] movedex { get; set; }

        public static Player FetchCurrentPlayerData()
        {
            Player p = new Player();
            string name;
            foreach (GameObject obj in GetAllObjectsOnlyInScene())
            {
                if (obj.name == "LoadGameSelect")
                {
                    name = obj.GetComponent<Text>().text;
                }
            }
            string appData = Application.persistentDataPath;
            string text = File.ReadAllText($"{appData}/PokeParadise_Saves/current.xml");
            XmlRootAttribute xRoot = new XmlRootAttribute
            {
                ElementName = "Player"
            };
            XmlSerializer x = new XmlSerializer(typeof(Player), xRoot);
            using (StringReader reader = new StringReader(text))
            {
                p = (Player)x.Deserialize(reader);
            }
            return p;
        }

        public static Player FetchPlayerData(string name)
        {
            PlayerData.name = name;
            Player p = new Player();
            string appData = Application.persistentDataPath;
            string text = File.ReadAllText($"{appData}/PokeParadise_Saves/{name}-savedata.xml");
            XmlRootAttribute xRoot = new XmlRootAttribute
            {
                ElementName = "Player"
            };
            XmlSerializer x = new XmlSerializer(typeof(Player), xRoot);
            using (StringReader reader = new StringReader(text))
            {
                p = (Player)x.Deserialize(reader);
            }
            return p;
        }

        public static void LoadPokemonMenuSprites(Pokemon[] pokemon)
        {
            if (!isRunning)
            {
                isRunning = true;
                player = FetchCurrentPlayerData();
                foreach (GameObject obj in GetAllObjectsOnlyInScene())
                {
                    if (obj.name == "CurrencyTotal")
                    {
                        obj.GetComponent<Text>().text = "$" + FetchCurrentPlayerData().coins;
                    }
                }
                Vector3 initialPosition = new Vector3(-30, 205);
                int x = -30;
                int y = 205;
                int cnt = 1;
                Sprite starterSprite = Resources.Load<Sprite>("1_menu");
                foreach (Pokemon p in pokemon)
                {
                    if (cnt <= 21 && p != null)
                    {
                        if (GameObject.Find("MenuSprite" + cnt) != null)
                        {
                            UnityEngine.Object.Destroy(GameObject.Find("MenuSprite" + cnt));
                        }

                        GameObject menuSprite = new GameObject
                        {
                            name = "MenuSprite" + cnt
                        };
                        SpriteRenderer rndr = menuSprite.AddComponent<SpriteRenderer>();
                        rndr.sortingLayerName = "UI - Sprites";
                        rndr.sprite = Resources.Load<Sprite>(p.dexNo + "_menu");
                        menuSprite.AddComponent<BoxCollider2D>();
                        menuSprite.AddComponent<PokemonContextHandler>().id = p.id;
                        menuSprite.transform.parent = GameObject.Find("CurrentPokemon").transform;
                        menuSprite.transform.localScale = new Vector3(100, 100);
                        if (cnt == 1)
                        {
                            menuSprite.transform.localPosition = initialPosition;
                            x += 30;
                        }
                        else if (cnt % 3 == 0)
                        {
                            menuSprite.transform.localPosition = new Vector3(x, y);
                            x = -30;
                            y -= 30;
                        }
                        else
                        {
                            menuSprite.transform.localPosition = new Vector3(x, y);
                            x += 30;
                        }
                    }
                    cnt++;
                }
                Vector3[] spawnPoints = new Vector3[21];
                spawnPoints[0] = new Vector3(3.96f, -4.87f);
                spawnPoints[1] = new Vector3(-8.77f, -2.56f);
                spawnPoints[2] = new Vector3(-1.56f, -4.01f);
                spawnPoints[3] = new Vector3(1.99f, -0.16f);
                spawnPoints[4] = new Vector3(-3.64f, -2.4f);
                spawnPoints[5] = new Vector3(-6.06f, -4.01f);
                spawnPoints[6] = new Vector3(6.58f, 5.15f);
                spawnPoints[7] = new Vector3(-5.28f, -8.65f);
                spawnPoints[8] = new Vector3(-8.91f, 3.96f);
                spawnPoints[9] = new Vector3(8.34f, 0.82f);
                spawnPoints[10] = new Vector3(1.06f, 6.91f);
                spawnPoints[11] = new Vector3(0.09f, -6.58f);
                spawnPoints[12] = new Vector3(-4.42f, 0.13f);
                spawnPoints[13] = new Vector3(3.63f, 2.17f);
                spawnPoints[14] = new Vector3(-1.92f, 3.8f);
                spawnPoints[15] = new Vector3(-1f, 5.8f);
                spawnPoints[16] = new Vector3(5f, 8.5f);
                spawnPoints[17] = new Vector3(7f, -8f);
                spawnPoints[18] = new Vector3(-7.16f, -7.13f);
                spawnPoints[19] = new Vector3(-6.44f, 2.79f);
                spawnPoints[20] = new Vector3(-8.88f, 8.52f);
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PlayerPokemon"))
                {
                    UnityEngine.Object.Destroy(obj);
                }
                foreach (Vector3 spawnPoint in spawnPoints)
                {
                    int index = Array.IndexOf(spawnPoints, spawnPoint);
                    if ((index + 1) <= pokemon.Length && pokemon[index] != null)
                    {
                        int dexNo = pokemon[index].dexNo;
                        if (!pokemon[index].isBreeding)
                        {
                            GameObject sprite = new GameObject
                            {
                                name = "Sprite" + pokemon[index].id,
                                tag = "PlayerPokemon"
                            };
                            SpriteRenderer sprndr = sprite.AddComponent<SpriteRenderer>();
                            sprndr.sortingLayerName = "Default";
                            sprndr.sprite = starterSprite;
                            sprite.transform.localScale = new Vector3(2, 2);
                            sprite.AddComponent<Animator>().runtimeAnimatorController = Resources.Load("Animations/AnimatorController") as RuntimeAnimatorController;
                            sprite.AddComponent<Wanderer>();
                            Rigidbody2D rigid = sprite.AddComponent<Rigidbody2D>();
                            rigid.bodyType = RigidbodyType2D.Dynamic;
                            rigid.gravityScale = 0;
                            rigid.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                            rigid.freezeRotation = true;
                            sprite.AddComponent<BoxCollider2D>().size = new Vector3(0.3f, 0.32f);
                            SpriteSwap swapper = sprite.AddComponent<SpriteSwap>();
                            if (dexNo < 10)
                            {
                                swapper.SpriteSheetName = "00" + dexNo;
                            }
                            else if (dexNo < 100)
                            {
                                swapper.SpriteSheetName = "0" + dexNo;
                            }
                            else
                            {
                                swapper.SpriteSheetName = dexNo.ToString();
                            }
                            sprite.transform.localPosition = spawnPoint;
                        }
                    }
                }
                isRunning = false;
            }
        }

        public static void SaveCurrentPlayer(Player p)
        {
            foreach (Pokemon pkmn in p.pokemon)
            {
                if (Array.Find(p.shownPokemon, x => x.id == pkmn.id) != null)
                {
                    int index = Array.IndexOf(p.shownPokemon, Array.Find(p.shownPokemon, x => x.id == pkmn.id));
                    p.shownPokemon[index] = pkmn;
                }
            }
            string appData = Application.persistentDataPath;
            if (!Directory.Exists($"{appData}/PokeParadise_Saves"))
            {
                Directory.CreateDirectory($"{appData}/PokeParadise_Saves");
            }
            XmlSerializer x = new XmlSerializer(p.GetType());
            File.WriteAllText($"{appData}/PokeParadise_Saves/current.xml", "");
            using FileStream fs = new FileStream($"{appData}/PokeParadise_Saves/current.xml", FileMode.OpenOrCreate);
            x.Serialize(fs, p);
        }

        public static void SavePlayerData(Player p)
        {
            GameObject panel = new GameObject();
            foreach (GameObject obj in GetAllObjectsOnlyInScene())
            {
                if (obj.name == "SaveIcon")
                {
                    UnityEngine.Object.Destroy(panel);
                    panel = obj;
                    obj.SetActive(true);
                }
            }
            foreach (Pokemon pkmn in p.pokemon)
            {
                if (Array.Find(p.shownPokemon, x => x.id == pkmn.id) != null)
                {
                    int index = Array.IndexOf(p.shownPokemon, Array.Find(p.shownPokemon, x => x.id == pkmn.id));
                    p.shownPokemon[index] = pkmn;
                }
            }
            string appData = Application.persistentDataPath;
            if (!Directory.Exists($"{appData}/PokeParadise_Saves"))
            {
                Directory.CreateDirectory($"{appData}/PokeParadise_Saves");
            }
            XmlSerializer x = new XmlSerializer(p.GetType());
            File.WriteAllText($"{appData}/PokeParadise_Saves/{p.name}-savedata.xml", "");
            using (FileStream fs = new FileStream($"{appData}/PokeParadise_Saves/{p.name}-savedata.xml", FileMode.OpenOrCreate))
                x.Serialize(fs, p);
            File.Delete($"{appData}/PokeParadise_Saves/current.xml");
            File.Copy($"{appData}/PokeParadise_Saves/{p.name}-savedata.xml", $"{appData}/PokeParadise_Saves/current.xml");
            panel.SetActive(false);
        }

        public static Berry[] FetchBerries()
        {
            Berry[] b = new Berry[65];
            string appData = Application.persistentDataPath;
            string text = File.ReadAllText($"{appData}/PokeParadise_Data/berries.xml");
            XmlRootAttribute xRoot = new XmlRootAttribute
            {
                ElementName = "ArrayOfBerry"
            };
            XmlSerializer x = new XmlSerializer(typeof(Berry[]), xRoot);
            using (StringReader reader = new StringReader(text))
            {
                b = (Berry[])x.Deserialize(reader);
            }
            return b;
        }

        public static Pokemon[] FetchPokedex()
        {
            if (pokedex == null)
            {
                Pokemon[] p = new Pokemon[810];
                string appData = Application.persistentDataPath;
                string text = File.ReadAllText($"{appData}/PokeParadise_Data/pokedex.xml");
                XmlSerializer x = new XmlSerializer(typeof(Pokemon[]));
                using (StringReader reader = new StringReader(text))
                {
                    p = (Pokemon[])x.Deserialize(reader);
                }
                pokedex = p;
            }
            return pokedex;
        }

        public static Move[] FetchMoveDex()
        {
            if (movedex == null)
            {
                Move[] p = new Move[810];
                string appData = Application.persistentDataPath;
                string text = File.ReadAllText($"{appData}/PokeParadise_Data/movedex.xml");
                XmlSerializer x = new XmlSerializer(typeof(Move[]));
                using (StringReader reader = new StringReader(text))
                {
                    p = (Move[])x.Deserialize(reader);
                }
                movedex = p;
            }
            return movedex;
        }

        public static List<GameObject> GetAllObjectsOnlyInScene()
        {
            List<GameObject> objectsInScene = new List<GameObject>();

            foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
            {
                if (!(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                    objectsInScene.Add(go);
            }

            return objectsInScene;
        }

        public static List<LevelSpeed> FetchXpToLevel()
        {
            List<LevelSpeed> speeds = new List<LevelSpeed>();
            string appData = Application.persistentDataPath;
            string text = File.ReadAllText($"{appData}/PokeParadise_Data/xpToLevel.xml");
            XmlRootAttribute xRoot = new XmlRootAttribute
            {
                ElementName = "ArrayOfLevelSpeed"
            };
            XmlSerializer x = new XmlSerializer(typeof(List<LevelSpeed>), xRoot);
            using (StringReader reader = new StringReader(text))
            {
                speeds = (List<LevelSpeed>)x.Deserialize(reader);
            }
            return speeds;
        }

        public static List<LevelSpeed> FetchXpAtLevel()
        {
            List<LevelSpeed> speeds = new List<LevelSpeed>();
            XmlRootAttribute xRoot = new XmlRootAttribute
            {
                ElementName = "ArrayOfLevelSpeed"
            };
            XmlSerializer x = new XmlSerializer(speeds.GetType(), xRoot);
            string appData = Application.persistentDataPath;
            string text = File.ReadAllText($"{appData}/PokeParadise_Data/xpAtLevel.xml");
            using (StringReader reader = new StringReader(text))
            {
                speeds = (List<LevelSpeed>)x.Deserialize(reader);
            }
            return speeds;
        }

        public static int XpToLevel(Pokemon p)
        {
            int xpToLevel = 0;
            foreach (LevelSpeed speed in FetchXpToLevel())
            {
                if (speed.levelSpeed == p.levelSpeed)
                {
                    xpToLevel = speed.xpLevels[p.level + 1];
                }
            }
            return xpToLevel;
        }

        public static int XpRemainingToLevel(Pokemon p)
        {
            return XpAtNextLevel(p) - p.xp;
        }

        public static int XpAtNextLevel(Pokemon p)
        {
            int xpAtLevel = 0;
            foreach (LevelSpeed speed in FetchXpAtLevel())
            {
                if (speed.levelSpeed == p.levelSpeed)
                {
                    xpAtLevel = speed.xpLevels[p.level + 1];
                }
            }
            return xpAtLevel;
        }

        public static int BaseXpGained(Pokemon p)
        {
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
            System.Random rand = new System.Random();
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
            return Convert.ToInt32(Math.Ceiling(multiplier * xpToLevel));
        }

        public static int EvoCheck(Pokemon p, string itemUsed = "", string location = "")
        {
            int evoDexNo = 0;
            foreach (Evolution evo in p.evolutions)
            {
                int evoTally = 0;
                int totalTally = 0;
                if (evo.evolutionTrigger.minLevel != null)
                {
                    totalTally++;
                    if (evo.evolutionTrigger.minLevel <= p.level)
                    {
                        evoTally++;
                    }
                }
                if (evo.evolutionTrigger.gender != null)
                {
                    totalTally++;
                    string genderToEvolve = "";
                    switch (evo.evolutionTrigger.gender)
                    {
                        case 1:
                            genderToEvolve = "female";
                            break;
                        case 2:
                            genderToEvolve = "male";
                            break;
                    }
                    if (p.gender == genderToEvolve)
                    {
                        evoTally++;
                    }
                }
                else if (evo.evolutionTrigger.item != null)
                {
                    totalTally++;
                    if (itemUsed == evo.evolutionTrigger.item)
                    {
                        evoTally++;
                    }
                }
                else if (evo.evolutionTrigger.minAffection != null)
                {
                    totalTally++;
                    if (p.affection >= evo.evolutionTrigger.minAffection)
                    {
                        evoTally++;
                    }
                }
                else if (evo.evolutionTrigger.minBeauty != null)
                {
                    totalTally++;
                    if (p.beauty >= evo.evolutionTrigger.minBeauty)
                    {
                        evoTally++;
                    }
                }
                else if (evo.evolutionTrigger.minHappiness != null)
                {
                    totalTally++;
                    if (p.friendship >= evo.evolutionTrigger.minHappiness)
                    {
                        evoTally++;
                    }
                }
                else if (evo.evolutionTrigger.needsOverworldRain)
                {
                    totalTally++;
                    GameVariables gv = GameVariables.FetchGameVariables();
                    if (gv.status == "raining")
                    {
                        evoTally++;
                    }
                    evoTally++;
                }
                else if (evo.evolutionTrigger.relativePhysicalStats != null)
                {
                    totalTally++;
                    switch (evo.evolutionTrigger.relativePhysicalStats)
                    {
                        case 1:
                            if (p.statAtk > p.statDef)
                            {
                                evoTally++;
                            }
                            break;
                        case -1:
                            if (p.statDef > p.statAtk)
                            {
                                evoTally++;
                            }
                            break;
                        case 0:
                            if (p.statAtk == p.statDef)
                            {
                                evoTally++;
                            }
                            break;
                    }
                }
                else if (evo.evolutionTrigger.turnUpsideDown)
                {
                    totalTally++;
                    GameVariables gv = GameVariables.FetchGameVariables();
                    if (gv.status == "upside-down")
                    {
                        evoTally++;
                    }
                }
                else if (evo.evolutionTrigger.heldItem != null)
                {
                    totalTally++;
                    if (itemUsed == evo.evolutionTrigger.heldItem)
                    {
                        evoTally++;
                    }
                }
                else if (evo.evolutionTrigger.knownMove != null)
                {
                    totalTally++;
                    //won't be working until I add moves for contests so for now bypass
                    evoTally++;
                }
                else if (evo.evolutionTrigger.knownMoveType != null)
                {
                    totalTally++;
                    //won't be working until I add moves for contests so for now bypass                    
                    evoTally++;
                }
                else if (evo.evolutionTrigger.partySpecies != null)
                {
                    totalTally++;
                    foreach (Pokemon pkmn in FetchCurrentPlayerData().shownPokemon)
                    {
                        if (string.Equals(pkmn.pkmnName, evo.evolutionTrigger.partySpecies, StringComparison.OrdinalIgnoreCase))
                        {
                            evoTally++;
                        }
                    }
                }
                else if (evo.evolutionTrigger.partyType != null)
                {
                    totalTally++;
                    foreach (Pokemon pkmn in FetchCurrentPlayerData().shownPokemon)
                    {
                        if (string.Equals(pkmn.type1, evo.evolutionTrigger.partyType, StringComparison.OrdinalIgnoreCase) || string.Equals(pkmn.type2, evo.evolutionTrigger.partyType, StringComparison.OrdinalIgnoreCase))
                        {
                            evoTally++;
                        }
                    }
                }
                else if (evo.evolutionTrigger.timeOfDay != null)
                {
                    totalTally++;
                    DateTimeOffset now = DateTimeOffset.Now;
                    switch (evo.evolutionTrigger.timeOfDay)
                    {
                        case "day":
                            if (now.Hour < 19 && now.Hour > 7)
                            {
                                evoTally++;
                            }
                            break;
                        case "night":
                            if (now.Hour > 19 && now.Hour < 7)
                            {
                                evoTally++;
                            }
                            break;
                    }
                }
                else if (evo.evolutionTrigger.location != null)
                {
                    totalTally++;
                    if (string.Equals(evo.evolutionTrigger.location, location, StringComparison.OrdinalIgnoreCase))
                    {
                        evoTally++;
                    }
                }
                else if (evo.evolutionTrigger.tradeSpecies != null)
                {
                    //can't do this until trading works, can't do trading without online. gonna be a while
                    totalTally++;
                    evoTally++;
                }
                if (evoTally == totalTally)
                {
                    evoDexNo = evo.evolveToDex;
                }
            }
            return evoDexNo;
        }

        public static Pokemon EvolutionHandler(Pokemon p, int evolveToDex)
        {
            Pokemon evolveTo = new Pokemon(evolveToDex);
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
            return p;
        }

        public static GameObject FindChildObj(Transform parent, string childName)
        {
            foreach (Transform obj in parent.transform)
            {
                if (childName == obj.gameObject.name)
                {
                    return obj.gameObject;
                }
            }
            return null;
        }

        public static void ClosePanels()
        {
            if (GameObject.Find("CookPredictor") != null)
            {
                GameObject.Find("CookPredictor").GetComponent<Text>().text = @"Berries Used: None

Current Flavor Totals
Spicy: 0 | Bitter: 0 | Sweet: 0 | Sour: 0 | Dry: 0";
            }
            if (GameObject.Find("PurchaseOKPanel") != null)
            {
                GameObject.Find("PurchaseOKPanel").SetActive(false);
            }
            if (GameObject.Find("InsuffFundsPanel") != null)
            {
                GameObject.Find("InsuffFundsPanel").SetActive(false);
            }
            if (GameObject.Find("TooltipPanel") != null)
            {
                GameObject.Find("TooltipPanel").SetActive(false);
            }
            if (GameObject.Find("MoveSelectPanel") != null)
            {
                GameObject.Find("MoveSelectPanel").SetActive(false);
            }
            if (GameObject.Find("MoveInfoPanel") != null)
            {
                GameObject.Find("MoveInfoPanel").SetActive(false);
            }
            GameObject panelHolder = GameObject.Find("PanelHolder");
            foreach (KeyValuePair<string, GameObject> kvp in panelHolder.GetComponent<PanelHolder>().panels)
            {
                kvp.Value.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}