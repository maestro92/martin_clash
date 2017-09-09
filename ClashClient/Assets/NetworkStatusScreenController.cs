using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class NetworkStatusScreenController : MonoBehaviour
{
    

    public NetworkStatusScreenController()
    {
        
    }


    public void Init()
    {

    }

    public void Update()
    {
        if (Main.instance.mainGameClient != null)
        {
            if (Main.instance.mainGameClient.connection.GetConnectionState() == NetGameConnectionState.CONNECTED)
            {
                Main.instance.ui.battleScreen.Activate();
                Main.instance.ui.mainMenu.Activate();
            }
        }


        /*

            switch(main.gameConnection.current state)
            {

                display different shit

            }


            


         */ 


    }

    public void Activate()
    {
        this.gameObject.SetActive(true);
    }
}

