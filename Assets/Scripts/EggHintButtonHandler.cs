using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokeParadise
{
    public class EggHintButtonHandler : MonoBehaviour
    {
        public void Close()
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            foreach (Transform obj in panels["IncubatorWrapper"].transform.GetChild(0).transform)
            {
                if (obj.gameObject.name == "EggHintPanel")
                {
                    obj.gameObject.SetActive(false);
                }
            }
        }
    }
}