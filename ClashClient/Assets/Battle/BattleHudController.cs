using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

// this is more for the UI
public class BattleHudController : MonoBehaviour
{
    public const int NUM_CARDS_IN_HAND = 4;
    public GameObject screen;
    public GameObject handContainer;
    public GameObject cardBtnPrefab;

    public List<BattleHudCardBtnController> buttons;

    public void Init()
    {
        for (int i = 0; i < NUM_CARDS_IN_HAND; i++)
        {
            var btn = GameObject.Instantiate<GameObject>(cardBtnPrefab);
            btn.transform.SetParent(handContainer.transform);

            var bhcbc = btn.GetComponent<BattleHudCardBtnController>();

            bhcbc.Init(Enums.CardType.Summon_Footman);
            buttons.Add(bhcbc);


            btn.SetActive(true);
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
       
    // this assumes the rectTransform of battleHud is the same size of the phone
    public bool TouchInPlay(Vector3 touchPosition, Camera camera)
    {
   //     Debug.LogError("touchPosition " + touchPosition);
        var rect = screen.GetComponent<RectTransform>();
   //     Debug.LogError("screen.GetComponent<RectTransform>() " + rect.rect.x + " " + rect.rect.y + " " + rect.rect.width + " " + rect.rect.height);

        return RectTransformUtility.RectangleContainsScreenPoint(screen.GetComponent<RectTransform>(), touchPosition, camera) &&
            !RectTransformUtility.RectangleContainsScreenPoint(handContainer.GetComponent<RectTransform>(), touchPosition, camera);
    }


}

