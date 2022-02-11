using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PokeParadise
{
    public class OpenSwapPanel : MonoBehaviour, IPointerDownHandler
    {
        public Pokemon toSwap;
        public void OnPointerDown(PointerEventData eventData)
        {
            Player player = PlayerData.FetchCurrentPlayerData();
            GameObject panel = new GameObject();
            foreach (GameObject obj in PlayerData.GetAllObjectsOnlyInScene())
            {
                if (obj.name == "SwapPanel")
                {
                    obj.SetActive(true);
                }
            }
            foreach (GameObject obj in PlayerData.GetAllObjectsOnlyInScene())
            {
                if (obj.name == "SwapListPanel")
                {
                    Destroy(panel);
                    panel = obj;
                }
            }
            panel.SetActive(true);
            float x = -88f;
            float y = 142f;
            Vector2 centerAnchor = new Vector2(0.5f, 0.5f);
            Vector3 initialPosition = new Vector3(x, y);
            int cnt = 1;
            float size = 316;
            if (player.pokemon.Count / 11 * 35 > 316)
            {
                size = (float)(Math.Ceiling(Convert.ToDouble(player.pokemon.Count / 11)) * 35);
            }
            panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
            panel.GetComponent<RectTransform>().localPosition = new Vector3(panel.GetComponent<RectTransform>().localPosition.x, panel.GetComponent<RectTransform>().localPosition.y - (float)Math.Round(size / 2));
            foreach (Pokemon p in player.shownPokemon)
            {
                if (p != null)
                {
                    if (GameObject.Find("PCSwapSprite" + cnt) != null)
                    {
                        Destroy(GameObject.Find("PCSwapSprite" + cnt));
                    }
                    GameObject pokemon = new GameObject
                    {
                        name = "PCSwapSprite" + cnt
                    };
                    pokemon.AddComponent<Image>().sprite = Resources.Load<Sprite>(p.dexNo.ToString() + "_menu");
                    pokemon.AddComponent<BoxCollider2D>();
                    SwapShownPokemon swapper = pokemon.AddComponent<SwapShownPokemon>();
                    swapper.swapIndex = cnt - 1;
                    swapper.toSwap = toSwap;
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
                    else if (cnt % 5 == 0)
                    {
                        pokemon.transform.localPosition = new Vector3(x, y);
                        x = -88f;
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
        }
    }
}
