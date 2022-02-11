using PokeParadise.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public class OpenPC : MonoBehaviour
    {
        public void Open()
        {
            Player player = PlayerData.FetchCurrentPlayerData();
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            PlayerData.ClosePanels();
            panels["PCWrapper"].transform.GetChild(0).gameObject.SetActive(true);
            GameObject panel = new GameObject();
            foreach (Transform obj in panels["PCWrapper"].transform.GetChild(0).transform)
            {
                if (obj.gameObject.name == "Scroll View")
                {
                    Destroy(panel);
                    panel = obj.transform.GetChild(0).transform.GetChild(1).gameObject;
                }
            }
            float x = -190.8f;
            float y = 130f;
            Vector2 centerAnchor = new Vector2(0.5f, 0.5f);
            Vector3 initialPosition = new Vector3(x, y);
            int cnt = 1;
            float size = 316;
            if (player.pokemon.Count / 11 * 35 > 316)
            {
                size = (float)(Math.Ceiling(Convert.ToDouble(player.pokemon.Count / 11)) * 35);
            }
            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
            panelRect.localPosition = new Vector3(panelRect.localPosition.x, panelRect.localPosition.y - (float)Math.Round(size / 2));
            foreach (Pokemon p in player.pokemon)
            {
                if (GameObject.Find("PCSprite" + cnt) != null)
                {
                    Destroy(GameObject.Find("PCSprite" + cnt));
                }
                GameObject pokemon = new GameObject
                {
                    name = "PCSprite" + cnt
                };
                pokemon.AddComponent<Image>().sprite = Resources.Load<Sprite>(p.dexNo.ToString() + "_menu");
                pokemon.AddComponent<BoxCollider2D>();
                pokemon.AddComponent<OpenSwapPanel>().toSwap = p;
                pokemon.transform.SetParent(panel.transform, false);
                pokemon.transform.localScale = new Vector3(.2f, .2f);
                RectTransform pokemonRect = pokemon.GetComponent<RectTransform>();
                pokemonRect.anchorMin = centerAnchor;
                pokemonRect.anchorMax = centerAnchor;
                if (cnt == 1)
                {
                    pokemon.transform.localPosition = initialPosition;
                    x += 35;
                }
                else if (cnt % 11 == 0)
                {
                    pokemon.transform.localPosition = new Vector3(x, y);
                    x = -190.8f;
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

        public void Close()
        {
            PlayerData.ClosePanels();
        }
    }
}