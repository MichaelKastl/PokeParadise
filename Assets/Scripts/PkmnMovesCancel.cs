using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokeParadise
{
    public class PkmnMovesCancel : MonoBehaviour
    {
        public void Cancel()
        {
            if (GameObject.Find("MoveSelectPanel") != null)
            {
                GameObject.Find("MoveSelectPanel").SetActive(false);
            }
            if (GameObject.Find("MoveInfoPanel") != null)
            {
                GameObject.Find("MoveInfoPanel").SetActive(false);
            }
            PlayerData.ClosePanels();
        }
    }
}