using UnityEngine;

namespace PokeParadise
{
    public class FoodContextCloseHandler : MonoBehaviour
    {
        public void Close()
        {
            GameObject.Find("FoodContextPanel").SetActive(false);
        }
    }
}
