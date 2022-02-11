using PokeParadise.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public static class BuyHandler
    {
        public static void BuyItem(int index)
        {
            Player player = PlayerData.FetchCurrentPlayerData();
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            Dusty dusty = Dusty.LoadShop();
            Item i = dusty.inventory[index];
            if (player.inventory.Count == 0)
            {
                i.id = 1;
            }
            else
            {
                i.id = player.inventory.Max(x => x.id) + 1;
            }
            if (player.coins >= i.price)
            {
                if (i.egg != null)
                {
                    i.egg.obtained = DateTimeOffset.Now.ToString();
                    player.eggs.Add(i.egg);
                    dusty.inventory[index] = new Item(new Egg(DateTimeOffset.Now, 0), 1);
                    dusty.SaveShop();
                }
                else
                {
                    player.inventory.Add(i);
                }
                player.coins -= i.price;
                PlayerData.SavePlayerData(player);
                GameObject panel = new GameObject();
                GameObject.Find("CurrencyTotal").GetComponent<Text>().text = "$" + player.coins;
                foreach (Transform obj in panels["StoreWrapper"].transform.GetChild(0).transform)
                {
                    if (obj.gameObject.name == "PurchaseOKPanel")
                    {
                        obj.gameObject.SetActive(true);
                        UnityEngine.Object.Destroy(panel);
                        panel = obj.gameObject;
                    }
                }
                foreach (Transform obj in panel.transform)
                {
                    if (obj.gameObject.name == "PurchaseOKBlurb")
                    {
                        if (i.egg != null)
                        {
                            obj.gameObject.GetComponent<Text>().text = "You've purchased an egg! It will hatch in " + i.egg.timeToHatch;
                        }
                        else
                        {
                            obj.gameObject.GetComponent<Text>().text = "Purchase complete! Check your backpack to find your new item.";
                        }
                    }
                }
            }
            else
            {
                foreach (Transform obj in panels["StoreWrapper"].transform)
                {
                    if (obj.gameObject.name == "InsuffFundsPanel")
                    {
                        obj.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
