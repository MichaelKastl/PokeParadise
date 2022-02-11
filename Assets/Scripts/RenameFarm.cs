using PokeParadise;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public class RenameFarm : MonoBehaviour
    {
        public void OpenDialog()
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            PlayerData.ClosePanels();
            panels["RenameFarmWrapper"].transform.GetChild(0).gameObject.SetActive(true);
        }

        public void Rename()
        {
            Player p = PlayerData.FetchCurrentPlayerData();
            p.breedingCenterName = GameObject.Find("RenameField").GetComponent<InputField>().text;
            PlayerData.SavePlayerData(p);
            GameObject.Find("FarmNameBox").GetComponent<Text>().text = p.breedingCenterName;
            GameObject.Find("RenameFarmPanel").SetActive(false);
        }
    }
}