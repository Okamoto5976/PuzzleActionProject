using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Use Map Generate
[System.Serializable]
public class RoomData
{
    public int m_ID;
    public AreaType m_type;
    public List<Vector2Int> m_roomSizes;

    public RoomData(int id, AreaType type, List<Vector2Int> roomSizes)
    {
        m_ID = id;
        m_type = type;
        m_roomSizes = roomSizes;
    }

    public void SetID(int id)
    {
        m_ID = id;
    }
}

public class MapPlaceSystem : MonoBehaviour
{
    [SerializeField] private InputActionReference m_action;

    [SerializeField] private Transform m_parent;//mousePos parent
    [SerializeField] private Camera m_mainCamera;

    private Vector3 m_mouseWorldPos;

    private GameObject m_roomPieceParentObj;//bringing piece now
    private RectTransform m_roomPieceParentRect;


    //use GenerateMap MainSece, PlaceRoomDatas
    private List<RoomData> m_roomData = new();

   
    //Use Place Room--------------------------
    //you have room naw,
    private Room m_room;

    public Room HaveRoom => m_room;

    [SerializeField] private Vector2Int m_difference;
    [SerializeField] private Vector2Int m_origin;

    public Vector2Int Origin { get => m_origin - m_difference; }

    private bool m_isDoorGenerate;


    //MapClass Data---------------
    [SerializeField] private MapClassData m_mapClassData;

    private Vector2Int m_startPos;
    private Vector2Int m_endPos;

    //UI reference-----------------
    [SerializeField] private GraphicRaycaster m_roomPieceCanvas;



    //use error check or limit ----
    [SerializeField] private int m_enemyPieceMax;
    private int m_enemyPieceCount;
    [SerializeField] private int m_shopPieceMax;
    private int m_shopPieceCount;
    [SerializeField] private int m_trapPieceMax;
    private int m_trapPieceCount;

    //error all connect roomcheck
    private HashSet<int> m_allRoomID;



    //component-----------
    private MapPlaceErrorMessage m_errorMessageClass;
    private BoardManager m_boardManager;

    //Debug---------------
    [Header("Debug")]
    //if ture, make MapClass by myself(scene)
    [SerializeField] private bool m_debugCheck;

    private MapClass m_mapClass = new(0, 0);
    [SerializeField] private Vector2Int m_size;

    [SerializeField] private Vector2Int m_DebugStartPos;
    [SerializeField] private Vector2Int m_DebugEndPos;

    [Header("Shop Reset")]
    //[SerializeField] private EventBusAsset 
    [SerializeField] private List<InstanceCounter> m_instanceCounterList;

    [SerializeField] private IntRunTime m_level;

    private void Awake()
    {
        m_errorMessageClass = GetComponent<MapPlaceErrorMessage>();
        m_boardManager = GetComponent<BoardManager>();

        if(m_debugCheck)
        {
            InitializeMapGrid();

            m_startPos = m_DebugStartPos;
            m_endPos = m_DebugEndPos;
        }
        else
        {
            m_mapClass = m_mapClassData.MapClass;

            m_startPos = m_mapClassData.StartPos;
            m_endPos = m_mapClassData.GoalPos;
        }
        m_boardManager.Generate(m_mapClass, m_startPos, m_endPos);
    }

    private void InitializeMapGrid()
    {
        m_mapClass = new MapClass(m_size.x, m_size.y);
    }

    //mause select gridObj
    private GridObject m_gridObj;

    private PointerEventData m_pointerData;


    private void Update()
    {
        MousePos();

        Vector2Int mousePos = m_origin;
        Vector2Int origin = m_origin - m_difference;


        #region �}�E�X����
        if (m_action.action.WasPressedThisFrame())
        {
            m_gridObj = null;
            
            //this is GridObject-----------------
            Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
            var ray = Camera.main.ScreenPointToRay(mouseScreenPos);
            if (Physics.Raycast(ray, out var hit))
            {
                m_gridObj = hit.collider.gameObject.GetComponent<GridObject>();
            }



            //this is UI Canvas-------------------
            m_pointerData = new PointerEventData(EventSystem.current);
            m_pointerData.position = Mouse.current.position.ReadValue();

            List<RaycastResult> results = new();

            m_roomPieceCanvas.Raycast(m_pointerData, results);
            //------------------------------------


            if (m_roomPieceParentObj != null)
            {
                var roomPieceParent = m_roomPieceParentObj.GetComponent<RoomPieceParent>();

                if (mousePos.x < m_size.x && mousePos.x >= 0&&
                    mousePos.y < m_size.y && mousePos.y >= 0
                )
                {
                    
                    //grid���Œu�����Ƃ��@origin �ɍ��킹�Ēu���@roompiece = null
                    if (!m_mapClass.IsRoomColliding(m_room, origin))
                    {
                        //========= Scene Processing =========
                        Debug.Log(m_mapClass.GetFloor(2, 2).State != Floor.FloorState.empty);

                        //if can not get GameScene gridObj, return;
                        if (m_gridObj == null) return;

                        switch (roomPieceParent.AreaType)
                        {
                            case AreaType.None:
                                break;
                            case AreaType.Summon:
                                if (m_enemyPieceMax <= m_enemyPieceCount)
                                {
                                    RoomCountLimitError();
                                    return;
                                }

                                m_enemyPieceCount++;

                                break;
                            case AreaType.Shop:
                                if(m_shopPieceMax <= m_shopPieceCount)
                                {
                                    RoomCountLimitError();

                                    return;
                                }
                                m_shopPieceCount++;

                                break;
                            case AreaType.Damage:
                                if(m_trapPieceMax <= m_trapPieceCount)
                                {
                                    RoomCountLimitError();

                                    return;
                                }
                                m_trapPieceCount++;

                                break;
                        }
                        m_gridObj.OnPlaceFloor(
                            m_room,
                            roomPieceParent.AreaType, 
                            origin, 
                            roomPieceParent
                        );

                        //======================================

                       
                        PlaceRoom(roomPieceParent.AreaType);

                    }
                    else
                    {
                        //errormessage
                        Debug.Log("Error");
                    }
                }
                else
                {
                    //grid�O�ł��鎞�@���̏�ɒu��
                    //Debug.Log("NotFind Map");
                    bool isHitCanvas = results.Count > 1;

                    if(!isHitCanvas)
                    {
                        roomPieceParent.CallResetTransform();

                    }

                    m_roomPieceParentObj = null;
                    m_room = null;
                }
            }
            //if do not have roompiece to mouse
            else
            {
                if (m_gridObj)
                {
                    if (!m_gridObj.IsPlace) return;

                    m_difference = m_gridObj.PieceIndex;
                    RoomPieceParent roomPieceParent = m_gridObj.OnRemoveFloor(mousePos);

                    if (roomPieceParent == null) return;

                    m_roomPieceParentObj = roomPieceParent.gameObject;
                    m_roomPieceParentRect = roomPieceParent.Rect;
                    m_room = roomPieceParent.Room;

                    switch (roomPieceParent.AreaType)
                    {
                        case AreaType.None:
                            break;
                        case AreaType.Summon:
                            m_enemyPieceCount--;

                            break;
                        case AreaType.Shop:
                            m_shopPieceCount--;

                            break;
                        case AreaType.Damage:
                            m_trapPieceCount--;

                            break;
                    }

                    Debug.Log("Call remove");

                    RemoveRoom();
                }
                else
                {
                    foreach (var result in results)
                    {
                        RoomPiece roomPieceObj = result.gameObject.GetComponent<RoomPiece>();

                        if (roomPieceObj == null) continue;

                        m_difference = roomPieceObj.Index;

                        RoomPieceParent roomPieceParent = roomPieceObj.Parent;

                        m_roomPieceParentObj = roomPieceParent.gameObject;
                        m_roomPieceParentRect = roomPieceParent.Rect;
                        m_room = roomPieceParent.Room;


                        roomPieceParent.SetRecodePos();

                        break;
                    }
                }
            }
        }
        #endregion

        //if have roomPieceParentObject, following mousePoint
        if (m_roomPieceParentObj != null)
        {
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

            //piece index 0, 1, 2...  Shift piece index difference
            Vector2 differencePos = new Vector2(
                            (m_difference.x) * 15f + 0.5f,
                            (m_difference.y) * 15f + 0.5f
                            );
            m_roomPieceParentRect.position = mouseScreenPos - differencePos;
        }
    }

    private void RoomCountLimitError()
    {
        //errorcheck is not connect to start form end
        Debug.Log("error: not connect to start from end");
        m_errorMessageClass.ShowErrorMessage(MapPlaceErrorMessageType.CountOver);
    }

    private void MousePos()
    {
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();

        Vector3 worldPos = m_mainCamera.ScreenToWorldPoint(mouseScreenPos);
        worldPos.y = 1;
        m_mouseWorldPos = worldPos;

        Vector3 localPos = m_parent.InverseTransformPoint(worldPos);

        int x = Mathf.FloorToInt(localPos.x / 1.5f);
        int z = Mathf.FloorToInt(localPos.z / 1.5f);

        m_origin = new Vector2Int(x, z);
    }

    private void PlaceRoom(AreaType type)
    {
        m_mapClass.PlaceRoom(m_room, m_origin - m_difference);


        //========= RoomData ============
        List<Vector2Int> roomSizes = new();
        Vector2Int roomPos = new Vector2Int(m_origin.x, m_origin.y);

        int ID = m_mapClass.GetFloorID(roomPos.x, roomPos.y);

        Vector2Int origin = new Vector2Int(m_origin.x - m_difference.x, m_origin.y - m_difference.y);

        for (int y = 0; y < m_room.Size.y; y++)
        {
            for (int x = 0; x < m_room.Size.x; x++)
            {
                if (m_room.GetFloor(x, y).State == Floor.FloorState.empty) continue;
                Vector2Int pos = origin + new Vector2Int(x, y);

                roomSizes.Add(pos);
            }
        }
        
        RoomData data = new RoomData(ID, type, roomSizes);

        m_roomData.Add(data);

        m_roomPieceParentObj = null;
        m_room = null;

        //===============================


        if (CallDFS(m_startPos, m_endPos))
        {
            m_isDoorGenerate = true;
        }
        else
        {
            m_isDoorGenerate = false;
        }
    }

    private void RemoveRoom()
    {
        Vector2Int origin = new Vector2Int(m_origin.x, m_origin.y);

        var id = m_mapClass.GetFloorID(origin.x, origin.y);
        m_mapClass.RemoveRoom(id);

        var room = m_roomData.FirstOrDefault(x => x.m_ID == id);
        m_roomData.Remove(room);

        //m_roomDataID make mapClassID the same
        foreach(var data in m_roomData)
        {
            if(data.m_ID > id)
            {
                data.SetID(data.m_ID - 1);

            }
        }
    }


    //Process
    //       2
    //       |
    // 0 --- 1--- 4
    //       |
    //       3
    //when place room, make a list(graph) of NeighborRooms numbers
    //0| 1
    //1| 2,3,4
    //2| 1
    //3| 1
    //4| 1
    //run BFS
    //if connect start from goal, run DFS

    Dictionary<int, HashSet<int>> m_graph = new();

    Dictionary<EdgeKey, List<EdgeVariant>> m_connectionMap = new();


    struct EdgeKey
    {
        public int A;
        public int B;

        public EdgeKey(int a, int b)
        {
            if (a < b)
            {
                A = a;
                B = b;
            }
            else
            {
                A = b;
                B = a;
            }
        }
    }

    class EdgeVariant
    {
        public Vector2Int pos;
        public Wall.Side dir;
    }

    //look floor id, Check which room that ID is connected
    public void GetNeighborRooms()
    {
        m_allRoomID = new();

        m_graph.Clear();
        m_connectionMap.Clear();

        for (int y = 0; y < m_size.y; y++)
        {
            for (int x = 0; x < m_size.x; x++)
            {
                var id = m_mapClass.GetFloorID(x, y);

                if (id == -1) continue;

                //all place room id check
                m_allRoomID.Add(id);

                Vector2Int[] dirs =
                {
                    new Vector2Int(-1, 0),
                    new Vector2Int(0, -1),
                };

                foreach (var dir in dirs)
                {
                    Vector2Int neighbor = new Vector2Int(x, y) + dir;

                    if (neighbor.x < 0 || neighbor.y < 0 || neighbor.x >= m_size.x || neighbor.y >= m_size.y)
                        continue;

                    var neighborID = m_mapClass.GetFloorID(neighbor.x, neighbor.y);

                    if (neighborID == -1) continue;
                    if (neighborID == id) continue;

                    //---graph----
                    if (!m_graph.ContainsKey(id))
                    {
                        m_graph[id] = new HashSet<int>();
                    }

                    m_graph[id].Add(neighborID);

                    if (!m_graph.ContainsKey(neighborID))
                    {
                        m_graph[neighborID] = new HashSet<int>();
                    }

                    m_graph[neighborID].Add(id);

                    //---edge-----

                    EdgeKey key = new EdgeKey(id, neighborID);

                    EdgeVariant variant = new EdgeVariant();
                    variant.pos = new Vector2Int(x, y);
                    if (dir == new Vector2Int(-1, 0))
                    {
                        variant.dir = Wall.Side.West;
                    }
                    else
                    {
                        variant.dir = Wall.Side.South;
                    }


                    if (!m_connectionMap.ContainsKey(key))
                    {
                        Debug.Log(key);
                        m_connectionMap[key] = new List<EdgeVariant>();
                    }

                    m_connectionMap[key].Add(variant);
                }
            }
        }
    }

    //are the start and goal connected?
    public bool CallDFS(Vector2Int startPos, Vector2Int endPos)
    {
        GetNeighborRooms();

        //get startPos ID
        int startID = m_mapClass.GetFloorID(startPos.x, startPos.y);
        //get goalPos ID
        int endID = m_mapClass.GetFloorID(endPos.x, endPos.y);

        //Debug.Log($"{startID} {endID}");

        if (startID == -1) return false;
        if (endID == -1) return false;

        HashSet<int> visited = new HashSet<int>();

        visited.Add(startID);

        return (DFS(startID, endID, visited));
    }

    //DFS
    //if you can find goal, true
    public bool DFS(int current, int end, HashSet<int> visited)
    {
        if (current == end) return true;

        if (!m_graph.ContainsKey(current))
        {
            //Debug.Log("Null");
            return false;
        }

        //Debug.Log(current);


        foreach (var next in m_graph[current].OrderBy(x => x))
        {
            if (visited.Contains(next)) continue;

            visited.Add(next);
            if(DFS(next, end, visited)) return true;
             visited.Remove(next);

        }
        return false;
    }

    private List<int> m_bestPath;
    private List<List<int>> m_pathList;



    
    //Call by Button
    public void OnClickDFS()
    {

        if (!m_isDoorGenerate)
        {
            //errorcheck is not connect to start form end
            Debug.Log("error: not connect to start from end");
            m_errorMessageClass.ShowErrorMessage(MapPlaceErrorMessageType.NotRouteConnected);
            return;
        }

        foreach (var placeId in m_graph.Keys)
        {
            
        }

        //Get startId
        int startID = m_mapClass.GetFloorID(m_startPos.x, m_startPos.y);
        //Get endId
        int endID = m_mapClass.GetFloorID(m_endPos.x, m_endPos.y);

        List<int> visited = new List<int>();

        m_pathList = new();

        OnDFS(startID, endID, visited);

        if(!GenerateDoor())
        {
            //errorcheck is all conect piece?
            Debug.Log("error: not connect all piece");
            m_errorMessageClass.ShowErrorMessage(MapPlaceErrorMessageType.NotPieceConnected);
            return;
        }

        ////shopObject reset
        foreach (var counter in m_instanceCounterList)
        {
            counter.ResetCount();
        }

        SceneManager.LoadScene("CreatMap");
    }

    private void OnDFS(int current, int goal, List<int> visited)
    {
        visited.Add(current);

        if (current == goal)
        {
            m_pathList.Add(new List<int>(visited));
            return;
        }

        foreach (var next in m_graph[current].OrderBy(x => x))
        {
            if (visited.Contains(next)) continue;

            List<int> copy = new List<int>(visited);
            OnDFS(next, goal, copy);
        }
    }

    public bool GenerateDoor()
    {
        m_bestPath = new();

        //if more steps to the goal, overWrite
        for(int i = 0;i < m_pathList.Count;i++)
        {
            var count = m_pathList[i].Count;

            if(m_bestPath.Count < count)
            {
                m_bestPath = m_pathList[i];
            }
        }

        for(int i = 0; i < m_bestPath.Count - 1; i++)
        {
            Connect(m_bestPath[i], m_bestPath[i + 1]);
        }

        HashSet<int> mainPath = new HashSet<int>(m_bestPath);

        bool added;


        //prevent isolated rooms
        do
        {
            added = false;

            foreach (var id in m_graph.Keys)
            {
                if (mainPath.Contains(id)) continue;

                foreach (var neighborID in m_graph[id])
                {
                    if (mainPath.Contains(neighborID))
                    {
                        //connect
                        Connect(id, neighborID);

                        mainPath.Add(id);
                        added = true;
                        break;
                    }
                }
            }

        } while (added);

        //errorcheck is all conect piece?
        foreach (var placeId in m_allRoomID)
        {
            Debug.Log($"allRoomID{placeId}");
            if (mainPath.Contains(placeId)) continue;
            Debug.Log("check false");
            return false;
        }

        m_mapClassData.SetMapClass(m_mapClass);
        m_mapClassData.SetRoomDatas(m_roomData);
        Debug.Log("check true");

        return true;
    }

    //Connect rooms random
    private void Connect(int id, int next)
    {
        int from = id;
        int to = next;

        var key = new EdgeKey(from, to);
        //m_mapClass.DebugPrintFloors();
        if (!m_connectionMap.ContainsKey(key))
        {
            Debug.Log("Null");
            Debug.Log($"{key}{"is Null"}");
        }

        List<EdgeVariant> list = m_connectionMap[key];

        var edge = list[Random.Range(0, list.Count)];

        var floor = m_mapClass.GetWall(edge.pos.x, edge.pos.y, edge.dir);

        floor.SetState(Wall.WallState.door);
    }
}
