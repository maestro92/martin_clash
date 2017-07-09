using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

// this is more for the UI
public class BattleHudController : MonoBehaviour
{
    public const int NUM_CARDS_IN_HAND = 4;
    public GameObject handContainer;
    public GameObject cardBtnPrefab;


    public void Init()
    {
        for (int i = 0; i < NUM_CARDS_IN_HAND; i++)
        {
            var btn = GameObject.Instantiate<GameObject>(cardBtnPrefab);
            btn.transform.SetParent(handContainer.transform);
        }

        cardBtnPrefab.SetActive(false);
    }


    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Update()
    {



    }
}

