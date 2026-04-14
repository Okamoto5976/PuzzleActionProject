using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapPlaceSystem : MonoBehaviour
{
    [SerializeField] private InputActionReference m_action;

    [SerializeField] private MapPieceBuild m_build;
    [SerializeField] private Transform m_parent;

    [SerializeField] private Camera m_mainCamera;

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
            if(!m_mapClass.IsRoomColliding(m_room, m_origin))
            {
                //もしroomがあったらRemoveRoom
                m_mapClass.PlaceRoom(m_room, m_origin);

                if (m_rooms.Count > 0)
                {
                    m_room = m_rooms.Dequeue();

                    Debug.Log(m_room.Size);

                }
            }
        }
    }

    private void MousePos()
    {
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();

        Vector3 worldPos = m_mainCamera.ScreenToWorldPoint(mouseScreenPos);

        Vector3 localPos = m_parent.InverseTransformPoint(worldPos);

        int x = Mathf.FloorToInt(localPos.x / 1.5f);
        int z = Mathf.FloorToInt(localPos.z / 1.5f);

        m_origin = new Vector2Int(x, z);
    }

    Dictionary<int,List<int>> m_roomsNumbers = new Dictionary<int,List<int>>();

    public void BFS(Vector2Int startPos, Vector2Int endPos)
    {
        //startPosのid取得
        int startID = GetId(startPos);
        //endPosのid取得
        int endID = GetId(endPos);

        Queue<int> queue = new Queue<int>();
        HashSet<int> visited = new HashSet<int>();

        queue.Enqueue(startID);
        visited.Add(startID);
        //最初の部屋のIDと部屋番号0を入れる

        while (queue.Count > 0)
        {
            int currentID = queue.Dequeue();


        }
    }

    private int GetId(Vector2Int pos)
    {
        int index = pos.x + pos.y * (m_size.x + 1);

        return m_mapClass.Floors[index].Id;
    }

    private int m_roomNumber = 0;//初期

    private HashSet<int> GetNeighborRooms(int id)
    {
        HashSet<int> neighborIDs = new HashSet<int>();
        m_roomNumber++;

        for (int y = 0; y < m_size.y; y++)
        {
            for (int x = 0; x < m_size.x; x++)
            {
                int index = x + y * (m_size.x + 1);
                var floor = m_mapClass.Floors[index];

                //同じidの部屋のfloorを見つける
                if (floor.Id != id) continue;

                // 4方向チェック
                Vector2Int[] dirs = {
                new Vector2Int(1,0),
                new Vector2Int(-1,0),
                new Vector2Int(0,1),
                new Vector2Int(0,-1)
            };

                foreach (var dir in dirs)
                {
                    Vector2Int neighbor = new Vector2Int(x, y) + dir;

                    //範囲外
                    if (neighbor.x < 0 || neighbor.y < 0 || neighbor.x > m_size.x || neighbor.y > m_size.y)
                        continue;

                    int floorIndex = neighbor.x + neighbor.y * (m_size.x + 1);
                    //同じidの部屋ならreturn
                    if (m_mapClass.Floors[floorIndex].Id == id) continue;

                    //indexとflootIndexをセットで覚えておきたい（idでもいい）追記　ここのindexだとpathはうまくいかない
                    //idを覚えよう
                    //indexならpathのindexを覚えよう
                    //知らないidの時は その部屋のpathのfloorIndexとindex保存
                    //Dic id , List<Vector2Int index)
                }
            }
        }



        return neighborIDs;
    }
}
