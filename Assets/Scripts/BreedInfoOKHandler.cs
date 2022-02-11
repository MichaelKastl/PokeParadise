using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokeParadise
{
    public class BreedInfoOKHandler : MonoBehaviour
    {
        public void Close()
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            foreach (Transform obj in panels["BreedWrapper"].transform.GetChild(0).transform)
            {
                if (obj.gameObject.name == "BreedButton")
                {
                    obj.gameObject.SetActive(true);
                }
            }
            GameObject.Find("BreedInfoPanel").SetActive(false);
            panels["BreedWrapper"].transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
