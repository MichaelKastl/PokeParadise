using UnityEngine;

namespace PokeParadise
{
    public class RenameCancelHandler : MonoBehaviour
    {
        public void Cancel()
        {
            PlayerData.ClosePanels();
        }
    }
}