using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameClient
{
  //  public Action OnLogin

    public GameClient()
    {

    }

    public NetGameConnection connection;

    private ClientSimulation clientSim;

    public void StartNetworkSession(string hostIPAddress, Action OnLogin)
    {
        connection = new NetGameConnection();
        connection.InitClientSideSocket();
        connection.OnHandleMessage = OnHandledMessage;

        Util.Log("Esablishing Connection to " + hostIPAddress);

        connection.ConnectToHost(hostIPAddress, NetworkManager.SERVER_PORT);

        Util.Log("StartNetworkSession");

        if (OnLogin != null)
        {
            OnLogin();
        }


    }

    // There are multiple ways to organize it
    // game2 does it so that connection.Pump is in Update(), while clientSim.Tick() is in FixedUpdate()

    // we will see how good each "orgnaization" of code is
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
                Util.LogError("Handling ServerClientResponse");
                Message loginRequest = Message.Login();
                connection.SendMessage(loginRequest);
                break;

            case Message.Type.LoginResponse:
                Util.LogError("Handling LoginResponse");
                break;
                         
            case Message.Type.BattleStartingInfo:
                Util.Log("Handling BattleStartingInfo Message");
                Main.instance.GoToBattle();
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
        Message message = Message.SearchMatch();
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

