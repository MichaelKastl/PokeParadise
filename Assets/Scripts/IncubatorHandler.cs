using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public class IncubatorHandler : MonoBehaviour
    {
        public void OpenIncubator()
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            PlayerData.ClosePanels();
            GameObject panel = panels["IncubatorWrapper"].transform.GetChild(0).gameObject;
            Player p = PlayerData.FetchCurrentPlayerData();
            panel.SetActive(true);
            int cnt = 0;
            Vector3 initialPosition = new Vector3(-185, 115);
            int x = -185;
            int y = 115;
            Sprite sprite = Resources.Load<Sprite>("egg");
            foreach (Egg e in p.eggs)
            {
                Egg egg = e;
                if (GameObject.Find("EggSprite" + cnt) != null)
                {
                    Destroy(GameObject.Find("EggSprite" + cnt));
                }
                GameObject go = new GameObject("EggSprite" + cnt);
                SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
                go.transform.parent = panel.transform;
                renderer.sprite = sprite;
                go.GetComponent<SpriteRenderer>().sortingLayerName = "UI - Overlay";
                go.AddComponent<BoxCollider2D>();
                go.AddComponent<EggHint>().egg = egg;
                go.transform.localScale = new Vector3(70, 70);
                if (cnt == 0)
                {
                    go.transform.localPosition = initialPosition;
                }
                else if (cnt % 10 != 0)
                {
                    x += 35;
                    go.transform.localPosition = new Vector3(x, y);
                }
                else
                {
                    x = -185;
                    y -= 35;
                    go.transform.localPosition = new Vector3(x, y);
                }
                go.SetActive(true);
                cnt++;
            }
        }
    }
}