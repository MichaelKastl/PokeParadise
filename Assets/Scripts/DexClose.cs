using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public class DexClose : MonoBehaviour
    {
        public void Close()
        {
            PlayerData.ClosePanels();
        }
    }
}