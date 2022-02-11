using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PokeParadise
{
    public class SwapShownPokemon : MonoBehaviour, IPointerDownHandler
    {
        public int swapIndex;
        public Pokemon toSwap;
        public void OnPointerDown(PointerEventData eventData)
        {
            Player player = PlayerData.FetchCurrentPlayerData();
            bool alreadyShown = false;
            foreach (Pokemon p in player.shownPokemon)
            {
                if (p != null && p.id == toSwap.id && p.dexNo == toSwap.dexNo)
                {
                    alreadyShown = true;
                }
            }
            if (alreadyShown)
            {
                foreach (GameObject obj in PlayerData.GetAllObjectsOnlyInScene())
                {
                    if (obj.name == "AlreadyShownPanel")
                    {
                        obj.SetActive(true);
                    }
                }
            }
            else
            {
                player.shownPokemon[swapIndex] = toSwap;
                PlayerData.SavePlayerData(player);
                Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
                panels["PCWrapper"].transform.GetChild(0).gameObject.SetActive(false);
                foreach (GameObject obj in PlayerData.GetAllObjectsOnlyInScene())
                {
                    if (obj.name == "SwapPanel")
                    {
                        obj.SetActive(false);
                    }
                }
                PlayerData.LoadPokemonMenuSprites(player.shownPokemon);
            }
        }
    }
}
