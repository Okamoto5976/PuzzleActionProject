using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapPlaceSystem : MonoBehaviour
{
    [SerializeField] private InputActionReference m_action;

    [SerializeField] private MapPlaceBuild m_build;

    [SerializeField] private Transform m_parent;//mousePos parent
    [SerializeField] private Camera m_mainCamera;
    private Vector3 m_mouseWorldPos;

    private GameObject m_roompiece;

    [Header("MapClass")]
    private MapClass m_mapClass = new(0, 0);
    [SerializeField] private Vector2Int m_size;

    [SerializeField] private int m_createRoomNumber;
    public Queue<Room> m_rooms = new Queue<Room>();

    private Room m_room;

    [SerializeField] private Vector2Int m_origin;

    [SerializeField] private Vector2Int m_startPos;
    [SerializeField] private Vector2Int m_endPos;


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

        m_room = m_rooms.Dequeue();//本当はシーンでクリックによって取得
        Debug.Log(m_room.Size);
    }

    #region ルーム作成
    private Room CreateRoom()
    {
        int num = UnityEngine.Random.Range(0, 5);

        Room room = new(new(), new(0,0));

        if (num == 0)
        {
            room = new(
                new()
                {
                    Floor.FloorState.full,Floor.FloorState.full,Floor.FloorState.full,
                    Floor.FloorState.full,Floor.FloorState.path,Floor.FloorState.full,
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
                    Floor.FloorState.full, Floor.FloorState.path,Floor.FloorState.full,
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
                    Floor.FloorState.path,Floor.FloorState.empty,Floor.FloorState.path,
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
                    Floor.FloorState.path,Floor.FloorState.full,
                }, new(2, 2)
                );
        }
        else if( num == 4)
        {
            room = new(
               new()
               {
                    Floor.FloorState.full,Floor.FloorState.empty,
                    Floor.FloorState.path,Floor.FloorState.full,
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


        if (m_action.action.WasPressedThisFrame())
        {
            if(m_roompiece != null)
            {
                if (m_origin.x < m_size.x && m_origin.x >= 0&&
                    m_origin.y < m_size.y && m_origin.y >= 0
                )
                {
                    var obj = m_roompiece.GetComponent<RoomObj>();
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
                        m_roompiece.transform.position = worldPos;
                        m_roompiece = null;

                        PlaceRoom();
                    }
                    else
                    {
                        //grid内でおけないとき　roompieceを　保存していた場所に返す
                        //room piece = null
                        //roompiece return 
                        Debug.Log("No Place");
                        m_roompiece.transform.position = obj.OriginalPos;
                        m_roompiece = null;

                    }
                }
                else
                {
                    //grid外である時　その場に置く　（roompiece = null)
                    Debug.Log("NotFind Map");
                    m_roompiece = null;
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
                        m_roompiece = parent.gameObject;
                        //remove

                    }
                    else
                    {
                        //roomがありIsPlaceがfalseだったら取得
                        //取得の際 現在のparentの位置を保存
                        parent.SetOriginalPos();
                        m_roompiece = parent.gameObject;

                    }

                }
            }

        }

        //もしroompieceがあるならmouseに追従
        if(m_roompiece != null)
        {
            m_roompiece.transform.position = m_mouseWorldPos;
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
        m_roompiece = null;

        //roompiece place in scene grid

        if (m_rooms.Count > 0)
        {
            m_room = m_rooms.Dequeue();

            Debug.Log(m_room.Size);

        }

        //if (BFS(m_startPos, m_endPos))
        //{
        //    Debug.Log("IsGoal OK");
        //}
        //else
        //{
        //    Debug.Log("IsGoal No");
        //}
    }

    Dictionary<int,List<int>> m_roomsNumbers = new Dictionary<int,List<int>>();

    //public bool BFS(Vector2Int startPos, Vector2Int endPos)
    //{
    //    //startPosのid取得
    //    int startID = GetId(startPos);
    //    //endPosのid取得
    //    int endID = GetId(endPos);

    //    bool IsRouteCheck = false;

    //    Queue<int> queue = new Queue<int>();
    //    HashSet<int> visited = new HashSet<int>();

    //    queue.Enqueue(startID);
    //    visited.Add(startID);
    //    //最初の部屋のIDと部屋番号0を入れる

    //    while (queue.Count > 0)
    //    {
    //        int currentID = queue.Dequeue();

    //        //goalまでつながっても
    //        //door生成の番号つけるため繰り返す
    //        foreach(var neighborID in GetNeighborRooms(currentID,visited))
    //        {
    //            if(neighborID == endID)
    //            {
    //                IsRouteCheck = true;
    //            }

    //            queue.Enqueue(neighborID);
    //            visited.Add(neighborID);
    //        }
    //    }

    //    if(IsRouteCheck) return true;

    //    return false;
    //}

    //private int GetId(Vector2Int pos)
    //{
    //    int index = pos.x + pos.y * (m_size.x + 1);

    //    return m_mapClass.Floors[index].Id;
    //}

    //private int m_roomNumber = 0;//初期

    //private HashSet<int> GetNeighborRooms(int id, HashSet<int> visited)
    //{
    //    HashSet<int> neighborIDs = new HashSet<int>();
    //    m_roomNumber++;

    //    for (int y = 0; y < m_size.y; y++)
    //    {
    //        for (int x = 0; x < m_size.x; x++)
    //        {
    //            int index = x + y * (m_size.x + 1);
    //            var floor = m_mapClass.Floors[index];

    //            //同じidの部屋のfloorを見つける
    //            if (floor.Id != id) continue;

    //            // 4方向チェック
    //            Vector2Int[] dirs = {
    //            new Vector2Int(1,0),
    //            new Vector2Int(-1,0),
    //            new Vector2Int(0,1),
    //            new Vector2Int(0,-1)
    //            };

    //            foreach (var dir in dirs)
    //            {
    //                Vector2Int neighbor = new Vector2Int(x, y) + dir;

    //                //範囲外
    //                if (neighbor.x < 0 || neighbor.y < 0 || neighbor.x > m_size.x || neighbor.y > m_size.y)
    //                    continue;

    //                int floorIndex = neighbor.x + neighbor.y * (m_size.x + 1);
    //                //floorがemptyならcontinu
    //                if (m_mapClass.Floors[floorIndex].Id == -1) continue;
    //                //同じidの部屋ならcontinu
    //                if (m_mapClass.Floors[floorIndex].Id == id) continue;
    //                //前回通ったidの部屋ならcontinu
    //                if (visited.Contains(m_mapClass.Floors[floorIndex].Id)) continue;

    //                neighborIDs.Add(m_mapClass.Floors[floorIndex].Id);

    //                //indexとflootIndexをセットで覚えておきたい（idでもいい）追記　ここのindexだとpathはうまくいかない
    //                //idを覚えよう
    //                //indexならpathのindexを覚えよう
    //                //知らないidの時は その部屋のpathのfloorIndexとindex保存
    //                //Dic id , List<Vector2Int index)
    //            }
    //        }
    //    }
    //    return neighborIDs;
    //}


}
