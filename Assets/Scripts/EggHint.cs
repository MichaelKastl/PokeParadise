using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public class EggHint : MonoBehaviour
    {
        public Egg egg;
        public void OnMouseDown()
        {
            Egg e = egg;
            string eggStr;
            if (!e.isIncubating)
            {
                TimeSpan since = DateTimeOffset.Now - DateTimeOffset.Parse(e.obtained);
                double percent = (double)since.Ticks / (double)TimeSpan.Parse(e.timeToHatch).Ticks;
                if (percent >= .75)
                {
                    eggStr = "Sounds can be heard coming from inside! This Egg will hatch soon!";
                }
                else if (percent >= .5)
                {
                    eggStr = "It appears to move occasionally. It may be close to hatching.";
                }
                else if (percent >= .25)
                {
                    eggStr = "What Pokémon will hatch from this Egg? It doesn't seem close to hatching.";
                }
                else
                {
                    eggStr = "It looks as though this Egg will take a long time yet to hatch.";
                }
            }
            else
            {
                eggStr = "Your egg is still being laid! Come back in a bit, it'll be ready to incubate soon!";
            }
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            foreach (Transform obj in panels["IncubatorWrapper"].transform.GetChild(0).transform)
            {
                if (obj.gameObject.name == "EggHintPanel")
                {
                    obj.gameObject.SetActive(true);
                }
                else if (obj.gameObject.name == "EggHintText")
                {
                    obj.gameObject.GetComponent<Text>().text = eggStr;
                }
            }
        }
    }
}
