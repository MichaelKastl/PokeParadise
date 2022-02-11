using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public static class BreedOpen
    {
        public static void Open(int id)
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            PlayerData.ClosePanels();
            panels["BreedWrapper"].transform.GetChild(0).gameObject.SetActive(true);
            Player player = PlayerData.FetchCurrentPlayerData();
            Pokemon pkmn = player.pokemon.Find(x => x.id == id);
            GameObject panel = GameObject.Find("BreedMatesPanel");
            float x = -150f;
            float y = 83f;
            Vector2 centerAnchor = new Vector2(0.5f, 0.5f);
            int cnt = 1;
            float size = 316;
            if (player.pokemon.Count / 9 * 35 > 316)
            {
                size = (float)(Math.Ceiling(Convert.ToDouble(player.pokemon.FindAll(i => (i.eggGroup1 == pkmn.eggGroup1 || i.eggGroup2 == pkmn.eggGroup1 || i.eggGroup1 == pkmn.eggGroup2 || i.eggGroup2 == pkmn.eggGroup2) && i.gender != pkmn.gender).Count / 9)) * 35);
            }
            panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
            x = 20f;
            y = -20f;
            Vector3 initialPosition = new Vector3(x, y);
            GameObject.Find("BreedHandler").GetComponent<BreedButtonHandler>().start = pkmn;
            panel.SetActive(true);
            foreach (Transform child in panel.transform)
            {
                UnityEngine.Object.Destroy(child.gameObject);
            }
            var xyz = player.pokemon.FindAll(i => (i.eggGroup1 == pkmn.eggGroup1 || i.eggGroup2 == pkmn.eggGroup1 || i.eggGroup1 == pkmn.eggGroup2 || i.eggGroup2 == pkmn.eggGroup2) && i.gender != pkmn.gender && !i.isBreeding);
            foreach (Pokemon p in player.pokemon.FindAll(i => (i.eggGroup1 == pkmn.eggGroup1 || i.eggGroup2 == pkmn.eggGroup1 || i.eggGroup1 == pkmn.eggGroup2 || i.eggGroup2 == pkmn.eggGroup2) && i.gender != pkmn.gender && !i.isBreeding))
            {
                GameObject pokemon = new GameObject
                {
                    name = "BreedSprite_" + p.id
                };
                pokemon.AddComponent<Image>().sprite = Resources.Load<Sprite>(p.dexNo.ToString() + "_menu");
                pokemon.AddComponent<Button>().onClick.AddListener(delegate { TogglePokemon(p, pokemon, player.pokemon); });
                pokemon.AddComponent<BoxCollider2D>();
                pokemon.transform.SetParent(GameObject.Find("BreedMatesPanel").transform, false);
                pokemon.transform.localScale = new Vector3(.2f, .2f);
                RectTransform pkmnRect = pokemon.GetComponent<RectTransform>();
                pkmnRect.anchorMin = centerAnchor;
                pkmnRect.anchorMax = centerAnchor;
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

        public static void TogglePokemon(Pokemon pokemon, GameObject obj, List<Pokemon> playerPokemon)
        {
            obj.GetComponent<Button>().interactable = false;
            foreach (Pokemon p in playerPokemon)
            {
                if (p.id == pokemon.id)
                {
                    GameObject.Find("BreedSprite_" + p.id).GetComponent<Button>().interactable = false;
                }
                else if (GameObject.Find("BreedSprite_" + p.id) != null)
                {
                    GameObject.Find("BreedSprite_" + p.id).GetComponent<Button>().interactable = true;
                }
            }
        }

        public static void Close()
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            PlayerData.ClosePanels();
            panels["BreedWrapper"].transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
