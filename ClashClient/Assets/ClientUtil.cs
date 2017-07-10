using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class ClientUtil
{

    static public GameObject Instantiate(string prefab)
    {
        return GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(prefab));
    }

    static public Sprite GetSprite(string imagePath)
    {
        return Resources.Load<Sprite>(imagePath);
    }
}

