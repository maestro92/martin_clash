using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI;

public class BattleHudCardBtnController : MonoBehaviour
{
    public CardConfig cardConfig;
    public GameObject highlight;
    public bool isHighlighted;

    public static BattleHudCardBtnController lastSelectedBtn;

    public void Init(Enums.CardType cardType)
    {
        cardConfig = Config.cardConfigs[cardType];
        isHighlighted = false;
    }

    public void Update()
    {
        if (this == lastSelectedBtn)
        {
            highlight.SetActive(true);
        }
        else
        {
            highlight.SetActive(false);
        }
    }

    public void OnTap()
    {
        Debug.LogError("OnTap");
        isHighlighted = false;
        lastSelectedBtn = this;
    }

}

