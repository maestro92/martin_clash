using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenuController : MonoBehaviour 
{
    public void Init()
    {

    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void OnTapCardScreenBtn()
    {
        Main.instance.ui.Reset();  
        Main.instance.ui.cardScreen.Activate();
        Main.instance.ui.mainMenu.Activate();
    }


    public void OnTapBattleScreenBtn()
    {
        Main.instance.ui.Reset(); 
        Main.instance.ui.battleScreen.Activate();
        Main.instance.ui.mainMenu.Activate();
    }
}

