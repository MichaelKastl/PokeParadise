using UnityEngine;

namespace PokeParadise
{
    public class EscapeHandler : MonoBehaviour
    {
        // Update is called once per frame
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PlayerData.ClosePanels();
            }
        }
    }
}