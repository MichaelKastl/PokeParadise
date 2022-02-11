using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaidOKHandler : MonoBehaviour
{
    public void Close()
    {
        GameObject.Find("LaidPanel").SetActive(false);
    }
}
