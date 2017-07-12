using System;


public class TowerHelper
{

    public bool isTowerA;
    public bool isTowerB;

    private TowerHelper()
    {
        isTowerA = false;
        isTowerB = false;
    }

    public static TowerHelper GetOne()
    {
        TowerHelper towerHelper = new TowerHelper();
        return towerHelper;
    }
}

