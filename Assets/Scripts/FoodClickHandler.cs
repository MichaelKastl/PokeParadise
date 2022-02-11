using PokeParadise.Classes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PokeParadise
{
    public class FoodClickHandler : MonoBehaviour, IPointerDownHandler
    {
        public Item item;
        public int toFeed;
        public void OnPointerDown(PointerEventData eventData)
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            foreach (Transform obj in panels["FeedWrapper"].transform.GetChild(0).transform)
            {
                if (obj.gameObject.name == "FoodContextPanel")
                {
                    obj.gameObject.SetActive(true);
                    break;
                }
            }
            GameObject.Find("FeedLabel").GetComponent<Text>().text = "Name: " + item.food.type + " " + item.food.category;
            int maxFlavor = item.food.flavorProfile.Max();
            int secondFlavor = item.food.flavorProfile.OrderByDescending(r => r).Skip(1).FirstOrDefault();
            string flavorString = "";
            string secondFlavorString = "";
            if (maxFlavor == item.food.flavorProfile[0])
            {
                flavorString = "Spicy";
            }
            else if (maxFlavor == item.food.flavorProfile[1])
            {
                flavorString = "Sour";
            }
            else if (maxFlavor == item.food.flavorProfile[2])
            {
                flavorString = "Bitter";
            }
            else if (maxFlavor == item.food.flavorProfile[3])
            {
                flavorString = "Sweet";
            }
            else if (maxFlavor == item.food.flavorProfile[4])
            {
                flavorString = "Dry";
            }
            if (secondFlavor > 0)
            {
                if (secondFlavor == item.food.flavorProfile[0] && flavorString != "Spicy")
                {
                    secondFlavorString = "Spicy";
                }
                else if (secondFlavor == item.food.flavorProfile[1] && flavorString != "Sour")
                {
                    secondFlavorString = "Sour";
                }
                else if (secondFlavor == item.food.flavorProfile[2] && flavorString != "Bitter")
                {
                    secondFlavorString = "Bitter";
                }
                else if (secondFlavor == item.food.flavorProfile[3] && flavorString != "Sweet")
                {
                    secondFlavorString = "Sweet";
                }
                else if (secondFlavor == item.food.flavorProfile[4] && flavorString != "Dry")
                {
                    secondFlavorString = "Dry";
                }
            }
            if (secondFlavorString != "")
            {
                flavorString += ", " + secondFlavorString;
            }
            GameObject.Find("FeedFlavorLabel").GetComponent<Text>().text = "Flavor(s): " + flavorString;
            FoodFeedHandler handler = GameObject.Find("FeedHandler").GetComponent<FoodFeedHandler>();
            handler.item = item;
            handler.id = toFeed;
        }
    }
}
