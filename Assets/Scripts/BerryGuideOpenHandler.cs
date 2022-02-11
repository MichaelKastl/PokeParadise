using System.Collections.Generic;
using UnityEngine;
using PokeParadise.Classes;
using UnityEngine.UI;
using System.Linq;

namespace PokeParadise
{
    public class BerryGuideOpenHandler : MonoBehaviour
    {
        public void Open()
        {
            PlayerData.ClosePanels();
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            panels["BerryGuideWrapper"].transform.GetChild(0).gameObject.SetActive(true);
            GameObject panel = panels["BerryGuideWrapper"].transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
            Berry[] berries = PlayerData.FetchBerries();
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2(350.67f, ((berries.Count() - 1) * 38) + 10);
            const float x = 33.135f;
            float y = -15f;
            Vector2 initialPos = new Vector2(x, y);
            Vector2 center = new Vector2(.5f, .5f);
            int cnt = 1;
            foreach (Transform obj in panel.transform)
            {
                Destroy(obj.gameObject);
            }
            foreach (Berry berry in PlayerData.FetchBerries())
            {
                if (berry.name != null)
                {
                    GameObject b = new GameObject(berry.name + "BerrySprite");
                    b.transform.SetParent(panel.transform);
                    b.AddComponent<Image>().sprite = Resources.Load<Sprite>("Food/" + berry.name.ToLower());
                    RectTransform bRect = b.GetComponent<RectTransform>();
                    bRect.anchorMin = center;
                    bRect.anchorMax = center;
                    b.transform.localScale = new Vector2(0.3f, 0.3f);
                    if (cnt == 1)
                    {
                        b.transform.localPosition = initialPos;
                    }
                    else
                    {
                        y -= 38;
                        b.transform.localPosition = new Vector2(x, y);
                    }
                    GameObject nameText = new GameObject(berry.name + "BerryName");
                    Text nameTextComponent = nameText.AddComponent<Text>();
                    nameTextComponent.text = char.ToUpper(berry.name[0]) + berry.name.Substring(1);
                    nameTextComponent.fontSize = 30;
                    nameTextComponent.font = Resources.Load<Font>("Fonts/PKMN RBYGSC");
                    nameTextComponent.color = Color.black;
                    nameTextComponent.alignment = TextAnchor.MiddleCenter;
                    nameText.transform.SetParent(b.transform);
                    RectTransform nameRect = nameText.GetComponent<RectTransform>();
                    nameRect.anchorMin = center;
                    nameRect.anchorMax = center;
                    nameText.transform.localPosition = new Vector2(183.85f, 0f);
                    nameText.transform.localScale = new Vector2(1f, 1f);
                    nameRect.sizeDelta = new Vector2(200, 100);
                    int[] flavors = new int[] { berry.spicyValue, berry.sourValue, berry.bitterValue, berry.sweetValue, berry.dryValue };
                    int maxFlavor = flavors.Max();
                    string strongest = "";
                    int second = flavors.OrderByDescending(r => r).Skip(1).FirstOrDefault();
                    string secondStrongest = "";
                    if (flavors[0] == maxFlavor)
                    {
                        strongest = "Spicy";
                    }
                    else if (flavors[1] == maxFlavor)
                    {
                        strongest = "Sour";
                    }
                    else if (flavors[2] == maxFlavor)
                    {
                        strongest = "Bitter";
                    }
                    else if (flavors[3] == maxFlavor)
                    {
                        strongest = "Sweet";
                    }
                    else if (flavors[4] == maxFlavor)
                    {
                        strongest = "Dry";
                    }
                    if (second > 0)
                    {
                        if (flavors[0] == second)
                        {
                            secondStrongest = "Spicy";
                        }
                        else if (flavors[1] == second)
                        {
                            secondStrongest = "Sour";
                        }
                        else if (flavors[2] == second)
                        {
                            secondStrongest = "Bitter";
                        }
                        else if (flavors[3] == second)
                        {
                            secondStrongest = "Sweet";
                        }
                        else if (flavors[4] == second)
                        {
                            secondStrongest = "Dry";
                        }
                    }
                    GameObject strongestFlavorText = new GameObject(berry.name + "BerryStrongest");
                    Text flavorTxt = strongestFlavorText.AddComponent<Text>();
                    flavorTxt.text = strongest + " (" + maxFlavor + ")";
                    flavorTxt.fontSize = 30;
                    flavorTxt.font = Resources.Load<Font>("Fonts/PKMN RBYGSC");
                    flavorTxt.color = Color.black;
                    flavorTxt.alignment = TextAnchor.MiddleCenter;
                    strongestFlavorText.transform.SetParent(b.transform);
                    RectTransform flavorRect = strongestFlavorText.GetComponent<RectTransform>();
                    flavorRect.anchorMin = center;
                    flavorRect.anchorMax = center;
                    strongestFlavorText.transform.localPosition = new Vector2(481.12f, 0f);
                    strongestFlavorText.transform.localScale = new Vector2(1f, 1f);
                    flavorRect.sizeDelta = new Vector2(200, 100);
                    GameObject secondStrongestFlavorText = new GameObject(berry.name + "BerrySecond");
                    Text secondTxt = secondStrongestFlavorText.AddComponent<Text>();
                    if (second > 0)
                    {
                        secondTxt.text = secondStrongest + " (" + second + ")";
                    }
                    else
                    {
                        secondTxt.text = "None (0)";
                    }
                    secondTxt.fontSize = 30;
                    secondTxt.font = Resources.Load<Font>("Fonts/PKMN RBYGSC");
                    secondTxt.color = Color.black;
                    secondTxt.alignment = TextAnchor.MiddleCenter;
                    secondStrongestFlavorText.transform.SetParent(b.transform);
                    RectTransform secondRect = secondStrongestFlavorText.GetComponent<RectTransform>();
                    secondRect.anchorMin = center;
                    secondRect.anchorMax = center;
                    secondStrongestFlavorText.transform.localPosition = new Vector2(852f, 0f);
                    secondStrongestFlavorText.transform.localScale = new Vector2(1f, 1f);
                    secondRect.sizeDelta = new Vector2(200, 100);
                    GameObject line = new GameObject(berry.name + "BerryLine");
                    RectTransform lineRect = line.AddComponent<RectTransform>();
                    lineRect.anchorMin = center;
                    lineRect.anchorMax = center;
                    line.transform.SetParent(b.transform);
                    line.AddComponent<Image>().color = new Color(0, 0, 0, 255);
                    lineRect.sizeDelta = new Vector2(1095, 2.5f);
                    line.transform.localPosition = new Vector2(474, -63);
                    line.transform.localScale = new Vector2(1f, 1f);
                    cnt++;
                }
            }
        }
    }
}