using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public static class StatsOpen
    {
        public static void Open(int id)
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            PlayerData.ClosePanels();
            panels["StatsWrapper"].transform.GetChild(0).gameObject.SetActive(true);
            Player player = PlayerData.FetchCurrentPlayerData();
            Pokemon p = player.pokemon.Where(x => x.id == id).First();
            GameObject.Find("ToughnessText").GetComponent<Text>().text = p.toughness.ToString();
            GameObject.Find("ClevernessText").GetComponent<Text>().text = p.cleverness.ToString();
            GameObject.Find("BeautyText").GetComponent<Text>().text = p.beauty.ToString();
            GameObject.Find("CutenessText").GetComponent<Text>().text = p.cuteness.ToString();
            GameObject.Find("CoolnessText").GetComponent<Text>().text = p.coolness.ToString();
        }
    }
}