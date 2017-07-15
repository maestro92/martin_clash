using System;
using UnityEngine;

public class Entity
{    
    public Enums.EntityType type;
    public EntityConfig config;
    public PhysBody physbody;
    public TroopHelper troopHelper;
    public TowerHelper towerHelper;
    public AttackerHelper attackerHelper;
    public Enums.Team teamId;
    // need to change this to fixed point math
    public Vector3 position;

    public Simulation simulation;

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
        attackerHelper = AttackerHelper.GetOne();

        if (config.hasPhysBody == true)
        {
            physbody = PhysBody.GetOne(this);
        }

        if (config.isTroop == true)
        {
            troopHelper = TroopHelper.GetOne(this);
        }

        if (config.isTower == true)
        {
            towerHelper = TowerHelper.GetOne(this);
        }
    }


	public void Tick()
	{
        if (physbody != null)
        {
            physbody.Tick();
        }

        if (troopHelper != null)
        {
            troopHelper.Tick();
        }

        if (towerHelper != null)
        {
            towerHelper.Tick();
        }
    }




}

