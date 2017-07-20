using System;


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EntityView : MonoBehaviour
{
    public Entity entity;

    private EntityView()
    {


    }

    public static EntityView GetOne()
    {
        EntityView view = new EntityView();
        return view;
    }

    public void Init()
    {
        
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();

        if (entity.config.isTroop)
        {
            sr.sprite = ClientUtil.GetSprite("E-100_preview");
        }
        else if (entity.config.isTower)
        {
            sr.sprite = ClientUtil.GetSprite("KV-2_preview");
        }
    }

    public void Tick()
    {
        var curPos = entity.position;
        curPos.z = BattleViewController.ENTITY_VIEW_Z_OFFSET;
        transform.localPosition = curPos;
    }
}


