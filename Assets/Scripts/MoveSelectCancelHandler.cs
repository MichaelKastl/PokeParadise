using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokeParadise
{
    public class MoveSelectCancelHandler : MonoBehaviour
    {
        public void Close()
        {
            GameObject.Find("MoveSelectPanel").SetActive(false);
        }
    }
}