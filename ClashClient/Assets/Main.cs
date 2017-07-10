using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour 
{

    // singleton
    static public Main instance;

    public UIManager ui;
    public GameObject uiCanvas;
  //  public BattleHudController hud;


    public BattleViewController bvc;

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
        instance = this;

        Config.Init();

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

//        ActivateMainScreen();

        // we start with the battle screen
        ui.Reset();
        ui.battleScreen.Activate();
        ui.mainMenu.Activate();

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
        if (mainGameClient != null)
        {
            mainGameClient.Pump();
        }
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

    void ActivateMainScreen()
    {
        ui.mainMenu.Activate();
    }


    public void GoToBattle()
    {
        ui.Reset();

        ui.battleHud.Activate();


        mainGameClient.StartBattle(null);

        bvc = ClientUtil.Instantiate("BattleView").GetComponent<BattleViewController>();

        bvc.Init(mainGameClient.GetClientSimulation());

        bvc.gameObject.SetActive(true);
        bvc.transform.SetAsLastSibling();

    //    bvc.clientSim = mainGameClient.
    }   

}
