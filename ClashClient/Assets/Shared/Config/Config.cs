﻿using System;
using System;
using System.Collections;
using System.Collections.Generic;

public class Enums
{
    public enum EntityType
    {
        KingTower,
        CrownTower,
        Footman,

    }
}


public class Config
{
    public static Dictionary<Enums.EntityType, EntityConfig> entityConfigs = new Dictionary<Enums.EntityType, EntityConfig>();

    public Config()
    {
        
    }

    public static void Init()
    {
        EntityConfig config = new EntityConfig();

        config = new EntityConfig(Enums.EntityType.KingTower, false, true, true);
        entityConfigs.Add(Enums.EntityType.KingTower, config);

        config = new EntityConfig(Enums.EntityType.CrownTower, false, true, true);
        entityConfigs.Add(Enums.EntityType.CrownTower, config);

        config = new EntityConfig(Enums.EntityType.Footman, true, false, true);
        entityConfigs.Add(Enums.EntityType.Footman, config);
    }
}

public class EntityConfig
{



    public Enums.EntityType type;
    public bool isTroop;
    public bool isTower;
    public bool hasPhysBody;

    public EntityConfig(Enums.EntityType typeIn, bool isTroopIn, bool isTowerIn, bool hasPhyBodyIn)
    {
        this.type = typeIn;
        this.isTroop = isTroopIn;
        this.isTower = isTowerIn;
        this.hasPhysBody = hasPhyBodyIn;
    }

}