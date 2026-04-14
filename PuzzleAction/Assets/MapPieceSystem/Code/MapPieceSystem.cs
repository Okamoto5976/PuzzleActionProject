using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum AreaType
{
    None,
    Enemy
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class RoomShape
{
    public List<Vector2Int> cells;

    public List<DoorData> doors;
}

public class DoorData
{
    public Vector2Int localPos;
    public Direction dir;
}

public class Rooma
{
    public int id;
    public AreaType type;

    public List<Vector2Int> cells;

    public Vector2Int origin;
    public List<DoorData> doors;
}

public class Cell
{
    public bool up;
    public bool down;
    public bool left;
    public bool right;

    public int roomId;

    //public bool goal;
}

//スタート設置
//ゴール設置
//ゴール付きのRoomShapeを先につくり　m_gridに先に設置するか
//RoomShapeの数増加


public class MapPieceSystem : MonoBehaviour
{
    [SerializeField] private Vector2Int m_origin;

    [SerializeField] private Transform m_parent;
    [SerializeField] private Camera m_mainCamera;


    [SerializeField] private MapPieceBuild m_build;

    [SerializeField] private InputAction m_attack;

    private void OnEnable()
    {
        m_attack.Enable();
    }

    private void OnDisable()
    {
        m_attack.Disable();
    }

    private RoomShape m_roomPlan;
    private Rooma m_room;

    private List<Rooma> m_rooms = new List<Rooma>();

    private int m_nextId = 0;

    private GameObject m_roomObj;
    private bool m_isDrag;

    private int m_gridWidth = 10;
    private int m_gridHeight = 10;

    private Vector2Int m_startPos;
    private Vector2Int m_endPos;

    Cell[,] m_grid;//二次元配列はx行y列の配列を用意　その中に型を入れれる
    bool[,] m_isValid;

    private void ReceiveMapData(int width, int height)
    {
        m_gridWidth = width;
        m_gridHeight = height;
    }


    private void Start()
    {
        GridMap(m_gridWidth, m_gridHeight);
        //GridValidMap(m_gridWidth, m_gridHeight);

        m_startPos = new Vector2Int(0, 0);
        m_endPos = new Vector2Int(8, 8);

        //生成
        m_build.Generate(m_gridWidth, m_gridHeight);
    }

    private void GridMap(int width, int height)//引数で受け取る
    {
        m_grid = new Cell[width, height];
    }

    private void GridValidMap(int width, int height)//床が有効かどうか
    {
        m_isValid = new bool[width, height];
    }

    private void Update()
    {
        MousePos();
  
        //if(m_attack.IsPressed())
        //{
        //    if(m_isDrag)
        //    {

        //    }

        //    if (m_roomObj == null) return;

        //    Vector3 localPos = m_parent.InverseTransformPoint(m_roomObj.transform.position);

        //    int x = Mathf.FloorToInt(localPos.x / 1.5f);
        //    int z = Mathf.FloorToInt(localPos.z / 1.5f);

        //    Vector2Int roomObj = new Vector2Int(x, z);

        //    if(roomObj == m_origin)
        //    {
        //        m_isDrag = true;
        //    }
        //}

        //if(m_isDrag)
        //{
            
        //}

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (m_roomPlan == null)
            {
                m_roomPlan = CreatePlan();

                m_room = CreateBuild(m_roomPlan);//生成する際にTypeを入れたい

                m_roomPlan = null;

                m_roomObj = m_build.InstantiatePieceObject(m_room);//シーンに実際に作る
    
                Debug.Log("you create room");
            }
            else
            {
                Debug.Log("you already have room");
            }

        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (m_room == null) return;

            var origin = m_origin;//一応

            if (CanRoomPlace(m_room, origin))
            {
                Debug.Log("On Place");
                m_room.origin = origin;

                m_rooms.Add(m_room);

                RoomPlace(m_room, origin);
                m_room = null;

                //Cellの壁のチェック＆ドア設置
                CellConnections();
                RoomDoorPlace(m_rooms);
            }
            else
            {
                Debug.Log("No Place");
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

    private RoomShape CreatePlan()//設計図を設計
    {
        var shape = new RoomShape();

        shape.cells = new List<Vector2Int>
        {
            new Vector2Int(-1, -1),
            new Vector2Int(-1,  0),
            new Vector2Int(-1,  1),
            new Vector2Int(0, -1),
            new Vector2Int(0,  0),
            new Vector2Int(0,  1),
            new Vector2Int(1, -1),
            new Vector2Int(1,  0),
            new Vector2Int(1,  1)
        };

        shape.doors = new List<DoorData>
        {
           new DoorData
           {
               localPos = new Vector2Int(0, -1),
               dir = Direction.Up
           },
           new DoorData
           {
               localPos = new Vector2Int(-1, 0),
               dir = Direction.Left
           },
           new DoorData
           {
               localPos = new Vector2Int(0, 1),
               dir = Direction.Down
           },
           new DoorData
           {
               localPos = new Vector2Int(1, 0),
               dir = Direction.Right
           }
        };

        return shape;
    }

    private Rooma CreateBuild(RoomShape shape)//設計図を実体に
    {
        var room = new Rooma();

        room.cells = shape.cells;

        room.id = m_nextId;
        m_nextId++;
        room.type = AreaType.None;
        room.doors = shape.doors;

        return room;
    }

    private bool CanRoomPlace(Rooma room, Vector2Int origin)//置けるかどうか
    {
        foreach (var cell in room.cells)
        {
            Vector2Int pos = origin + cell;

            if (pos.x < 0 || pos.x >= m_gridWidth) return false;
            if (pos.y < 0 || pos.y >= m_gridHeight) return false;

            //gridがIsValidがfalse かどうか

            if (m_grid[pos.x, pos.y] != null) return false;
        }

        return true;
    }

    private void RoomPlace(Rooma room, Vector2Int origin)
    {
        foreach (var cell in room.cells)
        {
            Vector2Int pos = origin + cell;

            Cell piece = new Cell();

            m_grid[pos.x, pos.y] = piece;

            m_grid[pos.x, pos.y].roomId = room.id;
        }
    }

    private void RoomDoorPlace(List<Rooma> rooms)
    {
        foreach (var room in rooms)
        {
            foreach (var door in room.doors)
            {
                int x = room.origin.x + door.localPos.x;
                int z = room.origin.y + door.localPos.y;

                ApplyDoor(x, z, door.dir);
            }
        }
    }

    private void ApplyDoor(int x, int z, Direction dir)
    {
        switch(dir)
        {
            case Direction.Up:
                m_grid[x, z].up = true;
                if (m_grid[x, z + 1] != null)
                {
                    m_grid[x, z + 1].down = true;

                }
                break;
        }
    }

    private void CellConnections()
    {
        for(int x = 0; x < m_gridWidth; x++)
        {
            for(int z = 0; z < m_gridHeight; z++)
            {
                var cell = m_grid[x, z];
                if(cell == null) continue;

                int id = cell.roomId;

                //上
                cell.up = IsSameRoom(x, z + 1, id);

                cell.down = IsSameRoom(x, z - 1, id);

                cell.right = IsSameRoom(x + 1, z, id);

                cell.left = IsSameRoom(x - 1, z, id);
            }
        }
    }

    private bool IsSameRoom(int x, int z, int myId)
    {
        if (x < 0 || x >= m_gridWidth || z < 0 || z >= m_gridHeight)
            return false;

        var other = m_grid[x, z];
        if(other == null) return false;

        return other.roomId == myId;
    }

    private Queue<Vector2Int> m_queue;
    private bool[,] m_isVisited;

    private bool IsConnectStage(Vector2Int startPos, Vector2Int endPos)
    {
        m_queue = null;
        m_isVisited =null;

        m_queue = new Queue<Vector2Int>();
        m_isVisited = new bool[m_gridWidth, m_gridHeight];

        m_queue.Enqueue(startPos);
        m_isVisited[startPos.x, startPos.y] = true;

        while (m_queue.Count > 0)
        {
            var pos = m_queue.Dequeue();

            if (pos == endPos) return true;

            var cell = m_grid[pos.x, pos.y];
            if (cell == null) continue;

            if(cell.up)
            {
                IsCheckRoute(pos.x, pos.y + 1);
            }
            else if (cell.down)
            {
                IsCheckRoute(pos.x, pos.y - 1);
            }
            else if(cell.left)
            {
                 IsCheckRoute(pos.x - 1, pos.y);
            }
            else
            {
                IsCheckRoute(pos.x  + 1, pos.y);
            }
        }

        
        return false;
    }

    private void IsCheckRoute(int x, int z)
    {
        if (m_isVisited[x, z]) return;

        if (m_grid[x, z] == null) return;

        m_queue.Enqueue(new Vector2Int(x,z));
        m_isVisited[x, z] = true;
    }

    //上と右だけ
                //if(IsWall(x + 1, z, id))
                //{
                //    //壁
                //}

    //private bool IsWall(int x, int z, int myId)
    //{
    //    //範囲外
    //    if(x < 0 || x >= m_gridWidth || z < 0 || z >= m_gridHeight)
    //        return true;

    //    var other = m_grid[x, z];

    //    //空
    //    if (other == null) return true;

    //    //別の部屋
    //    if (other.roomId != myId) return true;

    //    return false;
    //}

    //private void CreateWall(int x, int z, Direction dir)
    //{
    //    //向きに応じて回転
    //}
}
