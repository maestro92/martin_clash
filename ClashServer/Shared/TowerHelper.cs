using System;


public class TowerHelper
{
    public Entity entity;
    public bool isTowerA;
    public bool isTowerB;

    private TowerHelper()
    {
        isTowerA = false;
        isTowerB = false;
    }

    public static TowerHelper GetOne(Entity ent)
    {
        TowerHelper towerHelper = new TowerHelper();
        towerHelper.entity = ent;
        return towerHelper;
    }

    public void Tick()
    {

    }
}

