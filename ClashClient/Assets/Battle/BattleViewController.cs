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

            GameObject go = new GameObject();

            EntityView view = go.AddComponent<EntityView>();
            view.Init();


            go.transform.SetParent(worldLayer.transform);


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

