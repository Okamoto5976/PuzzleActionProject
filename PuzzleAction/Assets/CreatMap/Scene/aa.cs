using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
public class CreatMap2 : MonoBehaviour
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

    [Header("Debug用")]
    [SerializeField] private GameObject m_enemyPrefab;
    [SerializeField] private GameObject m_goalPrefab;
    [SerializeField] private GameObject m_shopPrefab;
    [SerializeField] private GameObject m_player;
    [SerializeField] private T_Camera m_camera;

    private GameObject m_playerController;

    private void Awake()
    {
        m_mapClass = m_mapClassData.MapClass;
        Debug.Log(m_mapClass.Size.ToString());
        m_size = m_mapClass.Size;

        InitializeMap(); //受け取った情報元に初期化

        UpdateObjects(); //マップの生成

        ProcessAreaTypes();

        SpawnPlayer();
    }

    /// <summary>
    /// MapClassの状態を3Dに反映
    /// </summary>

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
    /// MapClassのサイズに基づいて3Dの見た目をすべて生成
    /// </summary>
    private void InitializeMap()
    {
        // new map class
        Debug.Log(m_mapClass.Floors.Count);
        var floorCount = m_size.x * m_size.y;

        // get prefab bounds
        var floorBounds = m_floorPrefab.GetComponent<Renderer>().bounds;
        var wallBounds = m_wallPrefab.GetComponent<Renderer>().bounds;

        //get FloorPrefab size★
        var floorRenderer = m_floorPrefab.GetComponent<Renderer>();
        Vector3 baseFloorSize = floorRenderer.bounds.size;
        baseFloorSize.x = 1;
        baseFloorSize.y = 1;
        baseFloorSize.z = 1;

        //Debug.Log($"{baseFloorSize} ★★★★★★★★★★★");
        Vector3 floorSize = new Vector3(
            baseFloorSize.x * m_floorScale.x,
            baseFloorSize.y * m_floorScale.z,
            baseFloorSize.z * m_floorScale.z);

        //get FloorPrefab size★
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

        // set origin ★

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
                Vector3 floorPosition = new(origin.x + x * floorSize.x, 0, origin.y + y * floorSize.z);　//★
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

                wall.SetActive(isVisible);
                var renderer = wall.GetComponent<Renderer>();
                if (renderer != null) renderer.material.color = UnityEngine.Color.red;

                break;

        }

    }

    /// <summary>
    /// AreaType 処理本体
    /// </summary>
    private void ProcessAreaTypes()
    {
        if (m_mapClassData == null || m_mapClassData.roomDatas == null)
        {
            Debug.LogWarning("T_Gene : RoomData が存在しない");
            return;
        }

        //ID順に処理
        var sortedRoomDatas = new List<RoomData>(m_mapClassData.roomDatas);
       Vector3 goalPos = GridToWorld(m_mapClassData.GoalPos);
        GameObject obj2 = Instantiate(m_goalPrefab, goalPos, Quaternion.identity);
        foreach (var room in sortedRoomDatas)
        {
            switch (room.m_type)
            {
                case AreaType.None:

                    break; //何もしない

                case AreaType.Enemy:
                    {
                        var poses = RandomChoosePosition(room, 3); //マップとそのマスにオブジェクトを何個生成させるか指定

                        foreach (var position in poses)
                        {
                            Vector3 debugPos = GridToWorld(position); //Vector2Int を World座標変換
                            //CallAreaSetを呼ぶ
                            GameObject obj = Instantiate(m_enemyPrefab, debugPos, Quaternion.identity);
                        }
                        break;
                    }

                case AreaType.Shop:
                    {
                        var poses = RandomChoosePosition(room, 1);
                        foreach (var position in poses)
                        {
                            Vector3 debugPos = GridToWorld(position);
                            GameObject obj = Instantiate(m_shopPrefab, debugPos, Quaternion.identity);
                        }
                        break;
                    }
            }
        }
    }

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
            Debug.Log(pos + "★★★★★★★");
        }
        return poses;
    }

    private Vector3 GridToWorld(Vector2Int gridPos)
    {
        Transform floor00 = GetFloor00(); //0,0 の床の座標を取得
        var floorRenderer = m_floorPrefab.GetComponent<Renderer>();
        Vector3 baseSize = floorRenderer.bounds.size; //二倍
        Vector3 floorSize = new Vector3(
            baseSize.x * m_floorScale.x,
            baseSize.y * m_floorScale.y,
            baseSize.z * m_floorScale.z
            );

        return new Vector3(
            floor00.position.x + gridPos.x * floorSize.x,
            floor00.position.y + floorSize.y + 1,
            floor00.position.z + gridPos.y * floorSize.z);
    }

    /// <summary>
    /// ここから下はデバッグ用でPlayer関係の処理追加
    /// </summary>

    private void SpawnPlayer()
    {
        if (m_player == null)
        {
            Debug.LogError("T_Gene : Playerが設定されていません");
            return;
        }

        Transform floor00 = GetFloor00();
        if (floor00 == null) return;
        Vector3 spawnPos = floor00.position + Vector3.up * 0.5f;
        m_playerController = Instantiate(m_player, spawnPos, Quaternion.identity);
        m_playerController.name = "Player";
        //var controller = m_playerController.GetComponent<PlayerController>();
        //if (controller != null)
        //{
        //    controller.Initialize(this);
        //}

        //カメラにPlayerを追従させる

        if (m_camera != null)
        {
            m_camera.SetTarget(m_playerController.transform);
        }
        else
        {
            Debug.LogWarning("T_Gene : T_Cameraが設定されていません");
        }
        Debug.Log("Player spawned at (0,0)");
    }

    private Transform GetFloor00()
    {
        // (0,0) は最初に配置した Floor
        // m_floorObjects は x + y*width で詰めているので index=0
        if (m_floorObjects.Count == 0)
        {
            Debug.LogError("No floor objects found");
            return null;
        }
        return m_floorObjects[0].transform;
    }
}

