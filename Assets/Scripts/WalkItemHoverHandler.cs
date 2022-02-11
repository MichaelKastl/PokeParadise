using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public class WalkItemHoverHandler : MonoBehaviour
    {
        public string tooltipString;
        public void OnMouseEnter()
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            panels["TooltipWrapper"].transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("ToolTipText").GetComponent<Text>().text = tooltipString;
        }

        public void OnMouseExit()
        {
            GameObject.Find("ToolTipText").GetComponent<Text>().text = "";
            GameObject.Find("TooltipPanel").SetActive(false);
        }
    }
}
