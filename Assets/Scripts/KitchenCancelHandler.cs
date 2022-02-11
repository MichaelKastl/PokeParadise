using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public class KitchenCancelHandler : MonoBehaviour
    {
        public void Cancel()
        {
            GameObject.Find("CookPredictor").GetComponent<Text>().text = @"Berries Used: None

Current Flavor Totals
Spicy: 0 | Bitter: 0 | Sweet: 0 | Sour: 0 | Dry: 0";
            PlayerData.ClosePanels();
        }
    }
}