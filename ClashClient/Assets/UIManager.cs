using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 


public class UIManager
{
    public MainMenuController mainMenu;
    public BattleScreenController battleScreen;
    public CardScreenController cardScreen;   
    public BattleHudController battleHud;

    public UIManager()
    {
        
    }

    public void Init()
    {
        Vector2 prefabPosition = Vector2.zero;

        battleScreen = InitController<BattleScreenController>("BattleScreen");
        battleScreen.Init();

        cardScreen = InitController<CardScreenController>("CardScreen");
        cardScreen.Init();

        battleHud = InitController<BattleHudController>("BattleHud");
        battleHud.Init();

        mainMenu = InitController<MainMenuController>("MainMenu");
        mainMenu.Init();

    }

    private T InitController<T>(string prefab)
    {
        GameObject gameObject = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(prefab));
        Vector2 prefabPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
        gameObject.transform.SetParent(Main.instance.uiCanvas.transform);
        gameObject.GetComponent<RectTransform>().anchoredPosition = prefabPosition;
        return gameObject.GetComponent<T>();
    }


    public void Reset()
    {
        mainMenu.gameObject.SetActive(false);
        battleScreen.gameObject.SetActive(false);
        cardScreen.gameObject.SetActive(false);
        battleHud.gameObject.SetActive(false);
    }

    /*
    GameObject go = GameUtil.Instantiate<GameObject>(Resources.Load<GameObject>(prefabName));

    Vector2 prefabPosition = go.GetComponent<RectTransform>().anchoredPosition;

    Util.Parent(go,canvas);

    go.GetComponent<RectTransform>().anchoredPosition = prefabPosition;
    */


}

