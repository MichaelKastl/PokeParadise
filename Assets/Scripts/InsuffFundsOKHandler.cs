using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokeParadise
{
    public class InsuffFundsOKHandler : MonoBehaviour
    {
        public void Close()
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            foreach (GameObject obj in panels["StoreWrapper"].transform.GetChild(0).transform)
            {
                if (obj.name == "InsuffFundsPanel")
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
