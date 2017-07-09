using System;
using System.Collections;
using System.Collections.Generic;
// using UnityEngine;







// this is gonna be shared among client and server
public class Simulation 
{
    public Map map;
    public List<Entity> m_entities;

    private List<Entity> m_entitiesToAdd;
    private List<Entity> m_entitiesToRemove;

    public Action<Entity> OnAddEntity;

    public void Init(BattleStartingInfo bs)
    {
        m_entities = new List<Entity>();
        // init map

        List<Entity> temp = new List<Entity>();
        Entity tower0L = Entity.GetOne(EntityType.CrownTower);
        Entity tower0R = Entity.GetOne(EntityType.CrownTower);
        Entity tower0K = Entity.GetOne(EntityType.KingTower);

        Entity tower1L = Entity.GetOne(EntityType.CrownTower);
        Entity tower1R = Entity.GetOne(EntityType.CrownTower);
        Entity tower1K = Entity.GetOne(EntityType.KingTower);

        temp.Add(tower0L);
        temp.Add(tower0R);
        temp.Add(tower0K);

        temp.Add(tower1L);
        temp.Add(tower1R);
        temp.Add(tower1K);

        foreach (var tower in temp)
        {
            tower.Init();
            AddEntity(tower);
        }
    }

	// Update is called once per frame
	void Update () 
	{
		foreach (var entity in m_entities)
		{
			entity.Tick();
		}

        foreach (var entity in m_entitiesToRemove)
        {
            m_entities.Remove(entity);
        }
        m_entitiesToRemove.Clear();


        foreach(var entity in m_entitiesToAdd)
        {
            AddEntityNow(entity);
        }
        m_entitiesToAdd.Clear();
	}

    public void AddEntity(Entity entity)
    {
        m_entitiesToAdd.Add(entity);
    }

    private void AddEntityNow(Entity entity)
    {
        m_entities.Add(entity);

        if (OnAddEntity != null)
        {
            OnAddEntity(entity);
        }
    }
}
