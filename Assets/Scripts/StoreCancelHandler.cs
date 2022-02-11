using UnityEngine;

namespace PokeParadise
{
    public class StoreCancelHandler : MonoBehaviour
    {
        public void Cancel()
        {
            PlayerData.ClosePanels();
        }
    }
}