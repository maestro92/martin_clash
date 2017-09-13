using System;
using System.Collections;
using System.Collections.Generic;


// I plan to support 2v2s


public class PlayerInfo : INetSerializer
{
	public int userId;
	public int playerId;

	static public PlayerInfo GetOne()
	{
		PlayerInfo pi = new PlayerInfo();
		return pi;
	}

	public void Serialize(NetSerializer writer)
	{
		writer.WriteInt32(userId, "userId");
		writer.WriteInt32(playerId, "playerId");
	}

	public void Deserialize(NetSerializer reader)
	{
		userId = reader.ReadInt32("userId");
		playerId = reader.ReadInt32("playerId");

	}

    public void Print()
    {
        Util.LogError("userId " + userId.ToString());
		Util.LogError("playerId " + playerId.ToString());
	}
}

public class BattleStartingInfo : INetSerializer
{
	// playerIds
	// public List<PlayerInfo> team0;
	// public List<PlayerInfo> team1;

	public PlayerInfo playerInfo0;
	public PlayerInfo playerInfo1;

	public int mapId;

    public int filler;

    private BattleStartingInfo()
    {
		//	team0 = new List<PlayerInfo>();
		//	team1 = new List<PlayerInfo>();
		playerInfo0 = new PlayerInfo();
		playerInfo1 = new PlayerInfo();	
	}


	static public BattleStartingInfo GetOne()
	{
		BattleStartingInfo bs = new BattleStartingInfo();
		return bs;
	}

	public void AddPlayer(Enums.Team teamId, PlayerInfo playerInfo)
	{
	/*
		if (teamId == Enums.Team.Team0)
		{
			team0.Add(playerInfo);
		}
		else if (teamId == Enums.Team.Team1)
		{
			team1.Add(playerInfo);
		}
		else 
		{
			Util.LogError("Invalid teamId, not adding to any team");
		}
		*/

		if (teamId == Enums.Team.Team0)
		{
			playerInfo0 = playerInfo;
		}
		else if (teamId == Enums.Team.Team1)
		{
			playerInfo1 = playerInfo;
		}
		else
		{
			Util.LogError("Invalid teamId, not adding to any team");
		}
	}

    public void Serialize(NetSerializer writer)
    {
		writer.WriteOne<PlayerInfo>(playerInfo0, "playerInfo0");
        writer.WriteOne<PlayerInfo>(playerInfo1, "playerInfo1");
		writer.WriteInt32(mapId, "mapId");
    }

    public void Deserialize(NetSerializer reader)
    {
        playerInfo0 = reader.ReadOne<PlayerInfo>(()=>PlayerInfo.GetOne(), "playerInfo0");
        playerInfo1 = reader.ReadOne<PlayerInfo>(()=>PlayerInfo.GetOne(), "playerInfo1");
		mapId = reader.ReadInt32("mapId");
    }

    public void Print()
    {
   //     Util.LogError("filler " + filler.ToString());
    //    playerInfo1.Print();
    //    Util.LogError("mapId " + mapId.ToString());
    }
}

