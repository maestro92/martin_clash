using System;

public class Entity
{
    public EntityConfig config;
    public PhysBody physbody;
    public TroopHelper troopHelper;
    public TowerHelper towerHelper;

	private Entity()
	{


	}


    public static Entity GetOne(EntityType type)
    {
        Entity entity = new Entity();
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

