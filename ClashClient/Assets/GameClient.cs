﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
the idea is to set states in other functions
and in these PumpFunctions, do the actual work
*/ 

public class GameClient
{
    public Action OnLogin;
    public Action OnStartBattle;

    public int userId;
    public NetGameConnection connection;

    // we want to store all the message relevant to the simulation 
    // in here. Most of the time, simulation relevant stuff are frame sensitive,
    // meaning, the code needs to run at the correct frame, For example, casting cards,
    // therefore, we want to process these message at the correct frameNumber
    private Queue<Message> m_simMessageQueue;
    private ClientSimulation clientSim;

    public RateSmoother rateSmoother;
    public FrameBufferAnalyzer frameBufferAnalyzer;

    public GameClient()
    {
        m_simMessageQueue = new Queue<Message>();

        rateSmoother = new RateSmoother();
        rateSmoother.Init(RateSmoother.RateSmootherMode.Fastest);
//        rateSmoother.Init(RateSmoother.RateSmootherMode.Smooth);


        frameBufferAnalyzer = new FrameBufferAnalyzer();
        frameBufferAnalyzer.Init();
    }


    public void StartNetworkSession(string hostIPAddress, Action OnLoginIn)
    {
        connection = new NetGameConnection();
        connection.InitClientSideSocket();
        connection.OnHandleMessage = OnHandledMessage;

        Util.Log("Esablishing Connection to " + hostIPAddress);
        Util.LogError("Esablishing Connection to " + hostIPAddress);
        connection.SetConnectToHostInfo(hostIPAddress, NetworkManager.SERVER_PORT);
        connection.SetConnectionState(NetGameConnectionState.ClientContactingServer);


        Util.Log("StartNetworkSession");
        this.OnLogin = OnLoginIn;
        /*
        if (OnLogin != null)
        {
            OnLogin();
        }
        */


    }

    // There are multiple ways to organize it
    // game2 does it so that connection.Pump is in Update(), while clientSim.Tick() is in FixedUpdate()

    // we will see how good each "orgnaization" of code is
    public void Pump()
    {
        connection.Pump();

        if (clientSim != null)
        {
            frameBufferAnalyzer.Pump();
            rateSmoother.UpdateFrameToBufferAhead(frameBufferAnalyzer.GetComputedNumFrameToBuffer());
            Tick();
        }
    }

    public void ShutdownConnection()
    {
        connection.Close();
    }


    public void Tick()
    {
        rateSmoother.StartLoop();

        while (rateSmoother.ConsumeFrame())
        {
            while (m_simMessageQueue.Count > 0 && m_simMessageQueue.Peek().frameCount == clientSim.simulation.curFrameCount)
            {        
                Message msg = m_simMessageQueue.Dequeue();

                if (msg.type == Message.Type.CastCard)
                {
                    Debug.LogError("#########>>>>>>>>>>>>>> Casting Card");
                    clientSim.simulation.CastCard(msg.cardType, msg.teamId, msg.simPosition);
                }
            }
                
            clientSim.Tick();
        }
    }


    public void OnHandleSysMessage(NetGameConnection connection, Message message)
    {
        switch (message.type)
        {
            case Message.Type.SysPing:
                OnHandleSysPingMessage(connection, message);
                break;

            case Message.Type.SysHeartbeat:
                OnHandleSysHeartbeatMessage(connection, message);
                break;

            case Message.Type.SysKick:
                OnHandleSysKickMessage(connection, message);
                break;
        }
    }


    // make a register function 
    // see RegisterTerminalCommand 
    public void OnHandledMessage(NetGameConnection connection, Message message)
    {
        if (message.type == Message.Type.None)
        {
            Util.LogError("Message type is None");
            return;
        }


        if (message.IsSysMsg())
        {
            OnHandleSysMessage(connection, message);
        }
        else
        {
            switch (message.type)
            {
                case Message.Type.ServerConnectResponse:
                    Message loginRequest = Message.Login();
                    connection.SendMessage(loginRequest);
                    break;

                case Message.Type.LoginResponse:
                    //    Util.LogError("Handling LoginResponse");
                    this.userId = message.userId;
                    if (OnLogin != null)
                    {
                        OnLogin();
                    }

                    break;

                case Message.Type.BattleStartingInfo:
                    StartBattle(message.bs);
                    break;

                case Message.Type.EndFrame:

                    //                Util.LogError("EndFrame");
                    //           //     message.serverFrameInfo.Print();
                    //                Util.LogError("is serverFrameInfo null ? " + (message.serverFrameInfo == null));
                    //                Util.LogError("userId " + userId.ToString());
                    //                Util.LogError("clientSim ? " + (clientSim == null));
                    //                Util.LogError("clientSim.serverFrameInfoList ? " + (clientSim.serverFrameInfoList == null));
                    //
                    //
                    clientSim.serverFrameInfoList.Add(message.serverFrameInfo);
                    rateSmoother.AddNewFrame(message.serverFrameInfo.frameCount);
                    frameBufferAnalyzer.SetFrameHead(message.frameCount);
                    break;

                case Message.Type.CastCard:
                    Util.LogError("Cast Card");
                    Util.LogError("message " + message.playerId);
                    Util.LogError("message frameCount" + message.frameCount);

                    Util.LogError("simulation frameCount " + GetClientSimulation().simulation.curFrameCount.ToString());
                    m_simMessageQueue.Enqueue(message);
                    break;



                default:
                    Util.LogError("Default, message type " + message.type.ToString() + " not OnHandled");
                    break;
            }
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
        Debug.LogError(">>>>>>>>>>>> Game Client StartBattle");
        m_simMessageQueue.Clear();

        clientSim = new ClientSimulation();
        clientSim.state = ClientPlayerState.GetOne();
        clientSim.state.teamId = Enums.Team.Team0;
        clientSim.Init(bs);   

        if (OnStartBattle != null)
        {
            OnStartBattle();
        }
    }


    public void OnHandleSysPingMessage(NetGameConnection connection, Message message)
    {
        if (message.wantReply)
        {
            Message sysPingMessage2 = Message.SysPingMessage(message.pingId, message.timeStampInMs, false);
            connection.SendMessage(sysPingMessage2);
        }
        else
        {
            Int64 now_ms = Util.GetRealTimeMS();
            connection.pingHelper.UpdatePing(now_ms, message.timeStampInMs, message.pingId);
        }
    }

    public void OnHandleSysHeartbeatMessage(NetGameConnection connection, Message message)
    {
        if (message.wantReply)
        {
            Message sysHeartbeatMsg = Message.SysHeartbeatMessage(message.timeStampInMs, message.cargoSize, false);
            connection.SendMessage(sysHeartbeatMsg);
        }
        else
        {
            // not do anything
        }
    }

    public void OnHandleSysKickMessage(NetGameConnection connection, Message message)
    {
        Util.LogError("Disconnecting Now");
        connection.DisconnectNow();
    }

}

