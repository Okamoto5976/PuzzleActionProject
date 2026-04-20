using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    private ClassMap classMap = new ClassMap(0, 0);
    [SerializeField] private Vector2Int size;
    [SerializeField] private GameObject m_floorPrefab;
    [SerializeField] private GameObject m_wallPrefab;

    private List<GameObject> floorObjects = new();
    private List<GameObject> wallObjectsSouth = new();
    private List<GameObject> wallObjectsWest = new();


    private void Awake()
    {
        InitializeMap();

        Room room1 = new(
            new()
            {
                Floor.FloorState.full,Floor.FloorState.full,Floor.FloorState.full,
                Floor.FloorState.full,Floor.FloorState.empty,Floor.FloorState.full,
                Floor.FloorState.full,Floor.FloorState.empty,Floor.FloorState.full,
            }, new(3, 3)
            );
        classMap.PlaceRoom(room1, new(0, 0));

        Room room2 = new(
            new()
            {
                Floor.FloorState.empty,Floor.FloorState.full,Floor.FloorState.empty,
                Floor.FloorState.full,Floor.FloorState.full,Floor.FloorState.full,
                Floor.FloorState.empty,Floor.FloorState.full,Floor.FloorState.empty,
            }, new(3, 3)
            );
        classMap.PlaceRoom(room2, new(2, 2));

        UpdateObjects();
    }

    private void UpdateObjects()
    {
        for (int y = 0; y < classMap.Size.y; y++)
        {
            for (int x = 0; x < classMap.Size.x; x++)
            {
                var mapFloorIndex = x + y * classMap.Size.x;
                floorObjects[mapFloorIndex]
                    .SetActive(classMap.GetFloor(x, y).State != Floor.FloorState.empty);
                wallObjectsSouth[x + y * classMap.Size.x]
                    .SetActive(classMap.GetWall(x, y, Wall.Side.South).State != Wall.WallState.empty);
                wallObjectsWest[x + y * (classMap.Size.x + 1)]
                    .SetActive(classMap.GetWall(x, y, Wall.Side.West).State != Wall.WallState.empty);

                if (y == classMap.Size.y - 1)
                {
                    wallObjectsSouth[x + (y + 1) * classMap.Size.x].SetActive(classMap.GetWall(x, y + 1, Wall.Side.South).State != Wall.WallState.empty);
                }

                if (x == classMap.Size.x - 1)
                {
                    wallObjectsWest[(x + 1) + y * (classMap.Size.x + 1)].SetActive(classMap.GetWall(x + 1, y, Wall.Side.West).State != Wall.WallState.empty);
                }
            }
        }

        //classMap.DebugPrintFloors();
        classMap.DebugPrintWalls();
    }

    private void InitializeMap()
    {
        // new map class
        classMap = new ClassMap(size.x, size.y);
        Debug.Log(classMap.Floors.Count);
        var floorCount = size.x * size.y;

        // get prefab bounds
        var floorBounds = m_floorPrefab.GetComponent<Renderer>().bounds;
        var wallBounds = m_wallPrefab.GetComponent<Renderer>().bounds;

        // create floor parent
        var floorParent = new GameObject();
        floorParent.transform.parent = transform;
        floorParent.name = "Floors";

        // create wall parent
        var wallParent = new GameObject();
        wallParent.transform.parent = transform;
        wallParent.name = "Walls";

        for (int i = 0; i < size.x * size.y; i++)
        {
            var obj = Instantiate(m_floorPrefab, floorParent.transform);
            floorObjects.Add(obj);
        }

        for (int i = 0; i < (size.x) * (size.y + 1); i++)
        {
            var s = Instantiate(m_wallPrefab, wallParent.transform);
            wallObjectsSouth.Add(s);
        }

        for (int i = 0; i < (size.x + 1) * (size.y); i++)
        {
            var w = Instantiate(m_wallPrefab, wallParent.transform);
            wallObjectsWest.Add(w);
        }

        // set origin
        Vector2 origin = -floorBounds.extents;

        // create floor map
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                string name = $"({x},{y}";

                // create floor
                var floor = floorObjects[x + y * size.x];
                Vector3 floorPosition = new(origin.x + x, 0, origin.y + y);
                floor.transform.position = floorPosition;
                floor.name = name + ")";


                // create southern wall
                var sWall = wallObjectsSouth[x + y * size.x];
                sWall.transform.SetPositionAndRotation(
                    floorPosition + new Vector3(0, wallBounds.extents.y, -floorBounds.extents.z),
                    Quaternion.Euler(0, 180, 0)
                );
                sWall.name = name + ",S)";

                // create western wall
                var wWall = wallObjectsWest[x + y * (size.x + 1)];
                wWall.transform.SetPositionAndRotation(
                    floorPosition + new Vector3(-floorBounds.extents.x, wallBounds.extents.y, 0),
                    Quaternion.Euler(0, -90, 0)
                );
                wWall.name = name + ",W)";

                // create extra southern wall if edge floor
                if (y == size.y - 1)
                {
                    var nWall = wallObjectsSouth[x + y * size.x + size.x];
                    nWall.transform.SetPositionAndRotation(
                        floorPosition + new Vector3(0, wallBounds.extents.y, floorBounds.extents.z),
                        Quaternion.Euler(0, 180, 0)
                    );
                    nWall.name = $"({x},{y + 1},S)";
                }

                // create extra western wall if edge floor
                if (x == size.x - 1)
                {
                    var eWall = wallObjectsWest[x + y * (size.x + 1) + 1];
                    eWall.transform.SetPositionAndRotation(
                        floorPosition + new Vector3(floorBounds.extents.x, wallBounds.extents.y, 0),
                        Quaternion.Euler(0, -90, 0)
                    );
                    eWall.name = $"({x + 1},{y},W)";
                }
            }
        }
    }
}