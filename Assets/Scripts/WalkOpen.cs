using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokeParadise
{
    public static class WalkOpen
    {
        public static void Open(int id)
        {
            Pokemon pkmn = PlayerData.FetchCurrentPlayerData().pokemon.Find(x => x.id == id);
            if (string.IsNullOrEmpty(pkmn.lastWalked))
            {
                pkmn.lastWalked = DateTimeOffset.MinValue.ToString();
            }
            if (pkmn.walksThisHour < 3 || DateTimeOffset.Now - DateTimeOffset.Parse(pkmn.lastWalked) >= new TimeSpan(1, 0, 0))
            {
                if (DateTimeOffset.Now - DateTimeOffset.Parse(pkmn.lastWalked) >= new TimeSpan(1, 0, 0))
                {
                    Player player = PlayerData.FetchCurrentPlayerData();
                    foreach (Pokemon p in player.pokemon)
                    {
                        if (p.id == id)
                        {
                            p.walksThisHour = 0;
                        }
                    }
                    PlayerData.SavePlayerData(player);
                }
                Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
                panels["WalkWrapper"].transform.GetChild(0).gameObject.SetActive(true);
                GameObject.Find("WalkGoButtonHandler").GetComponent<WalkGoButtonHandler>().id = id;
            }
            else
            {
                Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
                panels["WalkWrapper"].transform.GetChild(0).gameObject.SetActive(false);
                panels["TooManyWalksWrapper"].transform.GetChild(0).gameObject.SetActive(true);
                panels["PokemonContextWrapper"].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}