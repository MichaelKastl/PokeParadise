using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokeParadise
{
    public class PanelHolder : MonoBehaviour
    {
        public Dictionary<string, GameObject> panels;
        // Start is called before the first frame update
        public void Start()
        {
            if (panels == null || panels == new Dictionary<string, GameObject>())
            {
                panels = new Dictionary<string, GameObject>();
                foreach (Transform obj in GameObject.Find("Canvas").transform)
                {
                    switch (obj.gameObject.name)
                    {
                        case "EggHatchWrapper":
                        case "RenameFarmWrapper":
                        case "IncubatorWrapper":
                        case "PkmnInfoWrapper":
                        case "StoreWrapper":
                        case "PCWrapper":
                        case "CareWrapper":
                        case "FeedWrapper":
                        case "WalkWrapper":
                        case "BreedWrapper":
                        case "PokemonContextWrapper":
                        case "BackpackWrapper":
                        case "KitchenWrapper":
                        case "CookingResultWrapper":
                        case "ItemUsePkmnWrapper":
                        case "WalkResultsWrapper":
                        case "TooManyWalksWrapper":
                        case "LaidWrapper":
                        case "LevelAnnounceWrapper":
                        case "EvoAnnounceWrapper":
                        case "TooltipWrapper":
                        case "SaveWrapper":
                        case "BerryGuideWrapper":
                        case "PkmnMovesWrapper":
                        case "StatsWrapper":
                            panels[obj.gameObject.name] = obj.gameObject;
                            break;
                    }
                }
            }
        }
    }
}
