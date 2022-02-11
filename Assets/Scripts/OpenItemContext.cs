using PokeParadise.Classes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public class OpenItemContext : MonoBehaviour
    {
        public int itemUsed;
        public void OnMouseDown()
        {
            Player player = PlayerData.FetchCurrentPlayerData();
            Item item = player.inventory.Where(x => x.id == itemUsed).ToList()[0];
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            foreach (Transform obj in panels["BackpackWrapper"].transform.GetChild(0).transform)
            {
                if (obj.gameObject.name == "ItemContextPanel")
                {
                    obj.gameObject.SetActive(true);
                }
            }
            GameObject.Find("ItemUseLabel").GetComponent<Text>().text = item.name;
            GameObject.Find("ItemUseHandler").GetComponent<ItemUseHandler>().itemUsed = itemUsed;
        }
    }
}