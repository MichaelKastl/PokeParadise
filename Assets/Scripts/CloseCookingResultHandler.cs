using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokeParadise
{
    public class CloseCookingResultHandler : MonoBehaviour
    {
        public void Close()
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            panels["CookingResultWrapper"].transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
