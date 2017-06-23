using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour 
{

    // singleton
    static public Main instance;

    public UIManager ui;



    /*
    possible approaches for managing bots

        bot living on Client vs bot living on Server
        bot living on Client
            you can utilize the Unity Editor to configure the AI.

        bot living on Server







    Lets start with just one ClientSessions connecting to the server
    
     */ 


    NetworkManager networkManager;

    public int TARGET_FRAME_RATE = 60;

    // List<ClientSession> clientSessions;
    public GameClient mainGameClient;

	// Use this for initialization
	void Start () 
    {

        Util.OnLog = (s) => 
            {
                Debug.Log(s);
            };

        Util.OnLogWarning = (s) => 
            {
                Debug.LogWarning(s);
            };
        
        Util.OnLogError = (s) => 
            {
                Debug.LogError(s);
            };



	    // connect to server
        // create a clientSessions
        networkManager = new NetworkManager();

        networkManager.GetServerIPAddress();

        ui = new UIManager();
        ui.Init();

        InitMainGameClient();


	}



    // my client is the same as clientSession
    // your main program can certainly have more than one client, right? :)
    void InitMainGameClient()
    {
        mainGameClient = new GameClient();
        mainGameClient.StartNetworkSession(networkManager.GetServerIPAddress());

    }



	// Update is called once per frame

	// Just like what we found out earlier, Update is called once per frame
	// so we want to put hardware input logic code here 

	// game2's setup was exactly like this, all the input and render related stuff goes in here
	// for example
	//		CityViewController 

    void Update() {

    }

	

	// all the clientSimulation related stuff
    // as well as Simulation Tick is placed in here
	void FixedUpdate()
	{

        // listen for socket?



        // send out your own messages

        mainGameClient.connection.Pump();
        /* 
        if not disconnected
        {
            try to reconnect;
        }
        else
        {
            


        }
        */ 

    }


}
