using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using PokeParadise;
using static System.Environment;

namespace PokeParadise
{
    public class NewGameCreate : MonoBehaviour
    {
        public void Begin()
        {
            GameObject oldUI = GameObject.Find("ScenePanel");
            GameObject panel = new GameObject();
            foreach (GameObject obj in PlayerData.GetAllObjectsOnlyInScene())
            {
                if (obj.name == "NewGamePanel")
                {
                    Destroy(panel);
                    panel = obj;
                }
            }
            oldUI.SetActive(false);
            panel.SetActive(true);
        }

        public void LoadGameBegin()
        {
            GameObject oldUI = GameObject.Find("ScenePanel");
            foreach (GameObject obj in PlayerData.GetAllObjectsOnlyInScene())
            {
                if (obj.name == "LoadGamePanel")
                {
                    obj.SetActive(true);
                }
            }
            oldUI.SetActive(false);
            Dropdown dropdown = GameObject.Find("LoadGameSelect").GetComponent<Dropdown>();
            List<string> players = new List<string>();
            string appData = Application.persistentDataPath;
            foreach (string file in Directory.GetFiles($"{appData}/PokeParadise_Saves"))
            {
                FileInfo fi = new FileInfo(file);
                string saveName = fi.Name;
                if (saveName != "current.xml")
                {
                    players.Add(saveName.Replace("-savedata.xml", ""));
                }
            }
            dropdown.ClearOptions();
            dropdown.AddOptions(players);
        }

        public void CreateNewGame()
        {
            GameObject inputFieldGo = GameObject.Find("NameField");
            InputField input = inputFieldGo.GetComponent<InputField>();
            Player p = new Player(input.text)
            {
                coins = 500
            };
            System.Random rand = new System.Random();
            int dex = rand.Next(1, 807);
            p.eggs.Add(new Egg(dex, System.DateTimeOffset.Now));
            PlayerData.SavePlayerData(p);

            //Without reference variable
            GameObject objectWithScript = GameObject.Find("LoadMainSceneObj");
            objectWithScript.GetComponent<MainSceneLoad>().MainScene(p);
        }
    }
}