using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class BattleViewController : MonoBehaviour
{

    public GameObject uiLayer;
    public GameObject worldLayer;


    public List<EntityView> entityViews;

    public ClientSimulation clientSim;

    public BattleViewController()
    {

    }


    public void Init(ClientSimulation clientSimIn)
    {
        clientSim = clientSimIn;


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
            go.transform.localPosition = Vector3.zero;

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

