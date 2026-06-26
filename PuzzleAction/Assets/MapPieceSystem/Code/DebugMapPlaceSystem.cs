using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugMapPlaceSystem : MonoBehaviour
{
    private MapClass m_mapClass;
    [SerializeField] private Vector2Int m_size;

    //error all connect roomcheck
    private HashSet<int> m_allRoomID;

    //use GenerateMap MainSece, PlaceRoomDatas
    private List<RoomData> m_roomData = new();

    //you have room naw,
    private Room m_room;

    //MapClass Data---------------
    [SerializeField] private MapClassData m_mapClassData;

    private Vector2Int m_startPos = new Vector2Int(0, 0);
    private Vector2Int m_endPos = new Vector2Int(5, 5);

    [Header("Shop Reset")]
    //[SerializeField] private EventBusAsset 
    [SerializeField] private List<InstanceCounter> m_instanceCounterList;

    [System.Serializable]
    struct DebugRoom
    {
        public AreaType m_type;
        public Vector2Int m_originPos;
    }

    [SerializeField] private List<DebugRoom> m_rooms;


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

    public bool DebugMapGenerate()
    {
        m_mapClass = new MapClass(m_size.x, m_size.y);

        //placeroom‚ðŒJ‚è•Ô‚·
        for(int i = 0; i < m_rooms.Count; i++)
        {
            m_room = CreateRoom();
            PlaceRoom(m_rooms[i].m_type, m_rooms[i].m_originPos);
        }

        //I‚í‚Á‚½‚çCallDFS true‚È‚çi
        if(!CallDFS(m_startPos, m_endPos))
        {
            Debug.Log("error: DebugMapSystem not connect start from end");
            return false;
        }

        if (!OnCheakMapClassData()) return false;

        return true;
    }

    private Room CreateRoom()
    {
        Room room = new(new(), new(0, 0));

        room = new(
            new()
            {
                Floor.FloorState.full,Floor.FloorState.full,Floor.FloorState.full,
                Floor.FloorState.full,Floor.FloorState.full,Floor.FloorState.full,
                Floor.FloorState.full,Floor.FloorState.full,Floor.FloorState.full,
            }, new(3, 3)
            );

        return room;
    }

    private void PlaceRoom(AreaType type, Vector2Int originPos)
    {
        m_mapClass.PlaceRoom(m_room, originPos);


        //========= RoomData ============
        List<Vector2Int> roomSizes = new();
        Vector2Int roomPos = new Vector2Int(originPos.x, originPos.y);

        int ID = m_mapClass.GetFloorID(roomPos.x, roomPos.y);

        for (int y = 0; y < m_room.Size.y; y++)
        {
            for (int x = 0; x < m_room.Size.x; x++)
            {
                if (m_room.GetFloor(x, y).State == Floor.FloorState.empty) continue;
                Vector2Int pos = originPos + new Vector2Int(x, y);

                roomSizes.Add(pos);
            }
        }

        RoomData data = new RoomData(ID, type, roomSizes);

        m_roomData.Add(data);
        //===============================

        m_room = null;

    }

    //are the start and goal connected?
    private bool CallDFS(Vector2Int startPos, Vector2Int endPos)
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

    //look floor id, Check which room that ID is connected
    private void GetNeighborRooms()
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

    //DFS
    //if you can find goal, true
    private bool DFS(int current, int end, HashSet<int> visited)
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
            if (DFS(next, end, visited)) return true;
            visited.Remove(next);

        }
        return false;
    }

    private List<int> m_bestPath;
    private List<List<int>> m_pathList;

    private bool OnCheakMapClassData()
    {
        //Get startId
        int startID = m_mapClass.GetFloorID(m_startPos.x, m_startPos.y);

        Debug.Log("kok");
        //Get endId
        int endID = m_mapClass.GetFloorID(m_endPos.x, m_endPos.y);

        List<int> visited = new List<int>();

        m_pathList = new();

        OnDFS(startID, endID, visited);

        if (!GenerateDoor())
        {
            //errorcheck is all conect piece?
            Debug.Log("error: DebugMapPlaceSystem not connect all piece");
            return false;
        }

        ////shopObject reset
        foreach (var counter in m_instanceCounterList)
        {
            counter.ResetCount();
        }

        return true;
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
        for (int i = 0; i < m_pathList.Count; i++)
        {
            var count = m_pathList[i].Count;

            if (m_bestPath.Count < count)
            {
                m_bestPath = m_pathList[i];
            }
        }

        for (int i = 0; i < m_bestPath.Count - 1; i++)
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
            //Debug.Log($"allRoomID{placeId}");
            if (mainPath.Contains(placeId)) continue;
            return false;
        }

        m_mapClassData.SetMapClass(m_mapClass);
        m_mapClassData.SetRoomDatas(m_roomData);
        m_mapClassData.SetStartPos(m_startPos);
        m_mapClassData.SetGoalPos(m_endPos);

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
