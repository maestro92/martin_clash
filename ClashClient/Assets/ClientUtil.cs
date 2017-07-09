using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class ClientUtil
{

    static public T Instantiate<T>(string prefab)
    {
        GameObject gameObject = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(prefab));
        return gameObject.GetComponent<T>();
    }

    static public Sprite GetSprite(string imagePath)
    {
        return Resources.Load<Sprite>(imagePath);
    }
}

