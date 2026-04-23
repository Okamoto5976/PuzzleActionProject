using System;
using System.Collections.Generic;
using UnityEngine;
using static Wall;

public class T_Gene : MonoBehaviour
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

    private void Awake()
    {
        m_mapClass = m_mapClassData.MapClass;
        Debug.Log(m_mapClass.Size.ToString());
        m_size = m_mapClass.Size;

        InitializeMap();

        UpdateObjects();
    }

    private void UpdateObjects()
    {
        for (int y = 0; y < m_mapClass.Size.y; y++)
        {
            for (int x = 0; x < m_mapClass.Size.x; x++)
            {
                //FLOOR
                var mapFloorIndex = x + y * m_mapClass.Size.x;
                m_floorObjects[mapFloorIndex].
                    SetActive(m_mapClass.GetFloor(x, y).State != Floor.FloorState.empty);

                //SOUTH WALL
                var southWall = m_wallObjectsSouth[x + y * m_mapClass.Size.x];
                ApplyWallState(southWall, m_mapClass.GetWall(x, y, Wall.Side.South).State);

                //WEST WALL
                var westWall = m_wallObjectsWest[x + y * (m_mapClass.Size.x + 1)];
                ApplyWallState(westWall, m_mapClass.GetWall(x, y, Wall.Side.West).State);

                //NORTH EDGE
                if(y == m_mapClass.Size.y - 1)
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
                //Debug.Log("!");

                //wallObjectsSouth[x + y * m_mapClass.Size.x].SetActive(m_mapClass.GetWall(x, y, Wall.Side.South).State != Wall.WallState.empty);
                //wallObjectsWest[x + y * (m_mapClass.Size.x + 1)].SetActive(m_mapClass.GetWall(x, y, Wall.Side.West).State != Wall.WallState.empty);
                //
                //if (y == m_mapClass.Size.y - 1)
                //{
                //    wallObjectsSouth[x + (y + 1) * m_mapClass.Size.x].SetActive(m_mapClass.GetWall(x, y + 1, Wall.Side.South).State != Wall.WallState.empty);
                //}
                //
                //if (x == m_mapClass.Size.x - 1)
                //{
                //    wallObjectsWest[(x + 1) + y * (m_mapClass.Size.x + 1)].SetActive(m_mapClass.GetWall(x + 1, y, Wall.Side.West).State != Wall.WallState.empty);
                //}
            }
        }
        m_mapClass.DebugPrintFloors();
    }

    private void InitializeMap()
    {
        // new map class
        //m_mapClass = new MapClass(size.x, size.y);
        Debug.Log(m_mapClass.Floors.Count);
        var floorCount = m_size.x * m_size.y;

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
        Vector2 origin = -floorBounds.extents;

        // create floor map
        for (int y = 0; y < m_size.y; y++)
        {
            for (int x = 0; x < m_size.x; x++)
            {
                string name = $"({x},{y}";

                // create floor
                var floor = m_floorObjects[x + y * m_size.x];
                Vector3 floorPosition = new(origin.x + x, 0, origin.y + y);
                floor.transform.position = floorPosition;
                floor.name = name + ")";


                // create southern wall
                var sWall = m_wallObjectsSouth[x + y * m_size.x];
                sWall.transform.SetPositionAndRotation(
                    floorPosition + new Vector3(0, wallBounds.extents.y, -floorBounds.extents.z),
                    Quaternion.Euler(0, 180, 0)
                );
                sWall.name = name + ",S)";

                // create western wall
                var wWall = m_wallObjectsWest[x + y * (m_size.x + 1)];
                wWall.transform.SetPositionAndRotation(
                    floorPosition + new Vector3(-floorBounds.extents.x, wallBounds.extents.y, 0),
                    Quaternion.Euler(0, -90, 0)
                );
                wWall.name = name + ",W)";

                // create extra southern wall if edge floor
                if (y == m_size.y - 1)
                {
                    var nWall = m_wallObjectsSouth[x + y * m_size.x + m_size.x];
                    nWall.transform.SetPositionAndRotation(
                        floorPosition + new Vector3(0, wallBounds.extents.y, floorBounds.extents.z),
                        Quaternion.Euler(0, 180, 0)
                    );
                    nWall.name = $"({x},{y + 1},S)";
                }

                // create extra western wall if edge floor
                if (x == m_size.x - 1)
                {
                    var eWall = m_wallObjectsWest[x + y * (m_size.x + 1) + 1];
                    eWall.transform.SetPositionAndRotation(
                        floorPosition + new Vector3(floorBounds.extents.x, wallBounds.extents.y, 0),
                        Quaternion.Euler(0, -90, 0)
                    );
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
    private void ApplyWallState(GameObject wall, Wall.WallState state)
    {

        bool isVisible = state == Wall.WallState.full;

        switch (state)
        {
            case Wall.WallState.full:
                wall.SetActive(isVisible);
                break;

            case Wall.WallState.empty:
                wall.SetActive(isVisible);
                break;

            case Wall.WallState.door:
                wall.SetActive(!isVisible);

                //Ç±Ç±Ç…doorÇæÇ¡ÇΩèÍçáÇÃèàóùí«â¡
                var renderer = wall.GetComponent<Renderer>();
                if (renderer != null) renderer.material.color = Color.red;
                break;
        }
    }

}
