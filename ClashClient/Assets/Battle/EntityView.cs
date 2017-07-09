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
        SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();

        if (entity.config.isTroop)
        {
            sr.sprite = ClientUtil.GetSprite("M-6_preview.png");
        }
        else if (entity.config.isTower)
        {
            sr.sprite = ClientUtil.GetSprite("T34_preview.png");
        }
    }

    public void Tick()
    {


    }
}


