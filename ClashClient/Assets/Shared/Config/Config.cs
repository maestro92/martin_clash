using System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
    public enum EntityType
    {
        None,
        KingTower,
        CrownTower,
        Footman,
        Bridge,
    }

    public enum Team
    {
        None,
        Team0,
        Team1,
    }

    public enum CardType
    {
        None,
        Summon_Footman,
    }
}


public class Config
{
    // will consider changing this to a list
    public static Dictionary<Enums.EntityType, EntityConfig> entityConfigs = new Dictionary<Enums.EntityType, EntityConfig>();
    public static Dictionary<Enums.CardType, CardConfig> cardConfigs = new Dictionary<Enums.CardType, CardConfig>();


    public Config()
    {
        
    }

    public static void Init()
    {
        EntityConfig config = new EntityConfig(Enums.EntityType.KingTower, false, true, true, "KingTower");
        entityConfigs.Add(Enums.EntityType.KingTower, config);

        config = new EntityConfig(Enums.EntityType.CrownTower, false, true, true, "CrownTower");
        entityConfigs.Add(Enums.EntityType.CrownTower, config);

        config = new EntityConfig(Enums.EntityType.Footman, true, false, true, "Footman");
        entityConfigs.Add(Enums.EntityType.Footman, config);

        /*
        List<Vector3> positions = new List<Vector3>{ new Vector3(0.5f, -0.5f, 0.0f), 
                                                    new Vector3(-0.5f, -0.5f, 0.0f), 
                                                    new Vector3(0.0f, 0.5f, 0.0f)};
                                                    */

        List<Vector3> positions = new List<Vector3>{ new Vector3(0.0f, 0.0f, 0.0f)};
        CardConfig cardConfig = new CardConfig(Enums.CardType.Summon_Footman, Enums.EntityType.Footman, positions);
        cardConfigs.Add(Enums.CardType.Summon_Footman, cardConfig);
    }
}

public class EntityConfig
{
    public Enums.EntityType type;
    public bool isTroop;
    public bool isTower;
    public bool hasPhysBody;
    public string displayName;
    public EntityConfig(Enums.EntityType typeIn, bool isTroopIn, bool isTowerIn, bool hasPhyBodyIn, string displayNameIn)
    {
        this.type = typeIn;
        this.isTroop = isTroopIn;
        this.isTower = isTowerIn;
        this.hasPhysBody = hasPhyBodyIn;
        this.displayName = displayNameIn;
    }

}


public class CardConfig
{
    public Enums.CardType cardType;
    public Enums.EntityType entityType;
    public List<Vector3> positions;

    public CardConfig(Enums.CardType cardTypeIn, Enums.EntityType entityTypeIn, List<Vector3> positionsIn)
    {
        this.cardType = cardTypeIn;
        this.entityType = entityTypeIn;
        this.positions = positionsIn;
    }
}