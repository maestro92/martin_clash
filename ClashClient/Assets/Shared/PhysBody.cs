using System;
using UnityEngine;

public class PhysBody
{
    public Entity entity;

    public Vector3 desiredVelocity;

	private PhysBody()
	{

    }

    public static PhysBody GetOne(Entity ent)
    {
        PhysBody physBody = new PhysBody();
        physBody.entity = ent;
        return physBody;
    }

    public void Tick()
    {
        /*
        if (entity.type == Enums.EntityType.Footman)
        {
            Util.LogError("desiredVelocity " + desiredVelocity);
            Util.LogError("\tentity.position " + entity.position + " Globals.FIXED_UPDATE_TIME_s;" + Globals.FIXED_UPDATE_TIME_s.ToString());               
        }
        */
        entity.position += desiredVelocity * Globals.FIXED_UPDATE_TIME_s;

        /*
        if (entity.type == Enums.EntityType.Footman)
        {
            Util.LogError("\tdesiredVelocity " + (desiredVelocity * Globals.FIXED_UPDATE_TIME_s));
            Util.LogError("\tentity.position " + entity.position);
        }
        */
    }

    public Vector3 GetPosition()
    {
        return entity.position;
    }
}
