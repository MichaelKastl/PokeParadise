using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokeParadise
{
    public class PCCancelHandler : MonoBehaviour
    {
        public void Cancel()
        {
            if (GameObject.Find("SwapPanel") != null)
            {
                GameObject.Find("SwapPanel").SetActive(false);
            }
            PlayerData.ClosePanels();
        }
    }
}