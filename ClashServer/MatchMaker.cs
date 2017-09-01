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


	}

	public void AddPlayerToQueue(ServerClientHandle player)
	{
		playerQueue.Add(player);
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
			PlayerInfo playerInfo1 = new PlayerInfo();
			playerInfo1.userId = player1.id;

			BattleStartingInfo bs = new BattleStartingInfo();
			bs.AddPlayer(Enums.Team.Team0, playerInfo0);
			bs.AddPlayer(Enums.Team.Team1, playerInfo1);

			Message message = Message.BattleStartingInfo(bs);

			player0.GetGameConnection().SendMessage(message);
			player1.GetGameConnection().SendMessage(message);

		}

	}

}

