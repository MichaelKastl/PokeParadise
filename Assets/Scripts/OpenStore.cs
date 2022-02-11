using PokeParadise.Classes;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public class OpenStore : MonoBehaviour
    {
        public void Open()
        {
            GameObject panel = new GameObject();
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            PlayerData.ClosePanels();
            panels["StoreWrapper"].transform.GetChild(0).gameObject.SetActive(true);
            foreach (Transform obj in panels["StoreWrapper"].transform.GetChild(0).transform)
            {
                if (obj.gameObject.name == "Scroll View")
                {
                    Destroy(panel);
                    panel = obj.transform.GetChild(0).transform.GetChild(0).gameObject;
                }
            }
            Dusty dusty = Dusty.LoadShop();
            Sprite sprite = Resources.Load<Sprite>("pokeball");
            const float x = 240f;
            float y = -30f;
            Vector2 centerAnchor = new Vector2(0.5f, 0.5f);
            Vector3 initialPosition = new Vector3(x, y);
            for (int i = 0; i <= dusty.inventory.Count; i++)
            {
                if (GameObject.Find("ItemSprite" + (i + 1)) != null)
                {
                    Destroy(GameObject.Find("ItemSprite" + (i + 1)));
                }
            }
            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0f, 0.5f);
            panelRect.anchorMax = new Vector2(1f, 0.5f);
            float size = 316;
            if ((dusty.inventory.Count * 50) + 30 > 316)
            {
                size = (float)Math.Ceiling(Convert.ToDouble((dusty.inventory.Count * 50) + 15));
            }
            panelRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
            panelRect.localPosition = new Vector3(panelRect.localPosition.x, panelRect.localPosition.y - (float)Math.Round(size / 2));
            int cnt = 0;
            foreach (Item i in dusty.inventory)
            {
                GameObject item = new GameObject
                {
                    name = "ItemSprite" + (cnt + 1)
                };
                item.AddComponent<Image>().sprite = sprite;
                item.transform.SetParent(panel.transform, false);
                item.transform.localScale = new Vector3(.35f, .35f);
                RectTransform itemRect = item.GetComponent<RectTransform>();
                itemRect.anchorMin = centerAnchor;
                itemRect.anchorMax = centerAnchor;
                GameObject text = new GameObject
                {
                    name = "ItemText" + (cnt + 1)
                };
                text.transform.SetParent(item.transform);
                RectTransform textRect = text.AddComponent<RectTransform>();
                textRect.anchorMin = centerAnchor;
                textRect.anchorMax = centerAnchor;
                text.transform.localScale = new Vector3(1f, 1f);
                Text txt = text.AddComponent<Text>();
                txt.font = Resources.Load<Font>("Fonts/PKMN RBYGSC");
                string name = "";
                if (i.berry != null)
                {
                    name = i.name + " Berry";
                }
                else if (i.food != null)
                {
                    name = i.name + " " + i.food.category;
                }
                else
                {
                    name = i.name;
                }
                txt.text = name;
                txt.alignment = TextAnchor.MiddleLeft;
                txt.color = Color.black;
                txt.fontSize = 30;
                textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 450);
                GameObject price = new GameObject();
                price.transform.SetParent(item.transform);
                Text priceText = price.AddComponent<Text>();
                priceText.text = "$" + i.price.ToString();
                priceText.font = Resources.Load<Font>("Fonts/PKMN RBYGSC");
                priceText.alignment = TextAnchor.MiddleLeft;
                priceText.color = Color.black;
                priceText.fontSize = 30;
                priceText.alignment = TextAnchor.MiddleCenter;
                RectTransform priceRect = price.GetComponent<RectTransform>();
                priceRect.localScale = new Vector3(1f, 1f);
                priceRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200);
                price.name = "ItemPrice" + (cnt + 1);
                GameObject button = Instantiate(Resources.Load<GameObject>("BuyButton"));
                button.transform.SetParent(item.transform);
                RectTransform buttonRect = button.GetComponent<RectTransform>();
                buttonRect.anchorMin = centerAnchor;
                buttonRect.anchorMax = centerAnchor;
                button.GetComponent<Button>().onClick.AddListener(delegate { BuyHandler.BuyItem(dusty.inventory.IndexOf(i)); });
                buttonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50);
                buttonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 165);
                button.transform.localScale = new Vector2(1f, 1f);
                if (cnt == 0)
                {
                    item.transform.localPosition = initialPosition;
                }
                else
                {
                    y -= 50;
                    item.transform.localPosition = new Vector3(x, y);
                }
                text.transform.localPosition = new Vector3(300f, -0.1f);
                button.transform.localPosition = new Vector3(975f, 0f);
                price.transform.localPosition = new Vector3(660f, -0.1f);
                cnt++;
            }
        }

        public void Close()
        {
            PlayerData.ClosePanels();
        }
    }
}