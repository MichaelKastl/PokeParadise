using UnityEngine;

namespace PokeParadise
{
    public class AnnounceCloseHandler : MonoBehaviour
    {
        public void CloseAll()
        {
            if (GameObject.Find("EvoAnnouncePanel") != null)
            {
                GameObject.Find("EvoAnnouncePanel").SetActive(false);
            }
            if (GameObject.Find("LevelAnnouncePanel") != null)
            {
                GameObject.Find("LevelAnnouncePanel").SetActive(false);
            }
        }
    }
}
