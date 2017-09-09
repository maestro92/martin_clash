using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class BattleScreenController : MonoBehaviour 
{
    public void Init()
    {


    }




    // sends a message to the server
    public void SearchForBattle()
    {
        Main.instance.SearchMatchVsAI();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }
}

