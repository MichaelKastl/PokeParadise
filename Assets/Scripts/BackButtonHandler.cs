using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokeParadise
{
    public class BackButtonHandler : MonoBehaviour
    {
        public void GoBack()
        {
            foreach (GameObject obj in PlayerData.GetAllObjectsOnlyInScene())
            {
                if (obj.name == "NewGamePanel")
                {
                    obj.SetActive(false);
                }
                else if (obj.name == "ScenePanel")
                {
                    obj.SetActive(true);
                }
                else if (obj.name == "LoadGamePanel")
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
