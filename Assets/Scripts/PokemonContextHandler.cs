using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public class PokemonContextHandler : MonoBehaviour
    {
        public int id;
        public void OnMouseDown()
        {
            Pokemon pokemon = PlayerData.FetchCurrentPlayerData().pokemon.Find(x => x.id == id);
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            Transform panel = panels["PokemonContextWrapper"].transform.GetChild(0);
            panel.gameObject.SetActive(true);
            GameObject hldrObj = new GameObject();
            foreach (Transform obj in panel)
            {
                if (obj.gameObject.name == "ContextHolder")
                {
                    Destroy(hldrObj);
                    hldrObj = obj.gameObject;
                }
            }
            ContextHolder holder = hldrObj.GetComponent<ContextHolder>();
            if (holder.objects == null || holder.objects.Count == 0)
            {
                holder.objects = new Dictionary<string, GameObject>();
                foreach (Transform obj in panel.transform)
                {
                    switch (obj.gameObject.name)
                    {
                        case "PokemonContextLabel":
                        case "DexButton":
                        case "StatsButton":
                        case "CareButton":
                        case "FeedButton":
                        case "WalkButton":
                        case "BreedButton":
                        case "MovesButton":
                        case "CancelContextButton":
                            holder.objects[obj.gameObject.name] = obj.gameObject;
                            break;
                    }
                }
            }
            holder.objects["PokemonContextLabel"].GetComponent<Text>().text = pokemon.pkmnName;
            Button dexButton = holder.objects["DexButton"].GetComponent<Button>();
            dexButton.onClick.RemoveAllListeners();
            dexButton.onClick.AddListener(delegate { DexButtonOpen.Open(id); });
            Button statsButton = holder.objects["StatsButton"].GetComponent<Button>();
            statsButton.onClick.RemoveAllListeners();
            statsButton.onClick.AddListener(delegate { StatsOpen.Open(id); });
            Button careButton = holder.objects["CareButton"].GetComponent<Button>();
            careButton.onClick.RemoveAllListeners();
            careButton.onClick.AddListener(delegate { CareOpen.Open(pokemon); });
            Button feedButton = holder.objects["FeedButton"].GetComponent<Button>();
            feedButton.onClick.RemoveAllListeners();
            feedButton.onClick.AddListener(delegate { FeedOpen.Open(id); });
            Button walkButton = holder.objects["WalkButton"].GetComponent<Button>();
            Button breedButton = holder.objects["BreedButton"].GetComponent<Button>();
            if (pokemon.isBreeding)
            {
                walkButton.onClick.RemoveAllListeners();
                walkButton.interactable = false;
                breedButton.onClick.RemoveAllListeners();
                breedButton.interactable = false;
            }
            else
            {
                walkButton.onClick.RemoveAllListeners();
                walkButton.interactable = true;
                breedButton.onClick.RemoveAllListeners();
                breedButton.interactable = true;
                walkButton.onClick.AddListener(delegate { WalkOpen.Open(id); });
                breedButton.onClick.AddListener(delegate { BreedOpen.Open(id); });
            }
            Button movesButton = holder.objects["MovesButton"].GetComponent<Button>();
            movesButton.onClick.RemoveAllListeners();
            movesButton.onClick.AddListener(delegate { MovesOpen.Open(id); });
            Button cancelContextButton = holder.objects["CancelContextButton"].GetComponent<Button>();
            cancelContextButton.onClick.RemoveAllListeners();
            cancelContextButton.onClick.AddListener(delegate { PokemonContextCancel.Close(); });
        }
    }
}
