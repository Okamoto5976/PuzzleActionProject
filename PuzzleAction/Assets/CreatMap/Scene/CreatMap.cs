using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
public class CreatMap : MonoBehaviour
{
    [Header("Map")]
    private MapClass m_mapClass = new MapClass(0, 0);
    [SerializeField] private Vector2Int m_size;
    [SerializeField] private GameObject m_floorPrefab;
    [SerializeField] private GameObject m_wallPrefab;

    private List<GameObject> m_floorObjects = new();
    private List<GameObject> m_wallObjectsSouth = new();
    private List<GameObject> m_wallObjectsWest = new();

    [SerializeField] private MapClassData m_mapClassData;

    [Header("Scale Setthings")]
    [SerializeField] private Vector3 m_floorScale = Vector3.one;
    [SerializeField] private Vector3 m_wallScale = Vector3.one;

    [Header("TreasureBox")]
    [SerializeField] private GameObject m_treasurePrefab;
    [SerializeField] private int m_treasureCount = 3;
    [SerializeField, Range(0, 1)] private float m_mimicRate = 0.2f;

    [Header("Debug")]
    [SerializeField] private GameObject m_goalPrefab;
    [SerializeField] private GameObject m_shopPrefab;

    [SerializeField] private GameObject m_player;
    [SerializeField] private T_Camera m_camera;
    private GameObject m_playerController;


    //--------------External API---------------
    public List<GameObject> SouthWall => m_wallObjectsSouth;
    public List<GameObject> WestWall => m_wallObjectsWest;

    private void Awake()
    {
        m_mapClass = m_mapClassData.MapClass;
        Debug.Log(m_mapClass.Size.ToString());
        m_size = m_mapClass.Size;

        InitializeMap();

        UpdateObjects(); //GeneratMap

        ProcessAreaTypes();//apply AreaType

        GenerateTreasures();

        SpawnPlayer();
    }
    /// <summary>
    /// MapClass Convert 3D
    /// </summary>
    private void UpdateObjects()
    {
        for (int y = 0; y < m_mapClass.Size.y; y++)
        {
            for (int x = 0; x < m_mapClass.Size.x; x++)
            {
                //FLOOR
                var mapFloorIndex = x + y * m_mapClass.Size.x;
                var floorState = m_mapClass.GetFloor(x, y).State;
                m_floorObjects[mapFloorIndex].
                    SetActive(floorState != Floor.FloorState.empty
                           && floorState != Floor.FloorState.blocked);

                //SOUTH WALL
                var southWall = m_wallObjectsSouth[x + y * m_mapClass.Size.x];
                ApplyWallState(southWall, m_mapClass.GetWall(x, y, Wall.Side.South).State);

                //WEST WALL
                var westWall = m_wallObjectsWest[x + y * (m_mapClass.Size.x + 1)];
                ApplyWallState(westWall, m_mapClass.GetWall(x, y, Wall.Side.West).State);

                //NORTH EDGE
                if (y == m_mapClass.Size.y - 1)
                {
                    ApplyWallState(m_wallObjectsSouth[x + (y + 1) * m_mapClass.Size.x],
                                   m_mapClass.GetWall(x, y + 1, Wall.Side.South).State);
                }
                //EAST EDGE
                if (x == m_mapClass.Size.x - 1)
                {
                    ApplyWallState(m_wallObjectsWest[(x + 1) + y * (m_mapClass.Size.x + 1)],
                                   m_mapClass.GetWall(x + 1, y, Wall.Side.West).State);
                }
            }
        }
        //m_mapClass.DebugPrintFloors();
    }
    /// <summary>
    /// Generate all 3D appearances based on the size of the MapClass
    /// </summary>
    private void InitializeMap()
    {
        // new map class
        Debug.Log(m_mapClass.Floors.Count);
        var floorCount = m_size.x * m_size.y;

        // get prefab bounds
        var floorBounds = m_floorPrefab.GetComponent<Renderer>().bounds;
        var wallBounds = m_wallPrefab.GetComponent<Renderer>().bounds;

        //get FloorPrefab size
        var floorRenderer = m_floorPrefab.GetComponent<Renderer>();
        Vector3 baseFloorSize = floorRenderer.bounds.size;
        baseFloorSize.x = 1;
        baseFloorSize.y = 1;
        baseFloorSize.z = 1;

        //Debug.Log($"{baseFloorSize} ÅöÅöÅöÅöÅöÅöÅöÅöÅöÅöÅö");
        Vector3 floorSize = new Vector3(
            baseFloorSize.x * m_floorScale.x,
            baseFloorSize.y * m_floorScale.z,
            baseFloorSize.z * m_floorScale.z);
        //get FloorPrefab sizeÅö
        var wallRenderer = m_wallPrefab.GetComponent<Renderer>();
        Vector3 baseWallSize = wallRenderer.bounds.size;
        Vector3 wallSize = new Vector3(
            baseWallSize.x * m_wallScale.x,
            baseWallSize.y * m_wallScale.y,
            baseWallSize.z * m_wallScale.z);

        // create floor parent
        var floorParent = new GameObject();
        floorParent.transform.parent = transform;
        floorParent.name = "Floors";

        // create wall parent
        var wallParent = new GameObject();
        wallParent.transform.parent = transform;
        wallParent.name = "Walls";

        for (int i = 0; i < m_size.x * m_size.y; i++)
        {
            var obj = Instantiate(m_floorPrefab, floorParent.transform);
            m_floorObjects.Add(obj);
        }

        for (int i = 0; i < (m_size.x) * (m_size.y + 1); i++)
        {
            var s = Instantiate(m_wallPrefab, wallParent.transform);
            m_wallObjectsSouth.Add(s);
        }

        for (int i = 0; i < (m_size.x + 1) * (m_size.y); i++)
        {
            var w = Instantiate(m_wallPrefab, wallParent.transform);
            m_wallObjectsWest.Add(w);
        }

        // set origin Åö
        //Vector2 origin = -floorBounds.extents;
        Vector2 origin = new Vector2(
            -floorSize.x * 0.5f,
            -floorSize.z * 0.5f);

        // create floor map
        for (int y = 0; y < m_size.y; y++)
        {
            for (int x = 0; x < m_size.x; x++)
            {
                string name = $"({x},{y}";

                // create floor
                var floor = m_floorObjects[x + y * m_size.x];
                Vector3 floorPosition = new(origin.x + x * floorSize.x, 0, origin.y + y * floorSize.z);Å@//Åö
                floor.transform.position = floorPosition;
                floor.transform.localScale = floorSize;
                floor.name = name + ")";


                // create southern wall
                var sWall = m_wallObjectsSouth[x + y * m_size.x];
                sWall.transform.SetPositionAndRotation(
                    floorPosition + new Vector3(0, wallBounds.extents.y * m_wallScale.y, -floorSize.z * 0.5f),
                    Quaternion.Euler(0, 180, 0)
                );
                sWall.transform.localScale = wallSize;
                sWall.name = name + ",S)";

                // create western wall
                var wWall = m_wallObjectsWest[x + y * (m_size.x + 1)];
                wWall.transform.SetPositionAndRotation(
                    floorPosition + new Vector3(-floorSize.x * 0.5f, wallBounds.extents.y * m_wallScale.y, 0),
                    Quaternion.Euler(0, -90, 0)
                );
                wWall.transform.localScale = wallSize;
                wWall.name = name + ",W)";

                // create extra southern wall if edge floor
                if (y == m_size.y - 1)
                {
                    var nWall = m_wallObjectsSouth[x + y * m_size.x + m_size.x];
                    nWall.transform.SetPositionAndRotation(
                        floorPosition + new Vector3(0, wallBounds.extents.y, floorBounds.extents.z + 0.5f),
                        Quaternion.Euler(0, 180, 0)
                    );
                    nWall.transform.localScale = wallSize;
                    nWall.name = $"({x},{y + 1},S)";
                }

                // create extra western wall if edge floor
                if (x == m_size.x - 1)
                {
                    var eWall = m_wallObjectsWest[x + y * (m_size.x + 1) + 1];
                    eWall.transform.SetPositionAndRotation(
                        floorPosition + new Vector3(floorBounds.extents.x + 0.5f, wallBounds.extents.y, 0),
                        Quaternion.Euler(0, -90, 0)
                    );
                    eWall.transform.localScale = wallSize;
                    eWall.name = $"({x + 1},{y},W)";
                }
            }
        }

    }

    /// <summary>
    /// full == SetActive(true)
    /// empty == SetActive(false)
    /// door == SetActive(false or true)
    /// </summary>
    /// <param name="wallObj"></param>
    /// <param name="state"></param>
    private void ApplyWallState(GameObject wall, Wall.WallState wallState)
    {
        bool isVisible = wallState == Wall.WallState.full;

        switch (wallState)
        {
            case Wall.WallState.full:
                wall.SetActive(isVisible);
                break;

            case Wall.WallState.empty:
                wall.SetActive(isVisible);
                break;

            case Wall.WallState.door:
                wall.SetActive(isVisible);

                var renderer = wall.GetComponent<Renderer>();
                if (renderer != null) renderer.material.color = UnityEngine.Color.red;
                break;
        }
    }

    /// <summary>
    /// AreaType èàóùñ{ëÃ
    /// </summary>
    /// 
    [SerializeField] private Poolinstallationpulling m_enemySpawner;

    private void ProcessAreaTypes()
    {
        if (m_mapClassData == null || m_mapClassData.roomDatas == null)
        {
            Debug.LogWarning("T_Gene : RoomData is nothing");
            return;
        }

        Vector3 startPos = GridToWorld(GetStartPos());
        Vector3 goalPos = GridToWorld(GetGoalPos());

        GameObject obj2 = Instantiate(m_goalPrefab, goalPos, Quaternion.identity);

        var sortedRoomDatas = new List<RoomData>(m_mapClassData.roomDatas);

        foreach (var room in sortedRoomDatas)
        {
            switch (room.m_type)
            {
                case AreaType.None:

                    break; //nothing


                case AreaType.Summon:
                    {
                        var poses = RandomChoosePosition(room, 3);
                        List<Vector3> worldposition = new();
                        foreach (var position in poses)
                        {
                            if (IsForbiddenPos(position)) continue;

                            Vector3 worldPos = GridToWorld(position);
                            worldposition.Add(worldPos);
                        }
                        if(m_enemySpawner!= null)
                        {
                            m_enemySpawner.SpawnEnemiesAtPositions(worldposition);
                        }
                        else
                        {
                            Debug.LogError("CreatMap: NO Poolinstallationpulling");
                        }
                            break;
                    }


                case AreaType.Shop:
                    {
                        var poses = RandomChoosePosition(room, 1);
                        foreach (var position in poses)
                        {
                            if (IsForbiddenPos(position)) continue;

                            Vector3 debugPos = GridToWorld(position);
                            GameObject obj = Instantiate(m_shopPrefab, debugPos, Quaternion.identity);
                        }
                        break;

                    }


                case AreaType.Damage:

                    break;
            }
        }
    }

    private Vector2Int GetStartPos()
    {
        return m_mapClassData.StartPos;
    }

    private Vector2Int GetGoalPos()
    {
        return m_mapClassData.GoalPos;
    }

    private Transform GetWorldOrigin()
    {
        return m_floorObjects[0].transform;
    }

    private bool IsForbiddenPos(Vector2Int pos)
    {
        var floorState = m_mapClass.GetFloor(pos.x, pos.y).State;
        if (pos == GetStartPos()) return true;
        if (pos == GetGoalPos()) return true;
        //if(floorState == Floor.FloorState.blocked)
        //                          return true;
        return false;
    }

    private void GenerateTreasures()
    {
        //Potential treasure chest spawn locations
        List<Vector2Int> candidates = new();

        foreach (var room in m_mapClassData.roomDatas)
        {
            //reject other than None
            if (room.m_type != AreaType.None) continue;
            foreach (var pos in room.m_roomSizes)
            {
                if (IsForbiddenPos(pos)) continue;
                candidates.Add(pos);

            }

        }

        //return smallest value
        int spawnCount = Mathf.Min(m_treasureCount, candidates.Count);

        for (int i = 0; i < spawnCount; i++)
        {
            //random select || Max roomSize
            int index = Random.Range(0, candidates.Count);

            Vector2Int gridPos = candidates[index];

            //delete index candidates
            candidates.RemoveAt(index);

            Vector3 worldPos = GridToWorld(gridPos);

            GameObject obj = Instantiate(m_treasurePrefab, worldPos, Quaternion.identity, transform);

            bool isMimic = Random.value < m_mimicRate;

            Treasure treasure = obj.GetComponent<Treasure>();
            treasure.SetIsMimic(isMimic);
        }
    }


    /// <summary>
    /// random obtain the pos in the room
    /// </summary>
    /// <param name="room"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    private List<Vector2Int> RandomChoosePosition(RoomData room, int count)
    {
        List<Vector2Int> copy = new(room.m_roomSizes);
        List<Vector2Int> poses = new();
        for (int i = 0; i < count; i++)
        {
            int number = UnityEngine.Random.Range(0, copy.Count);
            Vector2Int pos = copy[number];
            poses.Add(pos);
            copy.Remove(pos);
        }
        return poses;
    }

    /// <summary>
    /// Convert GridPos to WorldPos
    /// </summary>
    /// <param name="gridPos"></param>
    /// <returns></returns>
    private Vector3 GridToWorld(Vector2Int gridPos)
    {
        Transform origin = GetWorldOrigin(); //0, 0

        var floorRenderer = m_floorPrefab.GetComponent<Renderer>();
        Vector3 baseSize = floorRenderer.bounds.size; //2x

        Vector3 floorSize = new Vector3(
            baseSize.x * m_floorScale.x,
            baseSize.y * m_floorScale.y,
            baseSize.z * m_floorScale.z
            );


        return new Vector3(
            origin.position.x + gridPos.x * floorSize.x,
            0.5f,
            origin.position.z + gridPos.y * floorSize.z);
    }

    private void SpawnPlayer()
    {
        Vector3 spawnPos = GridToWorld(GetStartPos());
        spawnPos.y = 0.5f;

        m_playerController = Instantiate(m_player, spawnPos, Quaternion.identity);
        m_playerController.name = "Player";

        if (m_camera != null)
        {
            m_camera.SetTarget(m_playerController.transform);
        }
    }

}
