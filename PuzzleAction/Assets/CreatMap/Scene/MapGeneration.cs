using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    [Header("========== Map ==========")]
    private MapClass m_mapClass = new MapClass(0, 0);
    private Vector2Int m_size;

    private readonly List<GameObject> m_floorObjects = new();
    private readonly List<GameObject> m_wallObjectsSouth = new();
    private readonly List<GameObject> m_wallObjectsWest = new();
    [SerializeField] private GameObject m_floorPrefab;
    [SerializeField] private GameObject m_wallPrefab;
    [Space(10)]

    [Header("========== NavMesh ==========")]
    [SerializeField] private Transform m_navMeshPlane;
    [SerializeField] private NavMeshSurface m_navMeshSurface;
    [Space(10)]

    [Header("========== Scale ==========")]
    [SerializeField] private Vector3 m_floorScale = Vector3.one;
    [SerializeField] private Vector3 m_wallScale = Vector3.one;

    public List<GameObject> SouthWall => m_wallObjectsSouth;
    public List<GameObject> WestWall => m_wallObjectsWest;

    public Vector3 FloorScale => m_floorScale;
    public Vector3 WallScale => m_wallScale;

    public void Generate(MapClassData data)
    {
        m_mapClass = data.MapClass;
        m_size = m_mapClass.Size;

        InitializeMap();
        UpdateObjects();
        SetupNavMeshPlane();
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

        //Debug.Log($"{baseFloorSize}");
        Vector3 floorSize = new Vector3(
            baseFloorSize.x * m_floorScale.x,
            baseFloorSize.y * m_floorScale.z,
            baseFloorSize.z * m_floorScale.z);
        //get FloorPrefab size
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

        // set origin 
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
                Vector3 floorPosition = new(origin.x + x * floorSize.x, 0, origin.y + y * floorSize.z);
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
                        floorPosition + new Vector3(0, wallBounds.extents.y * m_wallScale.y, floorSize.z * 0.5f),
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
                        floorPosition + new Vector3(floorSize.x * 0.5f, wallBounds.extents.y * m_wallScale.y, 0),
                        Quaternion.Euler(0, -90, 0)
                    );
                    eWall.transform.localScale = wallSize;
                    eWall.name = $"({x + 1},{y},W)";
                }
            }
        }

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

    private void ApplyWallState(GameObject wall, Wall.WallState wallState)
    {
        bool isVisible = wallState == Wall.WallState.full;

       switch(wallState)
        {
            case Wall.WallState.full:
                {
                    wall.SetActive(isVisible);
                    break;
                }

            case Wall.WallState.empty:
                {
                    wall.SetActive(isVisible); 
                    break;
                }

            case Wall.WallState.door:
                {
                    wall.SetActive(isVisible);

                    var renderer = wall.GetComponent<Renderer>();
                    if (renderer != null) renderer.material.color = UnityEngine.Color.red;
                    break;
                }
        }
    }

    private void SetupNavMeshPlane()
    {
        if (m_navMeshPlane == null) return;

        Renderer floorRenderer = m_floorPrefab.GetComponent<Renderer>();

        Vector3 floorSize = new Vector3(
            floorRenderer.bounds.size.x * m_floorScale.x,
            floorRenderer.bounds.size.y * m_floorScale.y,
            floorRenderer.bounds.size.z * m_floorScale.z
        );

        float mapWidth = m_size.x * floorSize.x;

        float mapHeight = m_size.y * floorSize.z;


        Vector3 center = new Vector3(
            -floorSize.x * 0.5f + mapWidth * 0.5f,
            0f,
            -floorSize.z * 0.5f + mapHeight * 0.5f
        );

        m_navMeshPlane.position = center;

        // Unity Plane ‚Í 10x10
        m_navMeshPlane.localScale = new Vector3(mapWidth / 10f, 1f, mapHeight / 10f);

        if (m_navMeshSurface != null)
        {
            m_navMeshSurface.BuildNavMesh();
        }
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        Transform origin = m_floorObjects[0].transform; //x, y(0, 0)

        var floorRenderer = m_floorPrefab.GetComponent<Renderer>();
        Vector3 baseSize = floorRenderer.bounds.size; // 2x

        Vector3 floorSize = new Vector3(
            baseSize.x = m_floorScale.x,
            baseSize.y = m_floorScale.y,
            baseSize.z = m_floorScale.z);

        return new Vector3(
            origin.position.x + gridPos.x * m_floorScale.x,
            0.5f,
            origin.position.z + gridPos.y * m_floorScale.z);
    }
}