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
        connection.OnHandleMessage = OnHandledMessage;

        Util.Log("Esablishing Connection to " + hostIPAddress);

        connection.ConnectToHost(hostIPAddress, NetworkManager.SERVER_PORT);

        Util.Log("StartNetworkSession");
    }


    public void Pump()
    {
        connection.Pump();

        if (clientSim != null)
        {
            clientSim.Tick();
        }
    }



    public void OnHandledMessage(NetGameConnection connection, Message message)
    {
        if (message.type == Message.Type.None)
        {
            Util.LogError("Message type is None");
            return;
        }

        switch (message.type)
        {
            case Message.Type.ServerConnectResponse:
                Util.LogError("ServerClientResponse");
                Message loginRequest = Message.Login();
                connection.SendMessage(loginRequest);
                break;

            case Message.Type.LoginResponse:
                Util.LogError("LoginResponse");
                break;
                         
            default:
                break;
        }

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


        // start up an AI
        GameClient aiClient = new GameClient();
        aiClient.StartNetworkSession(Main.instance.networkManager.GetServerIPAddress());


    }

    public void StartBattle(BattleStartingInfo bs)
    {
        clientSim = new ClientSimulation();
        clientSim.state = ClientPlayerState.GetOne();
        clientSim.state.teamId = Enums.Team.Team0;
        clientSim.Init();   
    }
}

