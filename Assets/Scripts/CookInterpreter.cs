using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PokeParadise.Classes;
using UnityEngine.UI;

namespace PokeParadise
{
    public class CookInterpreter : MonoBehaviour
    {
        public void Interpret(string foodType)
        {
            List<int> itemIds = new List<int>();
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            foreach (Transform obj in panels["KitchenWrapper"].transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0))
            {
                if (obj.gameObject.name.Contains("BerrySprite") && obj.GetComponent<KitchenBerryIsSelected>().isSelected)
                {
                    itemIds.Add(Convert.ToInt32(obj.gameObject.name.Replace("BerrySprite", "")));
                }
            }
            List<Item> ings = new List<Item>();
            foreach (Item i in PlayerData.FetchCurrentPlayerData().inventory)
            {
                if (itemIds.Contains(i.id))
                {
                    ings.Add(i);
                }
            }
            KitchenHandler.Cook(ings, foodType);
        }
    }
}
