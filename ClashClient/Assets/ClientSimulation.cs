using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ClientSimulation
{
    public ClientPlayerState state;
    public Simulation simulation;

    // 
    public List<ServerFrameInfo> serverFrameInfoList;
    public int endMatchFrameNumber;


    public ClientSimulation()
    {

    }

    public void Init(BattleStartingInfo bs)
    {
        serverFrameInfoList = new List<ServerFrameInfo>();
        simulation = new Simulation();
        simulation.Init(bs);

    }


    public void AddNewServerFrame(ServerFrameInfo serverFrameInfo)                
    {
        serverFrameInfoList.Add(serverFrameInfo);        
    }


    public bool Tick()
    {
//        Debug.LogError("serverFrameInfoList Count " + serverFrameInfoList.Count.ToString());
       /*
        if (serverFrameInfoList != null && serverFrameInfoList.Count > 0)
        {
            Util.LogError("count is " + serverFrameInfoList.Count);
            Util.LogError("frameCount is " + serverFrameInfoList[serverFrameInfoList.Count - 1].frameCount.ToString());

            Util.LogError("\tserverFrameInfoList last frame " + serverFrameInfoList[serverFrameInfoList.Count - 1].frameCount.ToString());
        }
        */

        if(serverFrameInfoList.Count <= 0)
        {
            Util.Log("serverFrameInfoList is empty, No Frames to consume");
            return false;
        }

        if (serverFrameInfoList[0].frameCount <= simulation.curFrameCount)
        {
            Util.Log("serverFrameInfoList is lower than simulation curFrameCount");
            return false;
        }

        // make this into a queue?
        serverFrameInfoList.RemoveAt(0);
        simulation.Tick();
        return true;      
    }
}

