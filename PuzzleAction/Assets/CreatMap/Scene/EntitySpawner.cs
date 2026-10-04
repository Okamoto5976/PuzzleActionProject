using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [Header("========== Player ==========")]
    [SerializeField] private Transform m_player;
    [SerializeField] private PlayerController m_playerC;
    [SerializeField] private CameraManager m_camera;
    [SerializeField] private float m_playerHeightOffset;
    [Space(10)]

    [Header("========== Enemy ==========")]
    [SerializeField] private Middleman_Enemy m_enemyPool;
    [Tooltip("1Piece3~5"), SerializeField] private int m_spawnCount;
    
    [Header("========== EnemyGacha ==========")]
    [SerializeField] private GachaEngine m_enemyGachaEngine;
    [SerializeField] private EnemyRarityTable m_enemyRarityTable;
    [Space(10)]

    [Header("========== Boss ==========")]
    [SerializeField] private Middleman_BossEnemy m_bossEnemyPool;
    [SerializeField] private List<Enum_BossType> m_bossOrder;
    [Space(10)]

    [Header("========== Trap ==========")]
    [SerializeField] private Middleman_Trap m_trapPool;
    private TrapGenerator_EqualDistribution m_trapEqualDistribution = new();
    [Header("========== TrapGacha ==========")]
    [SerializeField] private GachaEngine m_trapGachaEngine;
    [SerializeField] private TrapRarityTable m_trapRarityTable;
    [SerializeField] private Entity m_trapOwner;
    [SerializeField] private AreaTrapPlaceData m_areaTrapPlaceData;
    [Space(10)]

    [Header("========== Goal ==========")]
    [SerializeField] private GameObject m_goalPrefab;
    [SerializeField] private MainGameManager m_mainGameManager;
    [Space(10)]

    [Header("========== Shop ==========")]
    [SerializeField] private GameObject m_shopPrefab;
    [Space(10)]

    [Header("========== Treasure ==========")]
    [SerializeField] private Middleman_Treasure m_treasurePool;
    [SerializeField] private GachaEngine m_treasureGachaEngine;
    [SerializeField] private TreasureRarityTable m_treasureRarityTable;
    [SerializeField] private int m_treasureCount = 3;

    [Header("========== Treasure ==========")]
    [SerializeField] private GameObject m_spring;

    private HashSet<Vector2Int> m_reservedPosition = new();

    private MapClassData m_mapClassData;
    private MapGeneration m_mapGeneration;

    private readonly Enum_TrapType[] m_areaTrapType =
    {
        Enum_TrapType.GasArea,
        Enum_TrapType.SwampArea,
        Enum_TrapType.Dynamite
    };

    [Header("========== Ref ==========")]
    [SerializeField] private ItemManager m_itemManager;


    [Header("========== Tutorial ==========")]
    [SerializeField] private TutorialManager m_tutorialManager;
    [SerializeField] private List<Enum_EnemyType> m_tutorialEnemies = new();
    [SerializeField] private Enum_TrapType m_tutorialTrap;

    private bool m_isTutorial => GameManager.Instance.IsTutorial;

    public void Generate(MapClassData mapData, MapGeneration mapGeneration)
    {
        m_mapClassData = mapData;
        m_mapGeneration = mapGeneration;

        m_playerC.SetState();

        if(m_playerC.CheckTrophy())
        {
            Debug.LogWarning("Title Trophy spawn");
        }

        InitializeEnemyPools();
        InitializeBossEnemyPool();
        InitializeTrapPool();
        InitializeTreasurePool();

        SpawnGoal();

        ProcessAreaTypes();

        if(m_isTutorial)
        {
            TSpawnTreasures();
        }
        else
        {
            SpawnTreasures();

        }

        SpawnPlayer();
    }
    #region INITIALIZE
    private void InitializeEnemyPools()
    {
        if (m_enemyPool == null) return;
        m_enemyPool.InitializePool();
    }
    private void InitializeTrapPool()
    {
        if (m_trapPool == null) return;
        m_trapPool.InitializePool();
    }
    private void InitializeTreasurePool()
    {
        if (m_treasurePool == null) return;
        m_treasurePool.InitializePool();
    }
    private void InitializeBossEnemyPool()
    {
        if(m_bossEnemyPool == null) return;
        m_bossEnemyPool.InitializePool();
    }
    #endregion

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
                case AreaType.Summon:
                    if(m_isTutorial)
                    {
                        TSpawnEnemy(room);
                    }
                    else
                    {
                        SpawnEnemy(room);
                    }

                    break;

                case AreaType.Shop:
                    SpawnShop(room);
                    break;

                case AreaType.Damage:
                    if(m_isTutorial)
                    {
                        TSpawnTrap(room);
                    }
                    else
                    {
                        SpawnTrap(room);
                    }

                    break;

                case AreaType.Fairy:
                    SpawnFairy(room);
                    break;

                case AreaType.Boss:
                    SpawnBoss(room);
                        break;
                default:
                        break;
            }

            if(m_isTutorial)
            {

                switch (room.m_type)
                {
                    case AreaType.Summon:
                        var enemyEventPos = m_mapGeneration.GridToWorld(room.m_roomSizes[0]);
                        //EnemyAreaに入る前にEvent
                        m_tutorialManager.SetEnemyEventPos(enemyEventPos.z - 15f);

                        break;

                    case AreaType.Shop:
                        var shopEventPos = m_mapGeneration.GridToWorld(room.m_roomSizes[0]);
                        //ShopAreaに入る前にEvent
                        m_tutorialManager.SetShopEventPos(shopEventPos.z - 15f);
                        break;

                    case AreaType.Damage:
                        var trapEventPos = m_mapGeneration.GridToWorld(room.m_roomSizes[0]);
                        //TrapAreaに入ってEvent
                        m_tutorialManager.SetTrapEventPos(trapEventPos.z - 5f);

                        var attentionEventPos = m_mapGeneration.GridToWorld(room.m_roomSizes[0]);

                        m_tutorialManager.SetAttentionPos(attentionEventPos.z - 10f);
                        break;
                    default:
                        break;
                }

                //var Pos = m_mapGeneration.GridToWorld(room.m_roomSizes[0]);

                //Instantiate(obj, Pos, Quaternion.identity);

                //Pos = m_mapGeneration.GridToWorld(room.m_roomSizes[1]);

                //Instantiate(obj, Pos, Quaternion.identity);
            }
            
        }
    }

    //[SerializeField] private GameObject m_shopAreaTutorial;
    //[SerializeField] private GameObject m_trapAreaTutorial;
    //[SerializeField] private GameObject m_enemyAreaTutorial;


    #region SPAWN
    //Enemy
    private void SpawnEnemy(RoomData room)
    {
        var positions = ChooseRandomPosition(room, m_spawnCount);

        foreach (var pos in positions)
        {
            Vector3 worldPositions = m_mapGeneration.GridToWorld(pos);
            Vector3 WorldPositions = worldPositions + GetRandomSpawnOffset();


            SpawnEnemyByGacha(WorldPositions);

            // 2 enemies spawn
            if (Random.Range(0f, 1f) <= 0.05f)
            {

                SpawnEnemyByGacha(WorldPositions);

                Debug.Log($"Double Spawn : {WorldPositions}");
            }
            m_reservedPosition.Add(pos);
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

        // �����A���e�B�������_��
        Enum_EnemyType selectedType = candidates[Random.Range(0, candidates.Count)];
        EnemyController enemy =m_enemyPool.GetComponent(selectedType);


        if (enemy == null)
        {
            Debug.LogWarning($"Pool Missing : {selectedType}");
            return;
        }

        AssignDropItem(enemy);

        enemy.transform.position = position;
        enemy.gameObject.SetActive(true);
        enemy.InitializeSpawn();
    }

    //BossEnemy
    private void SpawnBoss(RoomData room)
    {
        Vector2Int center = room.m_roomSizes[room.m_roomSizes.Count / 2];
        Enum_BossType bossType = GetCurrentBossType();
        BossEnemyController boss = m_bossEnemyPool.GetComponent(bossType);

        if (boss == null)
        {
            Debug.LogError($"Boss Missing : {bossType}");
            return;
        }

        boss.transform.position = m_mapGeneration.GridToWorld(center);
        boss.gameObject.SetActive(true);
        boss.InitializeSpawn();
        m_reservedPosition.Add(center);
    }
    private Enum_BossType GetCurrentBossType()
    {
        int level = GameManager.Instance.Level;
        int bossIndex = (level / 5) - 1;
        if (bossIndex < 0) bossIndex = 0;
        bossIndex %= m_bossOrder.Count;
        return m_bossOrder[bossIndex];
    }
    //itemDrop
    private void AssignDropItem(EnemyController enemy)
    {
        if (enemy == null) return;

        if (m_itemManager == null) 
        {
            Debug.Log("ItemManager is null");
            return; 
        }
        if(enemy.ItemDropGachaEngine == null)
        {
            Debug.LogWarning($"{enemy.name} ItemDropGachaEngine missing");
            return;
        }

        //Drop rates for each enemy
        RarityEnumAsset rarity = enemy.ItemDropGachaEngine.Collapse();

        // hoka tantou jissou yotei 
        Item item = m_itemManager.DropItem(rarity);
        //Item item = null;
        //set drop item 
        enemy.DropItem = item;
        //Debug.LogWarning($"{enemy.name} DropRarity = {rarity.name}");
    }

    //Trap
    private void SpawnTrap(RoomData room)
    {
        List<Vector3> worldPositions = new();
        foreach (var pos in room.m_roomSizes)
        {
            worldPositions.Add(m_mapGeneration.GridToWorld(pos));
            m_reservedPosition.Add(pos);
        }
        SpawnTrapByGacha(worldPositions);

    }
    private void SpawnTrapByGacha(List<Vector3> positions)
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

        if (selectedType == Enum_TrapType.GasArea ||
            selectedType == Enum_TrapType.SwampArea ||
            selectedType == Enum_TrapType.PoisonArea ||
            selectedType == Enum_TrapType.BurnArea ||
            selectedType == Enum_TrapType.HealingArea ||
            selectedType == Enum_TrapType.StunArea)
        {
            foreach (Vector3 pos in positions)
            {

                TrapBase trap = m_trapPool.GetComponent(selectedType);

                if (trap == null)
                {
                    Debug.LogWarning($"Trap Pool Missing : {selectedType}");
                    return;
                }

                trap.gameObject.transform.position = pos;

                BoxCollider box = trap.GetComponent<BoxCollider>();

                if (box != null)
                {
                    box.size = new Vector3(m_mapGeneration.FloorScale.x, box.size.y, m_mapGeneration.FloorScale.z);
                }

                //ItemRecieveData data = new ItemRecieveData()
                //{
                //    entity = null,
                //    pos = pos,
                //};

                trap.TrapInit();
                trap.gameObject.SetActive(true);
            }
        }
        else
        {
            float trapDensity = m_areaTrapPlaceData.GetAreaTrapPlaceData(selectedType);
            m_trapEqualDistribution.SpawnTraps(positions, m_mapGeneration.FloorScale, m_trapPool, selectedType, trapDensity);
        }
        Debug.Log($"Spawn Trap [{selectedType}] Rarity [{rarity.name}]");
    }

    //Object
    private void SpawnShop(RoomData room)
    {
        var positions = ChooseRandomPosition(room, 1);

        foreach (var pos in positions)
        {
            Instantiate(m_shopPrefab, m_mapGeneration.GridToWorld(pos), Quaternion.identity);
            m_reservedPosition.Add(pos);
        }
    }

    private void SpawnFairy(RoomData room)
    {
        var positions = ChooseRandomPosition(room, 1);

        foreach(var pos in positions)
        {
            Instantiate(m_spring, m_mapGeneration.GridToWorld(pos), Quaternion.identity);
            m_reservedPosition.Add(pos);
        }
    }

    private void SpawnGoal()
    {
        Vector3 pos = m_mapGeneration.GridToWorld(m_mapClassData.GoalPos);
        GameObject goal = Instantiate(m_goalPrefab, pos, Quaternion.identity);
        GoalSystem goalSystem = goal.GetComponent<GoalSystem>();
        goalSystem.Initialize(m_mainGameManager);
    }
    private void SpawnPlayer()
    {
        Vector3 pos = m_mapGeneration.GridToWorld(m_mapClassData.StartPos);
        m_player.position = pos;

        if (m_camera != null)
        {
            m_camera.SetTargetAndHeightOffset(m_player, m_playerHeightOffset);
        }

        //SpawnTitleTrophy(pos);
    }

    public void SpawnTitleTrophy(Vector3 pos)
    {
        if (!m_playerC.CheckTrophy()) return;
        if(m_itemManager == null) return;
        int id = 82;
        Item titleTrophy = m_itemManager.GetItem(id); // Title Trophy Id

        if(titleTrophy == null)
        {
            Debug.LogWarning($"{id} Trophy Not Found");
            return;
        }
        m_itemManager.DropItemSetData(pos, titleTrophy);
        Debug.Log("GameTitle Trophy Spawn");
    }

    //Treasure
    private void SpawnTreasures()
    {
        // Potential treasure chest spawn locations
        List<Vector2Int> candidates = new();

        foreach (var room in m_mapClassData.roomDatas)
        {
            //reject BossArea
            if (room.m_type == AreaType.Boss) continue;
            //reject rooms containing a StartPos
            if (room.m_roomSizes.Contains(GetStartPos())) continue;
            // reject other
            foreach(var pos in room.m_roomSizes)
            {
                //reject startPos goalPos
                if(IsForbiddenPos(pos)) continue;
                //reject Enemy, Trap, Shop, Boss. position
                if (m_reservedPosition.Contains(pos)) continue;
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

            SpawnTreasureByGacha(m_mapGeneration.GridToWorld(pos));
        }
    }
    private void SpawnTreasureByGacha(Vector3 position)
    {
        if (m_treasureGachaEngine == null) return;
        if (m_treasureRarityTable == null) return;

        RarityEnumAsset rarity = m_treasureGachaEngine.Collapse();

        List<Enum_TreasureType> candidates = m_treasureRarityTable.GetTreasures(rarity);

        if (candidates.Count == 0)
        {
            Debug.LogWarning($"Treasure Not Found : {rarity.name}");
            return;
        }

        Enum_TreasureType selectedType = candidates[Random.Range(0,candidates.Count)];

        switch (selectedType)
        {
            case Enum_TreasureType.TreasureBox:
                {
                    //gete Pool 
                    Treasure treasure = m_treasurePool.GetComponent(Enum_TreasureType.TreasureBox);
                    if (treasure == null)return;

                    treasure.transform.position = position;
                    treasure.gameObject.SetActive(true);

                    break;
                }

            case Enum_TreasureType.Mimic:
                {
                    EnemyController mimic = m_enemyPool.GetComponent(Enum_EnemyType.Mimic);

                    if (mimic == null) return;

                    AssignDropItem(mimic);

                    mimic.transform.position = position;
                    mimic.gameObject.SetActive(true);
                    mimic.InitializeSpawn();

                    break;
                }
        }
    }
    #endregion

    #region TutorialSPAWN

    //Enemy
    private void TSpawnEnemy(RoomData room)
    {
        int num = 0;

        //ListCount = spawnCount
        var positions = ChooseRandomPosition(room, 3);

        foreach (var pos in positions)
        {
            Vector3 worldPositions = m_mapGeneration.GridToWorld(pos);
            Vector3 WorldPositions = worldPositions + GetRandomSpawnOffset();

            TSpawnEnemyByGacha(WorldPositions, num);
            num++;

            m_reservedPosition.Add(pos);
        }

    }

    private void TSpawnEnemyByGacha(Vector3 position, int num)
    {
        if (m_enemyPool == null)
        {
            Debug.LogWarning("EnemyPool is null");
            return;
        }

        Enum_EnemyType selectedType = m_tutorialEnemies[num];

        EnemyController enemy = m_enemyPool.GetComponent(selectedType);

        if (enemy == null)
        {
            Debug.LogWarning($"Pool Missing : {selectedType}");
            return;
        }

        AssignDropItem(enemy);

        enemy.transform.position = position;
        enemy.gameObject.SetActive(true);
        enemy.InitializeSpawn();
    }


    //Trap
    private void TSpawnTrap(RoomData room)
    {
        List<Vector3> worldPositions = new();
        foreach (var pos in room.m_roomSizes)
        {
            worldPositions.Add(m_mapGeneration.GridToWorld(pos));
            m_reservedPosition.Add(pos);
        }
        TSpawnTrapByGacha(worldPositions);

    }
    private void TSpawnTrapByGacha(List<Vector3> positions)
    {
        if (m_trapPool == null) return;

        float trapDensity = m_areaTrapPlaceData.GetAreaTrapPlaceData(m_tutorialTrap);
        m_trapEqualDistribution.SpawnTraps(positions, m_mapGeneration.FloorScale, m_trapPool, m_tutorialTrap, trapDensity);

    }

    private void TSpawnTreasures()
    {
        // Potential treasure chest spawn locations
        List<Vector2Int> candidates = new();

        foreach (var room in m_mapClassData.roomDatas)
        {
            //reject BossArea
            if (room.m_type == AreaType.Boss) continue;
            //reject rooms containing a StartPos
            if (room.m_roomSizes.Contains(GetStartPos())) continue;
            // reject other
            foreach (var pos in room.m_roomSizes)
            {
                //reject startPos goalPos
                if (IsForbiddenPos(pos)) continue;
                //reject Enemy, Trap, Shop, Boss. position
                if (m_reservedPosition.Contains(pos)) continue;
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

            TSpawnTreasureByGacha(m_mapGeneration.GridToWorld(pos));
        }
    }
    private void TSpawnTreasureByGacha(Vector3 position)
    {
        if (m_treasureGachaEngine == null) return;
        if (m_treasureRarityTable == null) return;

        //gete Pool 
        Treasure treasure = m_treasurePool.GetComponent(Enum_TreasureType.TreasureBox);
        if (treasure == null) return;

        treasure.transform.position = position;
        treasure.gameObject.SetActive(true);

    }

    #endregion

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

    private Vector3 GetRandomSpawnOffset()
    {
        float maxOffset = Mathf.Min(m_mapGeneration.FloorScale.x, m_mapGeneration.FloorScale.z) * 0.5f;

        Vector2 offset = Random.insideUnitCircle * maxOffset;

        return new Vector3(offset.x, 0f, offset.y);
    }

    /// <summary>
    /// random obtain the pos in the room
    /// </summary>
    /// <param name="room"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    private List<Vector2Int> ChooseRandomPosition(RoomData room, int count)
    {
        List<Vector2Int> copy = new();
        foreach (var pos in room.m_roomSizes)
        {
            if (IsForbiddenPos(pos)) continue;

            copy.Add(pos);
        }

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