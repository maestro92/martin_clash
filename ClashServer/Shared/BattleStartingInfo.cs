using System;
using System.Collections;
using System.Collections.Generic;


// I plan to support 2v2s


public class PlayerInfo
{
	public int userId;

}

public class BattleStartingInfo
{
	// playerIds
	public List<PlayerInfo> team0;
	public List<PlayerInfo> team1;

    public BattleStartingInfo()
    {
		
    }

	public void AddPlayer(Enums.Team teamId, PlayerInfo playerInfo)
	{
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
	}
}

