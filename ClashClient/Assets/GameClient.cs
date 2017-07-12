using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameClient
{

    public GameClient()
    {

    }

    public NetGameConnection connection;

    private ClientSimulation clientSim;

    public void StartNetworkSession(string hostIPAddress)
    {
        connection = new NetGameConnection();
        connection.InitClientSideSocket();

        Util.Log("Esablishing Connection to " + hostIPAddress);

        connection.ConnectToHost(hostIPAddress, NetworkManager.SERVER_PORT);
        Util.Log("StartNetworkSession");
    }


    public void Pump()
    {
        connection.Pump();
    }



    public ClientSimulation GetClientSimulation()
    {
        return clientSim;
    }

    public void Login()
    {
        Message message = Message.Login();
        connection.SendMessage(message);
    }

    public void SearchMatch()
    {
        Debug.LogError("SearchMatch()");
        Message message = Message.SearchMessage();
        connection.SendMessage(message);
    }

    public void StartBattle(BattleStartingInfo bs)
    {
        clientSim = new ClientSimulation();
        clientSim.state = ClientPlayerState.GetOne();
        clientSim.state.teamId = Enums.Team.Team0;
        clientSim.Init();   
    }
}

