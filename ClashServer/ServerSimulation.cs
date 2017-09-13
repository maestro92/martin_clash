using System;
using System.Collections.Generic;

// the reason why we have both ServerSimulation and ClientSimulation is becuz
// we do different things in there

// ServerSimulation has to send down the "EndFrame" information to clients

// also for "Security" reasons
// Server will keep a copy of playerInfo (mana, cards, health) 
// only the simulation specific logic is shared
public class ServerSimulation
{
	//	public ClientPlayerState state;
	public List<ServerClientHandle> gameClients;
//

	public Simulation simulation;

	public ServerSimulation()
	{

	}

	public void Init(BattleStartingInfo bs, List<ServerClientHandle> gameClientsIn)
	{
		simulation = new Simulation();
		simulation.Init(bs);

		gameClients = gameClientsIn;


	}


	public void Tick()
	{
		simulation.Tick();

		ServerFrameInfo sfi = ServerFrameInfo.GetOne();
		sfi.frameCount = simulation.curFrameCount;
		Message endFrame = Message.EndFrame(sfi);
		BroadCastMsgNoWait(endFrame);
	}


	public void BroadCastMsgNoWait(Message message)
	{
		foreach (var client in gameClients)
		{
			client.GetGameConnection().SendMessage(message);
		}

	}
}

