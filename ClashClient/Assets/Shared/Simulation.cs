using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





// this is gonna be shared among client and server
public class Simulation 
{
    public int curFrameCount;
    public Map map;
    public List<Entity> m_entities;

    private List<Entity> m_entitiesToAdd;
    private List<Entity> m_entitiesToRemove;

    public Action<Entity> OnAddEntity;




    public void Init(BattleStartingInfo bs)
    {
        m_entities = new List<Entity>();
        m_entitiesToAdd = new List<Entity>();
        m_entitiesToRemove = new List<Entity>();

        // init map

        map = new Map();
        map.OnTowerPositionsReady = (towerPositions) =>
            {
                List<Entity> temp = new List<Entity>();
                Entity tower0L = Entity.GetOne(Enums.EntityType.CrownTower);    
                tower0L.teamId = Enums.Team.Team0;  
                tower0L.towerHelper.isTowerA = true;    

                Entity tower0R = Entity.GetOne(Enums.EntityType.CrownTower);    
                tower0R.teamId = Enums.Team.Team0;  
                tower0R.towerHelper.isTowerB = true;    

                Entity tower0K = Entity.GetOne(Enums.EntityType.KingTower);     
                tower0K.teamId = Enums.Team.Team0;  

                Entity tower1L = Entity.GetOne(Enums.EntityType.CrownTower);   
                tower1L.teamId = Enums.Team.Team1;  
                tower1L.towerHelper.isTowerA = true;

                Entity tower1R = Entity.GetOne(Enums.EntityType.CrownTower);    
                tower1R.teamId = Enums.Team.Team1;  
                tower1R.towerHelper.isTowerB = true;

                Entity tower1K = Entity.GetOne(Enums.EntityType.KingTower);     
                tower1K.teamId = Enums.Team.Team1;  

                temp.Add(tower0L);
                temp.Add(tower0R);
                temp.Add(tower0K);

                temp.Add(tower1L);
                temp.Add(tower1R);
                temp.Add(tower1K);

                int i = 0;
                foreach (var tower in temp)
                {
                    tower.position = towerPositions[i];
            //        Util.LogError("tower Position " + tower.position);
                    AddEntityNow(tower);
                    i++;
                }
            };

        map.Init();



    }


    private void InitTowers()
    {


    }


	// Update is called once per frame
	public void Tick () 
	{
	//	Util.LogError("curFrameCount " + curFrameCount.ToString());

        curFrameCount++;

		foreach (var entity in m_entities)
		{
			entity.Tick();
		}

        foreach (var entity in m_entitiesToRemove)
        {
            m_entities.Remove(entity);
        }
        m_entitiesToRemove.Clear();


        foreach(var entity in m_entitiesToAdd)
        {
        //    Util.LogError("adding entities");
            AddEntityNow(entity);
        }
        m_entitiesToAdd.Clear();
	}

    public void AddEntity(Entity entity)
    {
        m_entitiesToAdd.Add(entity);
    }

    private void AddEntityNow(Entity entity)
    {
        entity.simulation = this;
        m_entities.Add(entity);

        if (OnAddEntity != null)
        {
            OnAddEntity(entity);
        }
    }


    public void CastCard(Enums.CardType cardType, Enums.Team teamId, Vector3 position)
    {
        CardConfig config = Config.cardConfigs[cardType];
//        Debug.LogError("cast card");
//        Debug.LogError("\tcast count " + config.positions.Count);

        foreach (var pos in config.positions)
        {
            Entity entity = Entity.GetOne(config.entityType);

            Vector3 clampedPos = position + pos;


            clampedPos = map.ClampSimPos(clampedPos);
         //   Debug.LogError("\tcast point " + clampedPos);
         //   Debug.LogError("\tgridCoord is " + map.SimPosToGridCoord(clampedPos).x + " " + map.SimPosToGridCoord(clampedPos).y);


            DeployEntity(entity, teamId, clampedPos);
        }
    }

    public void DeployEntity(Entity entity, Enums.Team teamId, Vector3 pos)
    {
        entity.position = pos;
        entity.teamId = teamId;
        AddEntity(entity);
    }
}
