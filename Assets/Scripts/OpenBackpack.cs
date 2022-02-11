using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PokeParadise.Classes;
using UnityEngine.UI;
using System;

namespace PokeParadise
{
    public class OpenBackpack : MonoBehaviour
    {
        public void Open()
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            PlayerData.ClosePanels();
            panels["BackpackWrapper"].transform.GetChild(0).gameObject.SetActive(true);
            Player player = PlayerData.FetchCurrentPlayerData();
            GameObject panel = GameObject.Find("BackpackItemsPanel");
            if (player.inventory.Count >= 8)
            {
                panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)(35 * Math.Ceiling(player.inventory.Count / 9d)) + 15);
            }
            Vector2 centerAnchor = new Vector2(0.5f, 0.5f);
            float x = 30f;
            float y = -20f;
            Vector3 initialPosition = new Vector3(x, y);
            int cnt = 1;
            foreach (Item i in player.inventory)
            {
                if (GameObject.Find("Item" + cnt + "Sprite") != null)
                {
                    Destroy(GameObject.Find("Item" + cnt + "Sprite"));
                }
                GameObject item = new GameObject
                {
                    name = "Item" + cnt + "Sprite"
                };
                item.transform.SetParent(panel.transform);
                string name = "";
                string itemName = "";
                if (i.food != null && i.food.category != "")
                {
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
                        itemName = i.food.type + " " + i.food.category;
                    }
                    else if (i.food.category == "Poffin")
                    {
                        string poffinName = i.name;
                        if (i.name.Contains("-"))
                        {
                            poffinName = i.name.Replace("-", "_");
                        }
                        name = i.food.category + "_" + poffinName;
                        name = name.ToLower();
                        itemName = i.food.type + " " + i.food.category;
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
                    itemName = i.food.type + " " + i.food.category;
                }
                else if (i.berry != null && i.berry.color != "")
                {
                    name = i.berry.name.ToLower();
                    item.AddComponent<Image>().sprite = Resources.Load<Sprite>("Food/" + name);
                    itemName = i.name + " Berry";
                }
                else
                {
                    item.AddComponent<Image>().sprite = Resources.Load<Sprite>("pokeball");
                    item.AddComponent<OpenItemContext>().itemUsed = i.id;
                    itemName = i.name;
                }
                RectTransform itemRect = item.GetComponent<RectTransform>();
                itemRect.anchorMin = centerAnchor;
                itemRect.anchorMax = centerAnchor;
                item.transform.localScale = new Vector3(0.3f, 0.3f);
                item.AddComponent<BoxCollider2D>().size = new Vector2(100f, 100f);
                item.AddComponent<WalkItemHoverHandler>().tooltipString = itemName;
                if (cnt == 1)
                {
                    item.transform.localPosition = initialPosition;
                    x += 35;
                }
                else if (cnt % 9 == 0)
                {
                    item.transform.localPosition = new Vector3(x, y);
                    x = 30f;
                    y -= 35;
                }
                else
                {
                    item.transform.localPosition = new Vector3(x, y);
                    x += 35;
                }
                cnt++;
            }
        }
    }
}
