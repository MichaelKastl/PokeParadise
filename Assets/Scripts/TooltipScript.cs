using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public class TooltipScript : MonoBehaviour
    {
        public Vector3 offset;

        public void Update()
        {
            Vector3 camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(camPos.x + offset.x, camPos.y + offset.y, 0);
        }
    }
}
