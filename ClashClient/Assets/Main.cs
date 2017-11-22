using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour 
{

    // singleton
    static public Main instance;

    public UIManager ui;
    public GameObject uiCanvas;
    public Camera uiCamera;
  //  public BattleHudController hud;

    public ClientDebugPanel clientDebugPanel;

    public BattleViewController bvc;

    /*
    possible approaches for managing bots

        bot living on Client vs bot living on Server
        bot living on Client
            you can utilize the Unity Editor to configure the AI.

        bot living on Server







    Lets start with just one ClientSessions connecting to the server
    
     */ 


    public NetworkManager networkManager;
//    public static readonly NetMeter netMeterManager = new NetMeter();

    public int TARGET_FRAME_RATE = 60;

    public GameClient mainGameClient;
    public List<GameClient> gameClients;

	// Use this for initialization
	void Start () 
    {
        instance = this;

  //      netMeterManager.Init();

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


        gameClients = new List<GameClient>();

	    // connect to server
        // create a clientSessions
        networkManager = new NetworkManager();

        networkManager.GetServerIPAddress();



        NetGlobal.netMeter.Init();
        NetGlobal.netMeter.Enable(true);

        Time.fixedDeltaTime = Globals.FIXED_UPDATE_TIME_s;

        ui = new UIManager();
        ui.Init();

//        ActivateMainScreen();

        // we start with the battle screen
        ui.Reset();
      
     //   ui.networkStatusScreen.Activate();


        InitMainGameClient();
        clientDebugPanel = new ClientDebugPanel();
        clientDebugPanel.SetLocalGameClient(mainGameClient);

    }



    // my client is the same as clientSession
    // your main program can certainly have more than one client, right? :)
    void InitMainGameClient()
    {
        mainGameClient = CreateNewGameClient();

        mainGameClient.OnStartBattle = () =>
        {
            GoToBattle();
        };

        mainGameClient.StartNetworkSession(networkManager.GetServerIPAddress(), OnLoginAsMainGameClient);

    }


    public void OnLoginAsMainGameClient()
    {
        Debug.LogError("OnLoginAsMainGameClient()");
        ui.battleScreen.Activate();
        ui.mainMenu.Activate();
    }

	// Update is called once per frame

	// Just like what we found out earlier, Update is called once per frame
	// so we want to put hardware input logic code here 

	// game2's setup was exactly like this, all the input and render related stuff goes in here
	// for example
	//		CityViewController 

    void Update() {
        ui.Update();
    }

	

    void OnGUI()
    {
        if (clientDebugPanel != null)
        {
            clientDebugPanel.Render();
        }

    }

	// all the clientSimulation related stuff
    // as well as Simulation Tick is placed in here
	void FixedUpdate()
	{

        // listen for socket?


        /*
        // send out your own messages
        if (mainGameClient != null)
        {
            mainGameClient.Pump();
        }
        */

        foreach (var gc in gameClients)
        {
            gc.Pump();
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

        bvc = ClientUtil.Instantiate("BattleView").GetComponent<BattleViewController>();

        bvc.Init(mainGameClient.GetClientSimulation());

        bvc.gameObject.SetActive(true);
        bvc.transform.SetAsLastSibling();

    //    bvc.clientSim = mainGameClient.
    }   


    public GameClient CreateNewGameClient()
    {
        GameClient newClient = new GameClient();

        gameClients.Add(newClient);

        return newClient;

    }

    public void SearchMatchVsAI()
    {
        
        // start up an AI
        GameClient aiClient = CreateNewGameClient();
        aiClient.StartNetworkSession(Main.instance.networkManager.GetServerIPAddress(), ()=> {
            aiClient.SearchMatch();
        });


        mainGameClient.SearchMatch();
    }

}
