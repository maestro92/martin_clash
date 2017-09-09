using System;
using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;
public class MatchMaker
{
	public List<ServerClientHandle> playerQueue;

	public MatchMaker()
	{
		playerQueue = new List<ServerClientHandle>();
	}

	public void AddPlayerToQueue(ServerClientHandle player)
	{
		playerQueue.Add(player);
		Util.Log(GetQueueStatus());

	}

	public void Tick()
	{
		while (playerQueue.Count > 0)
		{
			ServerClientHandle player0 = playerQueue[0];
		//	ServerClientHandle player1 = playerQueue[1];

			playerQueue.RemoveAt(0);
		//	playerQueue.RemoveAt(0);    // second player will be at index0 after the first one gets removed

			PlayerInfo playerInfo0 = new PlayerInfo();
		//	playerInfo0.userId = player0.id;
			playerInfo0.userId = -200;


			//	PlayerInfo playerInfo1 = new PlayerInfo();
		//	playerInfo1.userId = player1.id;

			BattleStartingInfo bs = BattleStartingInfo.GetOne();
			bs.AddPlayer(Enums.Team.Team0, playerInfo0);
			//	bs.AddPlayer(Enums.Team.Team1, playerInfo1);
			bs.filler = -8;
			bs.mapId = 5;

			Message message = Message.BattleStartingInfoMessage(bs);
		//	Util.Log("Sending BattleStartingInfo to " + player0.id.ToString() + " " + player1.id.ToString());
			player0.GetGameConnection().SendMessage(message);
		//	player1.GetGameConnection().SendMessage(message);

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

