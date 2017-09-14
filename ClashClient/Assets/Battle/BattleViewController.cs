using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class BattleViewController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public GameObject uiLayer;
    public GameObject worldLayer;

    public Camera worldCamera;

    // left and right is from your point of view
    public GameObject leftBridge;   
    public GameObject rightBridge;
    public GameObject grass;
    public GameObject background; 
    public GameObject river;

    public GameObject tilePrefab;

    // such as the bridge
    public List<EntityView> backgroundEntityViews;
    public List<EntityView> entityViews;

    List<GameObject> tiles = new List<GameObject>();

    public ClientSimulation clientSim;
    public ClientPlayerState myPlayerState;

    public static float ENTITY_VIEW_Z_OFFSET = -5;

    public BattleViewController()
    {

    }


    public void Init(ClientSimulation clientSimIn)
    {
        clientSim = clientSimIn;

        backgroundEntityViews = new List<EntityView>();
        entityViews = new List<EntityView>();


        myPlayerState = clientSim.state;

        clientSim.simulation.OnAddEntity = (entity) => {

      //      Util.LogError("Here in OnAddEntity");

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

                //        Util.LogError("\t\tisTowerA " + tempPos.ToString());
                        leftBridge.transform.localPosition = tempPos;
                //        Util.LogError("\t\tleftBridge " + leftBridge.transform.localPosition.ToString());
                    }
                    else if (entity.towerHelper.isTowerB)
                    {
                        var tempPos = curPos;

                        tempPos.y = 0;
                            
                 //       Util.LogError("\t\tisTowerB " + tempPos.ToString());

                        rightBridge.transform.localPosition = tempPos;

                 //       Util.LogError("\t\trightBridge " + rightBridge.transform.localPosition.ToString());
                    }
                }
            }


            entityViews.Add(view);
        };

        clientSim.simulation.Init(null);


        Map map = clientSim.simulation.map;
        var edge = clientSim.simulation.map.max;
        var scale = grass.transform.localScale;
        scale.x = edge.x;
        scale.y = edge.y;
        grass.transform.localScale = scale;

        edge = clientSim.simulation.map.max + new Vector3(5.0f, 5.0f, 0f);
        scale.x = edge.x;
        scale.y = edge.y;
        background.transform.localScale = scale;

        scale = new Vector3(1.5f, 1f, 0);
        leftBridge.transform.localScale = scale;
        rightBridge.transform.localScale = scale;

        scale = new Vector3(clientSim.simulation.map.halfWidth, 1f, 0);
        river.transform.localScale = scale;

        /*
        for(int y=0; y<map.height; y++)
        {
            for(int x=0; x<map.width; x++)
            {
                var tile = GameObject.Instantiate<GameObject>(tilePrefab);
                tile.transform.SetParent(worldLayer.transform);
                tile.SetActive(true);
                tile.transform.localPosition = map.GridCoordToSimPos(x, y);
                tile.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

                tiles.Add(tile);
            }
        }
*/

        Util.LogError("tiles count " + tiles.Count);

    }


    // this is the render Update()
    public void Update()
    {
        foreach (var ev in entityViews)
        {
            ev.Tick();
        }
    }

    public void OnPointerDown(PointerEventData eventData) 
    {
    //    Util.LogError("OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData) 
    {
        if (Main.instance.ui.battleHud.TouchInPlay(eventData.position, Main.instance.uiCamera) == true)
        {

            Debug.LogError("eventData.position " + eventData.position.ToString());

            if (BattleHudCardBtnController.lastSelectedBtn != null)
            {
                
                Vector3 worldPoint = worldCamera.ScreenToWorldPoint(eventData.position);
//                Vector3 localWorldPoint = grass.transform.InverseTransformPoint(worldPoint);

                // transfroms worldPoint from worldspace to localSpace
                Vector3 localWorldPoint = worldLayer.transform.InverseTransformPoint(worldPoint);
                localWorldPoint.z = 0;


            //    Debug.LogError("eventData.position " + eventData.position);
            //    Debug.LogError("worldPoint " + localWorldPoint);



                Vector3 simPos = world2Sim(localWorldPoint);


           //     clientSim.simulation.CastCard(BattleHudCardBtnController.lastSelectedBtn.cardConfig.cardType, clientSim.state.teamId, localWorldPoint);
                Message castCardMsg = Message.CastCard(BattleHudCardBtnController.lastSelectedBtn.cardConfig.cardType, 
                    myPlayerState.playerId, simPos, 0, true);
                Main.instance.mainGameClient.connection.SendMessage(castCardMsg);
            }
        }
        else
        {
            Util.LogError("\tTouch Not in Play");
        }
    }
        

    public static Vector3 world2Sim(Vector3 worldPoint)
    {
        return worldPoint;
    }

}

