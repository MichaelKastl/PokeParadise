using PokeParadise.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public class OpenKitchenHandler : MonoBehaviour
    {
        public void Open()
        {
            ClearFood();
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            PlayerData.ClosePanels();
            panels["KitchenWrapper"].transform.GetChild(0).gameObject.SetActive(true);
            GameObject panel = GameObject.Find("KitchenItemsPanel");
            Player player = PlayerData.FetchCurrentPlayerData();
            int berryCount = player.inventory.FindAll(x => x.berry != null && x.berry.name != "").Count;
            if (berryCount >= 8)
            {
                panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)(35 * Math.Ceiling(berryCount / 9d)) + 15);
            }
            Vector2 centerAnchor = new Vector2(0.5f, 0.5f);
            float x = 20f;
            float y = -20f;
            Vector3 initialPosition = new Vector3(x, y);
            int cnt = 1;
            foreach (Item i in player.inventory)
            {
                if (i.berry != null && i.berry.name != "")
                {
                    if (GameObject.Find("BerrySprite" + i.id) != null)
                    {
                        Destroy(GameObject.Find("BerrySprite" + i.id));
                    }
                    GameObject item = new GameObject
                    {
                        name = "BerrySprite" + i.id
                    };
                    item.AddComponent<Image>().sprite = Resources.Load<Sprite>("Food/" + i.berry.name.ToLower());
                    item.transform.SetParent(panel.transform, false);
                    item.transform.localScale = new Vector3(0.3f, 0.3f);
                    RectTransform itemRect = item.GetComponent<RectTransform>();
                    itemRect.anchorMin = centerAnchor;
                    itemRect.anchorMax = centerAnchor;
                    item.transform.localPosition = new Vector3(x, y);
                    item.AddComponent<BoxCollider2D>().size = item.GetComponent<RectTransform>().sizeDelta;
                    item.AddComponent<KitchenBerryIsSelected>().isSelected = false;
                    item.AddComponent<Button>().onClick.AddListener(delegate { ToggleFood(item, player.inventory.FindAll(x => x.berry != null && x.berry.name != "")); });
                    if (cnt == 1)
                    {
                        item.transform.localPosition = initialPosition;
                        x += 35;
                    }
                    else if (cnt % 9 == 0)
                    {
                        item.transform.localPosition = new Vector3(x, y);
                        x = 20f;
                        y -= 35;
                    }
                    else
                    {
                        item.transform.localPosition = new Vector3(x, y);
                        x += 35;
                    }
                    cnt++;
                }
            }
        }

        public static void ToggleFood(GameObject obj, List<Item> inv)
        {
            if (obj.GetComponent<KitchenBerryIsSelected>().isSelected)
            {
                obj.GetComponent<KitchenBerryIsSelected>().isSelected = false;
                obj.GetComponent<Image>().color = new Color(191, 191, 191, 1);
                int spicyCount = 0;
                int bitterCount = 0;
                int sweetCount = 0;
                int sourCount = 0;
                int dryCount = 0;
                int penalty = 0;
                string berryNames = "None";
                foreach (Item i in inv)
                {
                    GameObject icon = GameObject.Find("BerrySprite" + i.id);
                    if (icon.GetComponent<KitchenBerryIsSelected>().isSelected)
                    {
                        spicyCount += i.berry.spicyValue;
                        bitterCount += i.berry.bitterValue;
                        sweetCount += i.berry.sweetValue;
                        sourCount += i.berry.sourValue;
                        dryCount += i.berry.dryValue;
                        if (berryNames == "None")
                        {
                            berryNames = char.ToUpper(i.berry.name[0]) + i.berry.name.Substring(1) + " | ";
                        }
                        else
                        {
                            berryNames += char.ToUpper(i.berry.name[0]) + i.berry.name.Substring(1) + " | ";
                        }
                    }
                }
                int spicyTotal = spicyCount;
                int bitterTotal = bitterCount;
                int sweetTotal = sweetCount;
                int sourTotal = sourCount;
                int dryTotal = dryCount;
                spicyTotal -= dryCount;
                if (spicyTotal < 0) { penalty++; }
                sourTotal -= spicyCount;
                if (sourTotal < 0) { penalty++; }
                bitterTotal -= sourCount;
                if (bitterTotal < 0) { penalty++; }
                sweetTotal -= bitterCount;
                if (sweetTotal < 0) { penalty++; }
                dryTotal -= sweetCount;
                if (dryTotal < 0) { penalty++; }
                spicyTotal -= penalty;
                if (spicyTotal < 0) { spicyTotal = 0; }
                sourTotal -= penalty;
                if (sourTotal < 0) { sourTotal = 0; }
                bitterTotal -= penalty;
                if (bitterTotal < 0) { bitterTotal = 0; }
                sweetTotal -= penalty;
                if (sweetTotal < 0) { sweetTotal = 0; }
                dryTotal -= penalty;
                if (dryTotal < 0) { dryTotal = 0; }
                if (berryNames.EndsWith(" | "))
                {
                    berryNames.TrimEnd(new char[] { ' ', '|' });
                }
                GameObject.Find("CookPredictor").GetComponent<Text>().text = @$"Berries Used: {berryNames}

Current Flavor Totals
Spicy: {spicyTotal} | Bitter: {bitterTotal} | Sweet: {sweetTotal} | Sour: {sourTotal} | Dry: {dryTotal}";
            }
            else
            {
                int interactableCount = 0;
                int spicyCount = 0;
                int bitterCount = 0;
                int sweetCount = 0;
                int sourCount = 0;
                int dryCount = 0;
                int penalty = 0;
                string berryNames = "None";
                foreach (Item i in inv)
                {
                    GameObject icon = GameObject.Find("BerrySprite" + i.id);
                    if (icon.GetComponent<KitchenBerryIsSelected>().isSelected)
                    {
                        interactableCount++;
                    }
                }
                if (interactableCount < 4)
                {
                    obj.GetComponent<KitchenBerryIsSelected>().isSelected = true;
                    obj.GetComponent<Image>().color = new Color(191, 191, 191, 0.5f);
                    foreach (Item i in inv)
                    {
                        GameObject icon = GameObject.Find("BerrySprite" + i.id);
                        if (icon.GetComponent<KitchenBerryIsSelected>().isSelected)
                        {
                            interactableCount++;
                            spicyCount += i.berry.spicyValue;
                            bitterCount += i.berry.bitterValue;
                            sweetCount += i.berry.sweetValue;
                            sourCount += i.berry.sourValue;
                            dryCount += i.berry.dryValue;
                            if (berryNames == "None")
                            {
                                berryNames = char.ToUpper(i.berry.name[0]) + i.berry.name.Substring(1) + " | ";
                            }
                            else
                            {
                                berryNames += char.ToUpper(i.berry.name[0]) + i.berry.name.Substring(1) + " | ";
                            }
                        }
                    }
                    int spicyTotal = spicyCount;
                    int bitterTotal = bitterCount;
                    int sweetTotal = sweetCount;
                    int sourTotal = sourCount;
                    int dryTotal = dryCount;
                    spicyTotal -= dryCount;
                    if (spicyTotal < 0) { penalty++; }
                    sourTotal -= spicyCount;
                    if (sourTotal < 0) { penalty++; }
                    bitterTotal -= sourCount;
                    if (bitterTotal < 0) { penalty++; }
                    sweetTotal -= bitterCount;
                    if (sweetTotal < 0) { penalty++; }
                    dryTotal -= sweetCount;
                    if (dryTotal < 0) { penalty++; }
                    spicyTotal -= penalty;
                    if (spicyTotal < 0) { spicyTotal = 0; }
                    sourTotal -= penalty;
                    if (sourTotal < 0) { sourTotal = 0; }
                    bitterTotal -= penalty;
                    if (bitterTotal < 0) { bitterTotal = 0; }
                    sweetTotal -= penalty;
                    if (sweetTotal < 0) { sweetTotal = 0; }
                    dryTotal -= penalty;
                    if (dryTotal < 0) { dryTotal = 0; }
                    if (berryNames.EndsWith(" | "))
                    {
                        berryNames.TrimEnd(new char[] { ' ', '|' });
                    }
                    GameObject.Find("CookPredictor").GetComponent<Text>().text = @$"Berries Used: {berryNames}

Current Flavor Totals
Spicy: {spicyTotal} | Bitter: {bitterTotal} | Sweet: {sweetTotal} | Sour: {sourTotal} | Dry: {dryTotal}";
                }
                else
                {
                    foreach (GameObject obj2 in PlayerData.GetAllObjectsOnlyInScene())
                    {
                        if (obj2.name == "InvalidSelectionPanel")
                        {
                            obj2.SetActive(true);
                        }
                    }
                }
            }
        }

        public static void ClearFood()
        {
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x.name.Contains("BerrySprite")))
            {
                Destroy(obj);
            }
        }
    }
}
