using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokeParadise
{
    public class TestingSprites : MonoBehaviour
    {
        // Start is called before the first frame update
        public void Start()
        {
            float x = -10.04f; //-9.54
            float y = 4.6f; //4.2
            int cnt = 1;
            Sprite starterSprite = Resources.Load<Sprite>("1_menu");
            foreach (Pokemon p in PlayerData.FetchPokedex())
            {
                if (p.dexNo != 0)
                {
                    GameObject obj = new GameObject();
                    int index = cnt;
                    obj.AddComponent<SpriteRenderer>();
                    obj.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                    obj.GetComponent<SpriteRenderer>().sprite = starterSprite;
                    obj.transform.localScale = new Vector3(2, 2);
                    obj.AddComponent<Animator>();
                    obj.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Animations/AnimatorController") as RuntimeAnimatorController;
                    obj.AddComponent<Wanderer>();
                    obj.AddComponent<Rigidbody2D>();
                    obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    obj.GetComponent<Rigidbody2D>().gravityScale = 0;
                    obj.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                    obj.GetComponent<Rigidbody2D>().freezeRotation = true;
                    obj.AddComponent<BoxCollider2D>();
                    obj.GetComponent<BoxCollider2D>().size = new Vector3(0.3f, 0.32f);
                    obj.AddComponent<SpriteSwap>();
                    if (p.dexNo < 10)
                    {
                        obj.GetComponent<SpriteSwap>().SpriteSheetName = "00" + p.dexNo;
                    }
                    else if (p.dexNo < 100)
                    {
                        obj.GetComponent<SpriteSwap>().SpriteSheetName = "0" + p.dexNo;
                    }
                    else
                    {
                        obj.GetComponent<SpriteSwap>().SpriteSheetName = p.dexNo.ToString();
                    }
                    obj.transform.localPosition = new Vector2(x, y);
                    obj.name = p.pkmnName;
                    if (cnt % 40 == 0)
                    {
                        x = -10.04f;
                        y -= .4f;
                    }
                    else
                    {
                        x += .5f;
                    }
                    cnt++;
                }
            }
        }
    }
}
