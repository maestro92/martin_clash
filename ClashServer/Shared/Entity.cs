using System;
using UnityEngine;

public class Entity
{
    public Enums.EntityType type;
    public EntityConfig config;
    public PhysBody physbody;
    public TroopHelper troopHelper;
    public TowerHelper towerHelper;
    public Enums.Team teamId;
    // need to change this to fixed point math
    public Vector3 position;

	private Entity()
	{


	}


    public static Entity GetOne(Enums.EntityType type)
    {
        Entity entity = new Entity();
        entity.type = type;
        entity.config = Config.entityConfigs[type];
        entity.Init();
        return entity;
    }

    public void Init()
    {
        if (config.hasPhysBody == true)
        {
            physbody = PhysBody.GetOne();
        }

        if (config.isTroop == true)
        {
            troopHelper = TroopHelper.GetOne();
        }

        if (config.isTower == true)
        {
            towerHelper = TowerHelper.GetOne();
        }


    }


	public void Tick()
	{


	}




}

