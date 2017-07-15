using System;
using UnityEngine;

public class TroopHelper
{
    public Entity entity;
    private Entity m_attackerTarget;

    private TroopHelper()
    {

    }

    public static TroopHelper GetOne(Entity ent)
    {
        TroopHelper troopHelper = new TroopHelper();
        troopHelper.entity = ent;
        troopHelper.m_attackerTarget = null;
        return troopHelper;
    }


    public void Tick()
    {
        if (m_attackerTarget == null)
        {
            // search for a target

        }

        GridCoord coord = entity.simulation.map.SimPosToGridCoord(entity.position);
        entity.physbody.desiredVelocity = entity.simulation.map.GetVelocity(coord);

    }
}

