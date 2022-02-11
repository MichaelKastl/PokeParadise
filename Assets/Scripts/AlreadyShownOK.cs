using UnityEngine;

namespace PokeParadise
{
    public class AlreadyShownOK : MonoBehaviour
    {
        public void CloseAlreadyShownPanel()
        {
            GameObject.Find("AlreadyShownPanel").SetActive(false);
        }
    }
}
