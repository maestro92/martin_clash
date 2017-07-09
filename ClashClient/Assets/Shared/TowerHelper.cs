using System;


public class TowerHelper
{
    private TowerHelper()
    {



    }

    public static TowerHelper GetOne()
    {
        TowerHelper towerHelper = new TowerHelper();
        return towerHelper;
    }
}

