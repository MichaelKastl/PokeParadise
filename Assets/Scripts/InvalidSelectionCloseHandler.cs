using System.Collections.Generic;
using UnityEngine;

namespace PokeParadise
{
    public class InvalidSelectionCloseHandler : MonoBehaviour
    {
        public void Close()
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            foreach (Transform obj in panels["KitchenWrapper"].transform.GetChild(0).transform)
            {
                if (obj.gameObject.name == "InvalidSelectionPanel")
                {
                    obj.gameObject.SetActive(false);
                }
            }
        }
    }
}
