using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public static class CareOpen
    {
        public static void Open(Pokemon p)
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            PlayerData.ClosePanels();
            panels["CareWrapper"].transform.GetChild(0).gameObject.SetActive(true);
            GameObject img = GameObject.Find("CarePanelImage");
            img.GetComponent<Image>().sprite = Resources.Load<Sprite>("Artwork/" + p.dexNo);
            if (img.GetComponent<PetHandler>() != null)
            {
                img.GetComponent<PetHandler>().id = p.id;
            }
            else
            {
                img.AddComponent<PetHandler>().id = p.id;
            }
        }

        public static void Close()
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            PlayerData.ClosePanels();
            panels["CareWrapper"].transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
