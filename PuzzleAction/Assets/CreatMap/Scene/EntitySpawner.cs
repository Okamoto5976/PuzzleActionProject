using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [Header("========== Player ==========")]
    [SerializeField] private Transform m_player;
    [SerializeField] private PlayerController m_playerC;
    [SerializeField] private T_Camera m_camera;
    [Space(10)]

    [Header("========== Enemy ==========")]
    [SerializeField] private Middleman_Enemy m_enemyPool;
    [Header("========== EnemyGacha ==========")]
    [SerializeField] private GachaEngine m_enemyGachaEngine;
    [SerializeField] private EnemyRarityTable m_enemyRarityTable;
    [Space(10)]

    [Header("========== Trap ==========")]
    [SerializeField] private Middleman_Trap m_trapPool;
    [Header("========== TrapGacha ==========")]
    [SerializeField] private GachaEngine m_trapGachaEngine;
    [SerializeField] private TrapRarityTable m_trapRarityTable;
    [SerializeField] private Entity m_trapOwner;   
    [Space(10)]

    [Header("========== Goal ==========")]
    [SerializeField] private GameObject m_goalPrefab;
    [SerializeField] private MainGameManager m_mainGameManager;
    [Space(10)]

    [Header("========== Shop ==========")]
    [SerializeField] private GameObject m_shopPrefab;
    [Space(10)]

    [Header("========== Treasure ==========")]
    [SerializeField] private GameObject m_treasureParent;
    [SerializeField] private GameObject m_treasurePrefab;
    [SerializeField] private GameObject m_mimicPrefab;

    [SerializeField] private int m_treasureCount = 3;
    [SerializeField, Range(0, 1)]
    private float m_mimicRate = 0.2f;

    private MapClassData m_mapClassData;
    private MapGeneration m_mapGeneration;

    private readonly Enum_TrapType[] m_areaTrapType =
    {
        Enum_TrapType.Gas,
        Enum_TrapType.Swamp,
        Enum_TrapType.Dynamite
    };

    public void Generate(MapClassData mapData, MapGeneration mapGeneration)
    {
        m_mapClassData = mapData;
        m_mapGeneration = mapGeneration;

        m_playerC.SetState();

        InitializeEnemyPools();
        InitializeTrapPool();

        SpawnGoal();

        ProcessAreaTypes();

        SpawnTreasures();

        SpawnPlayer();
    }
    private void InitializeEnemyPools()
    {
        if (m_enemyPool == null) return;
        foreach(Transform child in m_enemyPool.transform)
        {
            child.gameObject.SendMessage("Awake", SendMessageOptions.DontRequireReceiver);
        }
    }
    private void InitializeTrapPool()
    {
        if (m_trapPool == null) return;
        foreach(Transform child in m_trapPool.transform)
        {
            child.gameObject.SendMessage("Awake", SendMessageOptions.DontRequireReceiver);
        }
    }

    public Vector2Int GetStartPos()
    {
        return m_mapClassData.StartPos;
    }
    public Vector2Int GetGoalPos()
    {
        return m_mapClassData.GoalPos;
    }
    private void ProcessAreaTypes()
    {
        if(m_mapClassData == null || m_mapClassData.roomDatas == null)
        {
            Debug.LogWarning("EntitySpawner : RoomData is nothing");
        }

        var soredRoomDatas = new List<RoomData>(m_mapClassData.roomDatas);

        foreach (RoomData room in m_mapClassData.roomDatas)
        {
            switch (room.m_type)
            {
                case AreaType.None:
                    break;

                case AreaType.Summon:
                    SpawnEnemy(room);
                    break;

                case AreaType.Shop:
                    SpawnShop(room);
                    break;

                case AreaType.Damage:
                    SpawnTrap(room);
                    break;
            }
        }
    }

    private void SpawnEnemy(RoomData room)
    {
        var positions = ChooseRandomPosition(room, 3);


        foreach (var pos in positions)
        {
            if (IsForbiddenPos(pos))
                continue;

            Vector3 worldPositions = m_mapGeneration.GridToWorld(pos);
            SpawnEnemyByGacha(worldPositions);
        }

    }

    private void SpawnEnemyByGacha(Vector3 position)
    {
        if (m_enemyPool == null)
        {
            Debug.LogWarning("EnemyPool is null");
            return;
        }
        if (m_enemyGachaEngine == null)
        {
            Debug.LogWarning("EnemyGachaEngine is null");
            return;
        }
        if (m_enemyRarityTable == null)
        {
            Debug.LogWarning("EnemyRarityTable is null");
            return;
        }

        //choose
        RarityEnumAsset rarity = m_enemyGachaEngine.Collapse();
        //get enemy
        List<Enum_EnemyType> candidates = m_enemyRarityTable.GetEnemies(rarity);

        if (candidates.Count == 0)
        {
            Debug.LogWarning($"No Enemy Found : {rarity.name}");
            return;
        }

        // 同レアリティ内ランダム
        Enum_EnemyType selectedType = candidates[Random.Range(0, candidates.Count)];
        Debug.Log($"Selected Enemy : {selectedType}");
        EnemyController enemy =m_enemyPool.GetEnemy(selectedType);


        if (enemy == null)
        {
            Debug.LogWarning($"Pool Missing : {selectedType}");
            return;
        }

        enemy.transform.position = position;
        enemy.gameObject.SetActive(true);

        Debug.Log($"Spawn Enemy [{selectedType}] Rarity [{rarity.name}]");
    }

    private void SpawnShop(RoomData room)
    {
        var positions = ChooseRandomPosition(room, 1);

        foreach (var pos in positions)
        {
            if (IsForbiddenPos(pos)) continue;

            Instantiate(m_shopPrefab, m_mapGeneration.GridToWorld(pos), Quaternion.identity);
        }
    }

    private void SpawnTrap(RoomData room)
    {
        List<Vector3> worldPositions = new();
        foreach (var pos in room.m_roomSizes)
        {
            if (IsForbiddenPos(pos)) continue;

            worldPositions.Add(m_mapGeneration.GridToWorld(pos));
        }
        SpawnTrapByGacha(worldPositions);
    }

    private void SpawnTrapByGacha(List<Vector3> position)
    {
        if (m_trapPool == null)return;
        if (m_trapGachaEngine == null)return;
        if (m_trapRarityTable == null)return;

        RarityEnumAsset rarity = m_trapGachaEngine.Collapse();
        List<Enum_TrapType> candidates = m_trapRarityTable.GetTraps(rarity);

        if (candidates.Count == 0)
        {
            Debug.LogWarning($"No Trap Found : {rarity.name}");
            return;
        }

        Enum_TrapType selectedType = candidates[Random.Range(0, candidates.Count)];

        //
        foreach (Vector3 pos in position)
        {

            TrapBase trap = m_trapPool.GetTrap(selectedType);

            if (trap == null)
            {
                Debug.LogWarning($"Trap Pool Missing : {selectedType}");
                return;
            }

            trap.transform.position = pos;

            BoxCollider box = trap.GetComponent<BoxCollider>();

            if (box != null)
            {
                if (selectedType == Enum_TrapType.Gas || selectedType == Enum_TrapType.Swamp)
                {
                    box.size = new Vector3(m_mapGeneration.FloorScale.x, box.size.y, m_mapGeneration.FloorScale.z);
                }
                else
                {
                    box.size = Vector3.one;
                }
            }

            trap.Init(m_trapOwner, Vector3.forward, 1);
            trap.gameObject.SetActive(true);
        }
        Debug.Log($"Spawn Trap [{selectedType}] Rarity [{rarity.name}]");
    }

    private void SpawnGoal()
    {
        Vector3 pos =
            m_mapGeneration.GridToWorld(
                m_mapClassData.GoalPos);

        GameObject goal =
            Instantiate(
                m_goalPrefab,
                pos,
                Quaternion.identity);

        GoalSystem goalSystem =
            goal.GetComponent<GoalSystem>();

        goalSystem.Initialize(m_mainGameManager);
    }

    private void SpawnPlayer()
    {
        Vector3 pos =
            m_mapGeneration.GridToWorld(
                m_mapClassData.StartPos);

        pos.y = 0.5f;

        m_player.position = pos;

        if (m_camera != null)
        {
            m_camera.SetTarget(m_player);
        }
    }

    private void SpawnTreasures()
    {
        // Potential treasure chest spawn locations
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

        //Return smallest value
        int count = Mathf.Min(m_treasureCount, candidates.Count);

        for (int i = 0; i < count; i++)
        {
            //random selsect || Max roomSize
            int index = Random.Range(0, candidates.Count);

            Vector2Int pos = candidates[index];

            //delete index candidates
            candidates.RemoveAt(index);
           
            Vector3 worldPos = m_mapGeneration.GridToWorld(pos);

            bool isMimic = Random.value < m_mimicRate;

            if(isMimic)
            {
                Instantiate(m_mimicPrefab, worldPos, Quaternion.identity, m_treasureParent.transform);
                Debug.Log("Spawn Mimis");
            }
            else
            {
                Instantiate(m_treasurePrefab, worldPos, Quaternion.identity, m_treasureParent.transform);
                Debug.Log("Spawn TreasureBox");
            }
        }
    }

    private bool IsForbiddenPos(Vector2Int pos)
    {
        if (pos == GetStartPos()) return true;

        if (pos == GetGoalPos()) return true;

        return false;
    }

    private Enum_TrapType GetRandomTrapType()
    {
        int index = Random.Range(0, m_areaTrapType.Length);
        return m_areaTrapType[index];
    }

    /// <summary>
    /// random obtain the pos in the room
    /// </summary>
    /// <param name="room"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    private List<Vector2Int> ChooseRandomPosition(RoomData room, int count)
    {
        List<Vector2Int> copy = new(room.m_roomSizes);
        List<Vector2Int> result = new();

        count = Mathf.Min(count, copy.Count);

        for (int i = 0; i < count; i++)
        {
            int index = UnityEngine.Random.Range(0, copy.Count);

            result.Add(copy[index]);
            copy.RemoveAt(index);
        }

        return result;
    }
}