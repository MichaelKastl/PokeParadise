using UnityEngine;

namespace PokeParadise
{
    public class BerryGuideCloseHandler : MonoBehaviour
    {
        public void Close()
        {
            GameObject.Find("BerryGuidePanel").SetActive(false);
        }
    }
}