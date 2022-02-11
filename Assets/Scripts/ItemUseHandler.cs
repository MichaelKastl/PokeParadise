using UnityEngine;
using PokeParadise.Classes;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace PokeParadise
{
    public class ItemUseHandler : MonoBehaviour
    {
        public int itemUsed;
        public void Use()
        {
            Player player = PlayerData.FetchCurrentPlayerData();
            Item item = player.inventory.Where(x => x.id == itemUsed).ToList()[0];
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            if (item.name != "Upside-Down Button" && item.name != "Rainmaker")
            {
                GameObject panel = panels["ItemUsePkmnWrapper"].transform.GetChild(0).gameObject;
                panels["BackpackWrapper"].transform.GetChild(0).gameObject.SetActive(false);
                foreach (Transform obj in panel.transform)
                {
                    if (obj.gameObject.name == "ItemUsePkmnHandler")
                    {
                        obj.gameObject.GetComponent<ItemUsePkmnHandler>().itemUsed = itemUsed;
                    }
                }
                float x = -150f;
                float y = 83f;
                Vector2 centerAnchor = new Vector2(0.5f, 0.5f);
                int cnt = 1;
                float size = 316;
                if (player.pokemon.Count / 9 * 35 > 316)
                {
                    size = (float)(Math.Ceiling(Convert.ToDouble(player.pokemon.Count / 9)) * 35);
                }
                panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
                x = 20f;
                y = -20f;
                Vector3 initialPosition = new Vector3(x, y);
                panel.SetActive(true);
                foreach (Pokemon p in player.pokemon)
                {
                    if (GameObject.Find("ItemUseSprite" + p.id) != null)
                    {
                        Destroy(GameObject.Find("ItemUseSprite" + p.id));
                    }
                    GameObject pokemon = new GameObject
                    {
                        name = "ItemUseSprite" + p.id
                    };
                    pokemon.AddComponent<Image>().sprite = Resources.Load<Sprite>(p.dexNo.ToString() + "_menu");
                    pokemon.AddComponent<Button>().onClick.AddListener(delegate { TogglePokemon(p, pokemon, player.pokemon); });
                    pokemon.AddComponent<BoxCollider2D>();
                    pokemon.transform.SetParent(GameObject.Find("ItemUsePkmnListPanel").transform, false);
                    pokemon.transform.localScale = new Vector3(.2f, .2f);
                    RectTransform pokemonRect = pokemon.GetComponent<RectTransform>();
                    pokemonRect.anchorMin = centerAnchor;
                    pokemonRect.anchorMax = centerAnchor;
                    if (cnt == 1)
                    {
                        pokemon.transform.localPosition = initialPosition;
                        x += 35;
                    }
                    else if (cnt % 9 == 0)
                    {
                        pokemon.transform.localPosition = new Vector3(x, y);
                        x = 20f;
                        y -= 35;
                    }
                    else
                    {
                        pokemon.transform.localPosition = new Vector3(x, y);
                        x += 35;
                    }
                    cnt++;
                }
            }
            else
            {
                GameVariables gv = GameVariables.FetchGameVariables();
                foreach (Transform obj in panels["BackpackWrapper"].transform.GetChild(0).transform)
                {
                    if (obj.gameObject.name == "StatusChangePanel")
                    {
                        obj.gameObject.SetActive(true);
                    }
                }
                GameObject panel = GameObject.Find("StatusChangeText");
                Text panelTxt = panel.GetComponent<Text>();
                switch (item.name)
                {
                    case "Upside-Down Button":
                        gv.status = "upside-down";
                        panelTxt.text = "The room has turned upside down!";
                        break;
                    case "Rainmaker":
                        gv.status = "raining";
                        panelTxt.text = "It's begun raining!";
                        break;
                }
                player.inventory.RemoveAll(x => x.id == itemUsed);
                PlayerData.SavePlayerData(player);
                panel.SetActive(true);
                gv.SaveGameVariables();
            }
        }

        public static void TogglePokemon(Pokemon pokemon, GameObject obj, List<Pokemon> playerPokemon)
        {
            obj.GetComponent<Button>().interactable = false;
            foreach (Pokemon p in playerPokemon)
            {
                if (p.id == pokemon.id)
                {
                    GameObject.Find("ItemUseSprite" + p.id).GetComponent<Button>().interactable = false;
                }
                else if (GameObject.Find("ItemUseSprite" + p.id) != null)
                {
                    GameObject.Find("ItemUseSprite" + p.id).GetComponent<Button>().interactable = true;
                }
            }
        }

        public static void Close()
        {
            PlayerData.ClosePanels();
        }
    }
}