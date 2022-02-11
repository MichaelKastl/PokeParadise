using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public static class DexButtonOpen
    {
        // Start is called before the first frame update
        public static void Open(int id)
        {
            string[] typesArray = new string[] { "bug", "dark", "dragon", "electric", "fairy", "fighting", "fire", "flying", "ghost", "grass", "ground", "ice", "normal", "poison", "psychic", "rock", "steel", "water" };
            Pokemon p = PlayerData.FetchCurrentPlayerData().pokemon.Find(x => x.id == id);
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            PlayerData.ClosePanels();
            Transform panel = panels["PkmnInfoWrapper"].transform.GetChild(0);
            panel.gameObject.SetActive(true);
            GameObject dexHolder = new GameObject();
            foreach (Transform obj in panel.transform)
            {
                if (obj.gameObject.name == "DexHolder")
                {
                    UnityEngine.Object.Destroy(dexHolder);
                    dexHolder = obj.gameObject;
                }
            }
            DexHolder dexHolderComp = dexHolder.GetComponent<DexHolder>();
            if (dexHolderComp.objects == null || dexHolderComp.objects.Count == 0)
            {
                dexHolderComp.objects = new Dictionary<string, GameObject>();
                foreach (Transform obj in panel.transform)
                {
                    switch (obj.gameObject.name)
                    {
                        case "DexNoBckgrnd":
                        case "DexNameBckgrnd":
                        case "DexParent1":
                        case "DexParent2":
                        case "DexID":
                        case "DexLevel":
                        case "DexGender":
                        case "DexNature":
                        case "DexAbility":
                        case "DexFriendship":
                        case "DexEggGroup1":
                        case "DexEggGroup2":
                        case "DexType1":
                        case "DexType2":
                        case "DexBckground":
                            dexHolderComp.objects[obj.transform.GetChild(0).gameObject.name] = obj.transform.GetChild(0).gameObject;
                            break;
                        case "InfoSprite":
                        case "FavFlavorTxt":
                        case "HatedFlavorTxt":
                        case "DexTypeIcon1":
                        case "DexTypeIcon2":
                            dexHolderComp.objects[obj.gameObject.name] = obj.gameObject;
                            break;
                    }
                }
            }
            dexHolderComp.objects["DexNoText"].GetComponent<Text>().text = "No. " + p.dexNo.ToString();
            dexHolderComp.objects["DexNameText"].GetComponent<Text>().text = p.pkmnName;
            dexHolderComp.objects["DexParent1Text"].GetComponent<Text>().text = "Parent: " + p.parent1.pkmnName;
            dexHolderComp.objects["DexParent2Text"].GetComponent<Text>().text = "Parent: " + p.parent2.pkmnName;
            dexHolderComp.objects["DexIDText"].GetComponent<Text>().text = "ID " + p.id.ToString();
            dexHolderComp.objects["DexLevelText"].GetComponent<Text>().text = "Lvl. " + p.level.ToString();
            dexHolderComp.objects["DexGenderText"].GetComponent<Text>().text = "Gender: " + p.gender;
            dexHolderComp.objects["DexNatureText"].GetComponent<Text>().text = "Nature: " + p.nature;
            dexHolderComp.objects["DexAbilityText"].GetComponent<Text>().text = "Ability: " + p.ability;
            dexHolderComp.objects["DexFriendshipText"].GetComponent<Text>().text = "Friendship: " + p.friendship;
            dexHolderComp.objects["DexEggGroup1Text"].GetComponent<Text>().text = "Egg Group 1: " + p.eggGroup1;
            dexHolderComp.objects["DexEggGroup2Text"].GetComponent<Text>().text = "Egg Group 2: " + p.eggGroup2;
            dexHolderComp.objects["DexType1Text"].GetComponent<Text>().text = "Type 1: " + p.type1;
            dexHolderComp.objects["DexType2Text"].GetComponent<Text>().text = "Type 2: " + p.type2;
            dexHolderComp.objects["DexText"].GetComponent<Text>().text = p.blurb;
            dexHolderComp.objects["InfoSprite"].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(p.dexNo.ToString() + "_menu");
            dexHolderComp.objects["FavFlavorTxt"].GetComponent<Text>().text = p.favoriteFlavor;
            dexHolderComp.objects["HatedFlavorTxt"].GetComponent<Text>().text = p.hatedFlavor;
            dexHolderComp.objects["DexTypeIcon1"].GetComponent<Image>().sprite = (Sprite)Resources.LoadAll("typeIcons")[Array.IndexOf(typesArray, p.type1.ToLower()) + 1];
            if (!string.IsNullOrEmpty(p.type2))
            {
                dexHolderComp.objects["DexTypeIcon2"].SetActive(true);
                dexHolderComp.objects["DexTypeIcon2"].GetComponent<Image>().sprite = (Sprite)Resources.LoadAll("typeIcons")[Array.IndexOf(typesArray, p.type2.ToLower()) + 1];
            }
            else
            {
                dexHolderComp.objects["DexTypeIcon2"].SetActive(false);
            }
        }
    }
}