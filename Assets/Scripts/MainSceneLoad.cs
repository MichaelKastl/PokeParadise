using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

namespace PokeParadise
{
    public class MainSceneLoad : MonoBehaviour
    {
        public string pName = "";
        public Player player;

        public void Start()
        {
            if (SceneManager.GetActiveScene().name == "IntroScene")
            {
                System.Random rand = new System.Random();
                for (int cnt = 1; cnt <= 21; cnt++)
                {
                    GameObject introSprite = GameObject.Find("IntroSprite" + cnt);
                    int dexNo = rand.Next(1, 807);
                    if (dexNo < 10)
                    {
                        introSprite.GetComponent<SpriteSwap>().SpriteSheetName = "00" + dexNo;
                    }
                    else if (dexNo < 100)
                    {
                        introSprite.GetComponent<SpriteSwap>().SpriteSheetName = "0" + dexNo;
                    }
                    else
                    {
                        introSprite.GetComponent<SpriteSwap>().SpriteSheetName = dexNo.ToString();
                    }
                }
            }
        }

        public void MainScene(Player player)
        {
            this.player = player;
            pName = player.name;
            for (int cnt = 1; cnt <= 21; cnt++)
            {
                Destroy(GameObject.Find("IntroSprite" + cnt));
            }
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("MainScene");
        }
        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            foreach (GameObject obj in PlayerData.GetAllObjectsOnlyInScene())
            {
                if (obj.name == "NamePanel")
                {
                    obj.GetComponentInChildren<Text>().text = pName;
                }
                else if (obj.name == "FarmNamePanel")
                {
                    obj.GetComponentInChildren<Text>().text = player.breedingCenterName;
                }
                else if (obj.name == "CurrencyTotal")
                {
                    obj.GetComponent<Text>().text = "$" + player.coins;
                }
            }
            if (scene.name == "MainScene")
            {
                PlayerData.LoadPokemonMenuSprites(PlayerData.FetchCurrentPlayerData().shownPokemon);
            }
        }
        public void IntroScene()
        {
            SceneManager.LoadScene("IntroScene");
        }
        public void NewGameScene()
        {
            SceneManager.LoadScene("NewGameScene");
        }
    }
}