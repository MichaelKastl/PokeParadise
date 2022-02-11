using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokeParadise
{
    public class WalkResultsClose : MonoBehaviour
    {
        public void Close()
        {
            GameObject.Find("WalkResultsPanel").SetActive(false);
        }

        public void CloseError()
        {
            GameObject.Find("TooManyWalksPanel").SetActive(false);
        }
    }
}
