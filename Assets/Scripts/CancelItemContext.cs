using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokeParadise
{
    public class CancelItemContext : MonoBehaviour
    {
        public void Close()
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            foreach (GameObject obj in panels["BackpackPanel"].transform)
            {
                if (obj.name == "ItemContextPanel")
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}