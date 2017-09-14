using System;
using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;


public class MatchManager
{
	public List<ServerClientHandle> playerQueue;
	public List<ServerSimulation> matches;
	public object matchesLock;
	public Util.RateRegulator rateRegulator;

	public MatchManager()
	{
		playerQueue = new List<ServerClientHandle>();
		matches = new List<ServerSimulation>();
		matchesLock = new object();

		rateRegulator = new Util.RateRegulator(Globals.FRAMES_PER_SECOND);
		rateRegulator.Start();

	}

	public void AddPlayerToQueue(ServerClientHandle player)
	{
		playerQueue.Add(player);
		Util.Log(GetQueueStatus());

	}

	public void Tick()
	{
		while (playerQueue.Count > 1)
		{
			ServerClientHandle player0 = playerQueue[0];
			ServerClientHandle player1 = playerQueue[1];

			playerQueue.RemoveAt(0);
			playerQueue.RemoveAt(0);    // second player will be at index0 after the first one gets removed

			PlayerInfo playerInfo0 = new PlayerInfo();
			playerInfo0.userId = player0.id;
			playerInfo0.userId = -200;


			PlayerInfo playerInfo1 = new PlayerInfo();
			playerInfo1.userId = player1.id;

			BattleStartingInfo bs = BattleStartingInfo.GetOne();
			bs.AddPlayer(Enums.Team.Team0, playerInfo0);
			bs.AddPlayer(Enums.Team.Team1, playerInfo1);
			bs.mapId = 5;
			List<ServerClientHandle> clients = new List<ServerClientHandle>();
			clients.Add(player0);
			clients.Add(player1);

			Message message = Message.BattleStartingInfoMessage(bs);
		//	Util.Log("Sending BattleStartingInfo to " + player0.id.ToString() + " " + player1.id.ToString());
			player0.GetGameConnection().SendMessage(message);
			player1.GetGameConnection().SendMessage(message);

			StartMatch(bs, clients);
		}


		lock(matchesLock)
		{
		//	Util.LogError("\t\t>>>>> new While Loop");
			// this is essentially rateRegulator.FixedUpdate()
			while (rateRegulator.ShouldPump())
			{
		//		Util.LogError("rateRegulator.ShouldPunmp");
				rateRegulator.Pump();
			//	rateRegulator.Print();

				foreach (var match in matches)
				{
					match.Tick();
				}
			}
		}
	}


	public ServerSimulation FindSimulation(ServerClientHandle client)
	{
		lock(matchesLock)
		{
			foreach (var match in matches)
			{
				if (match.gameClients.Contains(client) == true)
				{
					return match;
				}
			}
		}
		return null;
	}

	public void StartMatch(BattleStartingInfo bs, List<ServerClientHandle> clients)
	{		
		ServerSimulation serverSim = new ServerSimulation();
		serverSim.Init(bs, clients);

		lock(matchesLock)
		{
			matches.Add(serverSim);
		}
	}

	public string GetQueueStatus()
	{
		string msg = "";
		msg += "Matchmaking queue: " + playerQueue.Count + " players waiting";

		foreach (ServerClientHandle sch in playerQueue)
		{
			msg += "\n\tplayer" + sch.id + " is waiting";
		}
		return msg;
	}



}

