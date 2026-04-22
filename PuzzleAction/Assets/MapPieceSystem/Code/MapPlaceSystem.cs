using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class MapPlaceSystem : MonoBehaviour
{
    [SerializeField] private InputActionReference m_action;

    [SerializeField] private MapPlaceBuild m_build;

    [SerializeField] private Transform m_parent;//mousePos parent
    [SerializeField] private Camera m_mainCamera;
    private Vector3 m_mouseWorldPos;

    private GameObject m_roomObj;

    [Header("MapClass")]
    private MapClass m_mapClass = new(0, 0);
    [SerializeField] private Vector2Int m_size;

    [SerializeField] private int m_createRoomNumber;
    public Queue<Room> m_rooms = new Queue<Room>();

    private Room m_room;

    [SerializeField] private Vector2Int m_origin;

    [SerializeField] private Vector2Int m_startPos;
    [SerializeField] private Vector2Int m_endPos;


    private bool m_isDoorGenerate;


    private void Awake()
    {
        InitializeMapGrid();
        m_build.Generate(m_size);

        for(int i = 0; i < m_createRoomNumber; i++)
        {
            Room room = CreateRoom();
            m_rooms.Enqueue(room);
            m_build.GenerateRoomObject(room);
        }

        //m_room = m_rooms.Dequeue();//本当はシーンでクリックによって取得
        //Debug.Log(m_room.Size);
    }

    #region ルーム作成
    private Room CreateRoom()
    {
        int num = UnityEngine.Random.Range(0, 3);

        Room room = new(new(), new(0,0));

        if (num == 0)
        {
            room = new(
                new()
                {
                    Floor.FloorState.full,Floor.FloorState.full,Floor.FloorState.full,
                    Floor.FloorState.full,Floor.FloorState.full,Floor.FloorState.full,
                    Floor.FloorState.full,Floor.FloorState.full,Floor.FloorState.full,
                },new(3,3)
                );
        }
        else if(num == 1)
        {
            room = new(
                new()
                {
                    Floor.FloorState.empty,Floor.FloorState.full,Floor.FloorState.empty,
                    Floor.FloorState.full, Floor.FloorState.full,Floor.FloorState.full,
                    Floor.FloorState.empty,Floor.FloorState.full,Floor.FloorState.empty,
                }, new(3, 3)
                );
        }
        else if(num == 2)
        {
            room = new(
                new()
                {
                    Floor.FloorState.full,Floor.FloorState.full ,Floor.FloorState.full,
                    Floor.FloorState.full,Floor.FloorState.full,Floor.FloorState.full,
                    Floor.FloorState.full,Floor.FloorState.full ,Floor.FloorState.full,
                }, new(3, 3)
                );
        }
        else if(num == 3)
        {
            room = new(
                new()
                {
                    Floor.FloorState.full,Floor.FloorState.full,
                    Floor.FloorState.full,Floor.FloorState.full,
                }, new(2, 2)
                );
        }
        else if( num == 4)
        {
            room = new(
               new()
               {
                    Floor.FloorState.full,Floor.FloorState.empty,
                    Floor.FloorState.full,Floor.FloorState.full,
               }, new(2, 2)
               );
        }

            return room;
    }
    #endregion

    private void InitializeMapGrid()
    {
        m_mapClass = new MapClass(m_size.x, m_size.y);
    }

    private void Update()
    {
        MousePos();

        #region マウス操作
        if (m_action.action.WasPressedThisFrame())
        {
            if(m_roomObj != null)
            {
                if (m_origin.x < m_size.x && m_origin.x >= 0&&
                    m_origin.y < m_size.y && m_origin.y >= 0
                )
                {
                    var obj = m_roomObj.GetComponent<RoomObj>();
                    //grid内で置けたとき　origin に合わせて置く　roompiece = null
                    if (!m_mapClass.IsRoomColliding(m_room, m_origin))
                    {
                        Debug.Log("On Place");
                        obj.SetIsPlace(true);


                        Vector3 localPos = new Vector3(
                            (m_origin.x) * 1.5f,
                            1,
                            (m_origin.y) * 1.5f
                            );

                        Vector3 worldPos = m_parent.TransformPoint(localPos);
                        m_roomObj.transform.position = worldPos;
                        m_roomObj = null;

                        PlaceRoom();
                    }
                    else
                    {
                        //grid内でおけないとき　roompieceを　保存していた場所に返す
                        //room piece = null
                        //roompiece return 
                        Debug.Log("No Place");
                        m_roomObj.transform.position = obj.OriginalPos;
                        m_roomObj = null;

                    }
                }
                else
                {
                    //grid外である時　その場に置く　（roompiece = null)
                    Debug.Log("NotFind Map");
                    m_roomObj = null;
                }
            }
            else
            {
                Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
                var ray = Camera.main.ScreenPointToRay(mouseScreenPos);
                if(Physics.Raycast(ray, out var hit))
                {
                    var obj = hit.collider.gameObject.GetComponent<RoomPieceObj>();
                    if (obj == null) return;

                    var parent = obj.GetComponentInParent<RoomObj>();

                    if (obj.IsPlace)
                    {
                        //roomがありIsPlaceがtrueだったらRemoveRoom
                        //取得　
                        //m_roompiece = obj.Parent;
                        parent.SetIsPlace(false);
                        m_roomObj = parent.gameObject;
                        m_room = parent.Room;
                        //remove
                        //マウスカーソルのfloorのID
                        var id = m_mapClass.GetFloorID(m_origin.x, m_origin.y);
                        m_mapClass.RemoveRoom(id);

                    }
                    else
                    {
                        //roomがありIsPlaceがfalseだったら取得
                        //取得の際 現在のparentの位置を保存
                        parent.SetOriginalPos();
                        m_roomObj = parent.gameObject;
                        m_room = parent.Room;

                    }

                }
            }

        }
        #endregion

        //もしroompieceがあるならmouseに追従
        if (m_roomObj != null)
        {
            m_roomObj.transform.position = m_mouseWorldPos;
        }
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

    private void PlaceRoom()
    {
        m_mapClass.PlaceRoom(m_room, m_origin);
        m_roomObj = null;
        m_room = null;

        //roompiece place in scene grid

        //if (m_rooms.Count > 0)
        //{
        //    m_room = m_rooms.Dequeue();

        //    Debug.Log(m_room.Size);

        //}

        if (CallDFS(m_startPos, m_endPos))
        {
            //Debug.Log("IsGoal OK");
            m_isDoorGenerate = true;
        }
        else
        {
            //Debug.Log("IsGoal No");
            m_isDoorGenerate = false;
        }
    }

    //------順------
    //部屋を置く　GetNeighborRoomsで隣接の数のリスト（グラフ）を作る
    //GetNeighborRoomsで隣接同士の値[02]の座標と方向を記録（後にdoorにステート変更）
    //隣接リストを見て　BFSを走らせる
    //startとgorlが繋がればDFSを走らせる
    //DFSの順にdoorを設置

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

    //floorのidを見て　そのidがどのidの部屋と隣接しているかみる
    public void GetNeighborRooms()
    {
        m_graph.Clear();
        m_connectionMap.Clear();

        for (int y = 0; y < m_size.y; y++)
        {
            for (int x = 0; x < m_size.x; x++)
            {
                var id = m_mapClass.GetFloorID(x, y);

                if (id == -1) continue;

                Vector2Int[] dirs =
                {
                    new Vector2Int(-1, 0),
                    new Vector2Int(0, -1),
                };

                foreach (var dir in dirs)
                {
                    Vector2Int neighbor = new Vector2Int(x, y) + dir;

                    //範囲外
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

    //スタートとゴールがつながっているか
    public bool CallDFS(Vector2Int startPos, Vector2Int endPos)
    {
        GetNeighborRooms();

        //startPosのid取得
        int startID = m_mapClass.GetFloorID(startPos.x, startPos.y);
        //endPosのid取得
        int endID = m_mapClass.GetFloorID(endPos.x, endPos.y);

        //Debug.Log($"{startID} {endID}");

        if (startID == -1) return false;
        if (endID == -1) return false;

        HashSet<int> visited = new HashSet<int>();

        visited.Add(startID);

        return (DFS(startID, endID, visited));
    }

    public bool DFS(int current, int end, HashSet<int> visited)
    {
        if (current == end) return true;

        if (!m_graph.ContainsKey(current))
        {
            //Debug.Log("Null");
            return false;
        }

        Debug.Log(current);


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

    //DFSを呼ぶ
    //ボタンで
    public void OnClickDFS()
    {
        if (!m_isDoorGenerate) return;

        //startPosのid取得
        int startID = m_mapClass.GetFloorID(m_startPos.x, m_startPos.y);
        //endPosのid取得
        int endID = m_mapClass.GetFloorID(m_endPos.x, m_endPos.y);

        List<int> visited = new List<int>();//訪れたところ

        m_pathList = new();

        OnDFS(startID, endID, visited);

        GenerateDoor();

        SceneManager.LoadScene("MainMapClassScene");
    }

    //CallDFSから呼ばれる
    //DFS（深さ優先探索）
    //startIDからendIDに到達するまで枝状に進む
    //endIDについたとき進んだ分を記録　前回の記録より多ければ上書き
    private void OnDFS(int current, int goal, List<int> visited)
    {
        visited.Add(current);

        

        if (current == goal)
        {
            m_pathList.Add(new List<int>(visited));
            return;
        }

        //m_graphの中　idを若順に取得
        //端まで到達したら端から削除していく
        foreach (var next in m_graph[current].OrderBy(x => x))
        {
            if (visited.Contains(next)) continue;

            List<int> copy = new List<int>(visited);
            OnDFS(next, goal, copy);
        }
    }


    [SerializeField] private MapClassData m_mapClassData;

    public void GenerateDoor()
    {
        m_bestPath = new();


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

        //trueなら続ける
        //必ず一回通る
        //bestPath(mainPath)に含まれるならつなげる
        //孤立を防ぐ
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

        m_mapClassData.SetValue(m_mapClass);
    }

    private void Connect(int id, int next)
    {
        int from = id;
        int to = next;

        var key = new EdgeKey(from, to);
        m_mapClass.DebugPrintFloors();
        if (!m_connectionMap.ContainsKey(key))
        {
            Debug.Log("Null");
            Debug.Log($"{key}{"is Null"}");
        }

        List<EdgeVariant> list = m_connectionMap[key];

        //keyが複数ある場合　ランダムに選ぶ
        var edge = list[Random.Range(0, list.Count)];

        var floor = m_mapClass.GetWall(edge.pos.x, edge.pos.y, edge.dir);

        floor.SetState(Wall.WallState.door);
    }

    //public bool OnBFS(Vector2Int startPos, Vector2Int endPos)
    //{
    //    //startPosのid取得
    //    int startID = m_mapClass.GetFloorID(startPos.x, startPos.y);
    //    //endPosのid取得
    //    int endID = m_mapClass.GetFloorID(endPos.x, endPos.y);

    //    Queue<int> queue = new Queue<int>();
    //    HashSet<int> visited = new HashSet<int>();

    //    queue.Enqueue(startID);
    //    visited.Add(startID);

    //    while (queue.Count > 0)
    //    {
    //        int currentID = queue.Dequeue();

    //        foreach (var next in m_graph[currentID])
    //        {
    //            if (next == endID)
    //            {
    //                return true;
    //            }

    //            if (visited.Contains(next)) continue;

    //            queue.Enqueue(next);
    //            visited.Add(next);
    //        }
    //    }

    //    return false;
    //}
}
