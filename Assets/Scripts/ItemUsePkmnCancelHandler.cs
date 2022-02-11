using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokeParadise
{
    public class ItemUsePkmnCancelHandler : MonoBehaviour
    {
        public void Cancel()
        {
            PlayerData.ClosePanels();
        }
    }
}