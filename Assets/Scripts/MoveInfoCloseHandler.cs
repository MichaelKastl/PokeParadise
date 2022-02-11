using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokeParadise
{
    public class MoveInfoCloseHandler : MonoBehaviour
    {
        public void Close()
        {
            GameObject.Find("MoveInfoPanel").SetActive(false);
        }
    }
}