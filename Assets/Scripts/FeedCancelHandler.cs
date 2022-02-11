using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokeParadise
{
    public class FeedCancelHandler : MonoBehaviour
    {
        public void Cancel()
        {
            PlayerData.ClosePanels();
        }
    }

}