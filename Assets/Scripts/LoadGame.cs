using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public class LoadGame : MonoBehaviour
    {
        public void LoadFile()
        {
            Dropdown dropdown = GameObject.Find("LoadGameSelect").GetComponent<Dropdown>();
            PlayerData.name = dropdown.options[dropdown.value].text;
            PlayerData.player = PlayerData.FetchPlayerData(PlayerData.name);
            PlayerData.SavePlayerData(PlayerData.player);
            GameObject objectWithScript = GameObject.Find("LoadMainSceneObj");

            objectWithScript.GetComponent<MainSceneLoad>().MainScene(PlayerData.player);
        }
    }
}