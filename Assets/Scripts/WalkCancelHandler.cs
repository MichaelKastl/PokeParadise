using UnityEngine;

namespace PokeParadise
{
    public class WalkCancelHandler : MonoBehaviour
    {
        public void Cancel()
        {
            PlayerData.ClosePanels();
        }
    }
}