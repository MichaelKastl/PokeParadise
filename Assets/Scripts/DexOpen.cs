using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PokeParadise
{
    public class DexOpen : MonoBehaviour, IPointerDownHandler
    {
        public Pokemon pokemon;
        // Start is called before the first frame update
        public void Open()
        {
            Pokemon p = pokemon;
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            PlayerData.ClosePanels();
            panels["PkmnInfoWrapper"].transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("DexNoText").GetComponent<Text>().text = "No. " + p.dexNo.ToString();
            GameObject.Find("DexNameText").GetComponent<Text>().text = p.pkmnName;
            GameObject.Find("DexParent1Text").GetComponent<Text>().text = "Parent: " + p.parent1.pkmnName;
            GameObject.Find("DexParent2Text").GetComponent<Text>().text = "Parent: " + p.parent2.pkmnName;
            GameObject.Find("DexIDText").GetComponent<Text>().text = "ID " + p.id.ToString();
            GameObject.Find("DexLevelText").GetComponent<Text>().text = "Lvl. " + p.level.ToString();
            GameObject.Find("DexGenderText").GetComponent<Text>().text = "Gender: " + p.gender;
            GameObject.Find("DexNatureText").GetComponent<Text>().text = "Nature: " + p.nature;
            GameObject.Find("DexAbilityText").GetComponent<Text>().text = "Ability: " + p.ability;
            GameObject.Find("DexFriendshipText").GetComponent<Text>().text = "Friendship: " + p.friendship;
            GameObject.Find("DexEggGroup1Text").GetComponent<Text>().text = "Egg Group 1: " + p.eggGroup1;
            GameObject.Find("DexEggGroup2Text").GetComponent<Text>().text = "Egg Group 2: " + p.eggGroup2;
            GameObject.Find("DexType1Text").GetComponent<Text>().text = "Type 1: " + p.type1;
            GameObject.Find("DexType2Text").GetComponent<Text>().text = "Type 2: " + p.type2;
            GameObject.Find("DexText").GetComponent<Text>().text = p.blurb;
            GameObject.Find("InfoSprite").GetComponent<SpriteSwap>().SpriteSheetName = p.dexNo.ToString() + "_menu";
            GameObject.Find("FavFlavorTxt").GetComponent<Text>().text = p.favoriteFlavor;
            GameObject.Find("HatedFlavorTxt").GetComponent<Text>().text = p.hatedFlavor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Pokemon p = pokemon;
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            PlayerData.ClosePanels();
            panels["PkmnInfoWrapper"].transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("DexNoText").GetComponent<Text>().text = "No. " + p.dexNo.ToString();
            GameObject.Find("DexNameText").GetComponent<Text>().text = p.pkmnName;
            GameObject.Find("DexParent1Text").GetComponent<Text>().text = "Parent: " + p.parent1.pkmnName;
            GameObject.Find("DexParent2Text").GetComponent<Text>().text = "Parent: " + p.parent2.pkmnName;
            GameObject.Find("DexIDText").GetComponent<Text>().text = "ID " + p.id.ToString();
            GameObject.Find("DexLevelText").GetComponent<Text>().text = "Lvl. " + p.level.ToString();
            GameObject.Find("DexGenderText").GetComponent<Text>().text = "Gender: " + p.gender;
            GameObject.Find("DexNatureText").GetComponent<Text>().text = "Nature: " + p.nature;
            GameObject.Find("DexAbilityText").GetComponent<Text>().text = "Ability: " + p.ability;
            GameObject.Find("DexFriendshipText").GetComponent<Text>().text = "Friendship: " + p.friendship;
            GameObject.Find("DexEggGroup1Text").GetComponent<Text>().text = "Egg Group 1: " + p.eggGroup1;
            GameObject.Find("DexEggGroup2Text").GetComponent<Text>().text = "Egg Group 2: " + p.eggGroup2;
            GameObject.Find("DexType1Text").GetComponent<Text>().text = "Type 1: " + p.type1;
            GameObject.Find("DexType2Text").GetComponent<Text>().text = "Type 2: " + p.type2;
            GameObject.Find("DexText").GetComponent<Text>().text = p.blurb;
            GameObject.Find("InfoSprite").GetComponent<SpriteSwap>().SpriteSheetName = p.dexNo.ToString() + "_menu";
            GameObject.Find("FavFlavorTxt").GetComponent<Text>().text = p.favoriteFlavor.ToLower();
            GameObject.Find("HatedFlavorTxt").GetComponent<Text>().text = p.hatedFlavor.ToLower();
        }
    }
}