using PokeParadise.Classes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public static class FeedOpen
    {
        public static void Open(int toFeed)
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            PlayerData.ClosePanels();
            panels["FeedWrapper"].transform.GetChild(0).gameObject.SetActive(true);
            Player player = PlayerData.FetchCurrentPlayerData();
            GameObject panel = GameObject.Find("FeedItemsPanel");
            GameObject.Find("FeedPanelImage").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Artwork/" + player.pokemon.Find(x => x.id == toFeed).dexNo);
            if (player.inventory.FindAll(x => x.food != null).Count > 0 && player.inventory.FindAll(x => x.food != null && x.food.category != "").Count >= 8)
            {
                panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (35 * player.inventory.FindAll(x => x.food != null && x.food.category != "").Count) + 15);
            }
            Vector2 centerAnchor = new Vector2(0.5f, 0.5f);
            const float x = 27f;
            float y = -20f;
            Vector3 initialPosition = new Vector3(x, y);
            int cnt = 1;
            foreach (Item i in player.inventory)
            {
                if (GameObject.Find("FeedItem" + cnt + "Sprite") != null)
                {
                    Object.Destroy(GameObject.Find("FeedItem" + cnt + "Sprite"));
                }
                if (i.food != null && i.food.category != "")
                {
                    GameObject item = new GameObject
                    {
                        name = "FeedItem" + cnt + "Sprite"
                    };
                    string name = "";
                    if (i.food?.category == "Pokeblock")
                    {
                        if (i.name.Contains("+"))
                        {
                            name = i.food?.category.ToLower() + "_" + i.name.ToLower().Replace("+", "_plus");
                        }
                        else
                        {
                            name = i.food?.category.ToLower() + "_" + i.name.ToLower();
                        }
                    }
                    else if (i.food?.category == "Poffin")
                    {
                        name = i.food?.category + "_" + i.name;
                    }
                    else if (i.food?.category == "Pokepuff")
                    {
                        name = i.food?.category.ToLower() + "_" + i.name.Substring(0, i.name.IndexOf(" ")).ToLower();
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
                    item.transform.SetParent(panel.transform, false);
                    item.transform.localScale = new Vector3(0.3f, 0.3f);
                    RectTransform itemRect = item.GetComponent<RectTransform>();
                    itemRect.anchorMin = centerAnchor;
                    itemRect.anchorMax = centerAnchor;
                    item.transform.localPosition = new Vector3(x, y);
                    item.AddComponent<BoxCollider2D>();
                    FoodClickHandler handler = item.AddComponent<FoodClickHandler>();
                    handler.item = i;
                    handler.toFeed = toFeed;
                    y -= 35;
                    cnt++;
                }
            }
        }

        public static void Close()
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            panels["FeedWrapper"].transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
