using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokeParadise
{
    public class StatsClose : MonoBehaviour
    {
        public void Close()
        {
            if (GameObject.Find("StatsPanel") != null)
            {
                GameObject.Find("StatsPanel").SetActive(false);
            }
            PlayerData.ClosePanels();
        }
    }
}