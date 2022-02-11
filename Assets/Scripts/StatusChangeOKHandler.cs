using UnityEngine;

namespace PokeParadise
{
    public class StatusChangeOKHandler : MonoBehaviour
    {
        public void Close()
        {
            GameObject.Find("StatusChangePanel").SetActive(false);
            PlayerData.ClosePanels();
        }
    }
}