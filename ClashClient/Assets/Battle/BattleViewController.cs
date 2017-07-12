using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class BattleViewController : MonoBehaviour
{

    public GameObject uiLayer;
    public GameObject worldLayer;

    // left and right is from your point of view
    public GameObject leftBridge;   
    public GameObject rightBridge;

    // such as the bridge
    public List<EntityView> backgroundEntityViews;
    public List<EntityView> entityViews;

    public ClientSimulation clientSim;

    public float ENTITY_VIEW_Z_OFFSET = -5;

    public BattleViewController()
    {

    }


    public void Init(ClientSimulation clientSimIn)
    {
        clientSim = clientSimIn;

        backgroundEntityViews = new List<EntityView>();
        entityViews = new List<EntityView>();


        clientSim.simulation.OnAddEntity = (entity) => {

            Util.LogError("Here in OnAddEntity");

            GameObject go = ClientUtil.Instantiate("EntitySprite");

            EntityView view = go.AddComponent<EntityView>();
            view.entity = entity;
            view.Init();
            go.SetActive(true);

            go.name = entity.config.displayName;
            go.transform.SetParent(worldLayer.transform);
           // go.transform.localPosition = Vector3.zero;
            var curPos = view.entity.position;
            curPos.z = ENTITY_VIEW_Z_OFFSET;

            go.transform.localPosition = curPos;


            if(entity.config.type == Enums.EntityType.CrownTower)
            {
                if(entity.teamId == clientSim.state.teamId)
                {
                    if(entity.towerHelper.isTowerA)
                    {
                        var tempPos = curPos;
                        tempPos.y = 0;

                        Util.LogError("\t\tisTowerA " + tempPos.ToString());
                        leftBridge.transform.localPosition = tempPos;
                        Util.LogError("\t\tleftBridge " + leftBridge.transform.localPosition.ToString());
                    }
                    else if (entity.towerHelper.isTowerB)
                    {
                        var tempPos = curPos;

                        tempPos.y = 0;
                            
                        Util.LogError("\t\tisTowerB " + tempPos.ToString());

                        rightBridge.transform.localPosition = tempPos;

                        Util.LogError("\t\trightBridge " + rightBridge.transform.localPosition.ToString());
                    }
                }
            }


            entityViews.Add(view);
        };

        clientSim.simulation.Init(null);
    }


    // this is the render Update()
    public void Update()
    {
        foreach (var ev in entityViews)
        {
            ev.Tick();
        }
    }
}

