using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class BattleScreenController : MonoBehaviour 
{
    public BattleScreenController()
    {


    }


    // sends a message to the server
    public void SearchForBattle()
    {
        Main.instance.mainGameClient.SearchMatch();
        Main.instance.mainGameClient.SearchMatch();
    }


}

