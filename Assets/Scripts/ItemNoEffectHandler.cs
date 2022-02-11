using System.Collections.Generic;
using UnityEngine;

namespace PokeParadise
{
    public class ItemNoEffectHandler : MonoBehaviour
    {
        public void Close()
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            foreach (GameObject obj in panels["ItemUsePkmnWrapper"].transform.GetChild(0).transform)
            {
                if (obj.name == "ItemNoEffectPanel")
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}