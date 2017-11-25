using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class NetworkStatusScreenController : MonoBehaviour
{
    
    public GameObject spinningCircle;
    public float angle;
    public NetworkStatusScreenController()
    {
        
    }


    public void Init()
    {
        angle = 0;
    }

    public void Update()
    {



        bool shouldBeOn = false;

        if (Main.instance.mainGameClient != null)
        {
            NetGameConnectionState state = Main.instance.mainGameClient.connection.GetConnectionState();
            switch (state)
            {
                case NetGameConnectionState.ClientContactingServer:
                case NetGameConnectionState.ClientDisconnected:
                case NetGameConnectionState.None:
                    shouldBeOn = true;
                    break;
            }                    
        }


        if (shouldBeOn == true)
        {
            if (Main.instance.ui.networkStatusScreen.isActiveAndEnabled == false)
            {
                Main.instance.ui.networkStatusScreen.Activate();
            }
//            Main.instance.ui.battleScreen.Activate();
//            Main.instance.ui.mainMenu.Activate();
        }
        else
        {
            if (Main.instance.ui.networkStatusScreen.isActiveAndEnabled == true)
            {
                Main.instance.ui.networkStatusScreen.gameObject.SetActive(false);
            }
        }





        spinningCircle.transform.Rotate(new Vector3(0, 0, 1));

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

