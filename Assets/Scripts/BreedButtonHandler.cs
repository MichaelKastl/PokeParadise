using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public class BreedButtonHandler : MonoBehaviour
    {
        public Pokemon start;
        public void Breed()
        {
            Egg breeding = new Egg();
            GameObject button = GameObject.Find("BreedButton");
            Button btnComp = button.GetComponent<Button>();
            btnComp.interactable = false;
            int otherParentId = 0;
            Player player = PlayerData.FetchCurrentPlayerData();
            foreach (Pokemon p in player.pokemon)
            {
                if (GameObject.Find("BreedSprite_" + p.id) != null && !GameObject.Find("BreedSprite_" + p.id).GetComponent<Button>().interactable)
                {
                    otherParentId = p.id;
                    Egg e = new Egg(player, 0, DateTimeOffset.Now, start, p, false)
                    {
                        isIncubating = true
                    };
                    breeding = e;
                    player.eggs.Add(e);
                    p.isBreeding = true;
                }
                else if (p.id == start.id)
                {
                    p.isBreeding = true;
                }
            }
            foreach (Pokemon p in player.shownPokemon)
            {
                if (p != null)
                {
                    if (p.id == start.id || p.id == otherParentId)
                    {
                        p.isBreeding = true;
                    }
                }
            }
            PlayerData.SavePlayerData(player);
            PlayerData.LoadPokemonMenuSprites(player.shownPokemon);
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            panels["BreedWrapper"].transform.GetChild(0).gameObject.SetActive(true);
            foreach (Transform obj in panels["BreedWrapper"].transform.GetChild(0).transform)
            {
                if (obj.gameObject.name == "BreedInfoPanel")
                {
                    obj.gameObject.SetActive(true);
                }
            }
            btnComp.interactable = true;
            GameObject.Find("BreedInfoText").GetComponent<Text>().text = "Your Pokemon are breeding! They will be done in " + breeding.timeToBreed + ". Until then, they won't be accessible!";
        }
    }
}
