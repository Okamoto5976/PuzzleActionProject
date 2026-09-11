using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : Entity
{
    [Header("Target")]
    [SerializeField] private Vector3Asset m_target;
    [Header("Range")]
    [SerializeField] private float m_findRange = 8f;
    [SerializeField] private float m_attackRange = 1.5f;
    [Header("Attack")]
    [SerializeField] private float m_attackCooldown = 1f;
    private float m_attackCooldownDuration;
    private bool m_isCooldownEnd = true;
    [Header("Ref")]
    [SerializeField] private AttackHitBox m_attackHitBox;
    private HitCollider m_hitCollider;
    [Header("Item")]
    [SerializeField] private Item m_attackItem;
    private ItemManager m_itemManager;

    [Header("Drop")]
    [SerializeField] private GachaEngine m_itemDropGachaEngine;
    public GachaEngine ItemDropGachaEngine => m_itemDropGachaEngine;
    [NonSerialized] public Item m_dropItem;
    public Item DropItem
    {
        get => m_dropItem;
        set => m_dropItem = value;
    }

    private NavMeshAgent m_agent;
    private IEnemyBehaviour m_enemyBehaviour;
    private Vector3 m_spawnPosition;

    private bool m_isRotating = true;

    //===== API =====

    public float AttackRange => m_attackRange;
    public float FindRange => m_findRange;
    public bool IsCooldownReady => m_isCooldownEnd;
    public Vector3 Forward => transform.forward;
    public Vector3 SpawnPosition => m_spawnPosition;
    public Vector3Asset Target => m_target;
    public NavMeshAgent Agent => m_agent;
    public AttackHitBox AttackHitBox => m_attackHitBox;
    public HitCollider HitCollider => m_hitCollider;

    #region UNITY EVENT
    protected override void Awake()
    {
        base.Awake();

        m_spawnPosition = transform.position;

        m_agent = GetComponent<NavMeshAgent>();
        m_itemManager = FindAnyObjectByType<ItemManager>();
        if(m_itemManager == null)
        {
            Debug.LogWarning($"{this.name} : ItemManager Not Found");
        }



        m_enemyBehaviour = GetComponent<IEnemyBehaviour>();
        m_hitCollider = GetComponent<HitCollider>();

        if (m_enemyBehaviour != null)
        {
            m_enemyBehaviour.Initialized(this);
        }

        m_agent.updateRotation = false;
        m_agent.updatePosition = true;
    }

    private void Update()
    {
        OnUpdateFlag();

        if (m_target == null) return;

        HandleCooldown();

        float distance = Vector3.Distance(transform.position, m_target.Value);

        HandleRotation(distance);

        if (distance > m_findRange)
        {
            StopAll();
            return;
        }

        //if (m_type == Enum_EnemyType.Chase || m_type == Enum_EnemyType.Mimic)
        //{
        //    if (distance <= m_attackRange)
        //    {
        //        StopAll();

        //        TryAttack();

        //        return;
        //    }
        //}
        m_enemyBehaviour.Execute();
    }
    private void OnEnable()
    {
        m_isCooldownEnd = true;
        m_attackCooldownDuration = 0f;

        if(m_entityHP is EnemyHP hp)
        {
            hp.ResetHP();
        }
    }
    #endregion

    #region ATTACK
    public bool TryAttack()
    {
        if (!m_isCooldownEnd) return false;

        Attack();
        ConsumeCooldown();
        return true;
    }
    public void Attack()
    {
        Debug.DrawLine(transform.position,m_attackHitBox.m_transform.position,Color.red,2f);
        Debug.Log(Vector3.Distance(m_attackHitBox.m_transform.position,m_target.Value));
        Debug.Log(m_attackHitBox.m_transform.position);
        Debug.Log(m_attackHitBox.m_radius);


        if (m_hitCollider == null) return;

        DamageData damage = new DamageData
            {
                Attack = (int)STR,
                CriticalRate = CriticalRate,
                CriticalDamage = CriticalDamage,
                BreakRate = BreakRate,
                Knockback = KnockBack,
                Stun = Stun,
                AttackDir = transform.forward,
                Attacker = this,
                //AttackerSE = AttackSE,
                //AudioSource = AudioSource
            };

        m_hitCollider.AttackCollider(damage, Team, m_attackHitBox);
        Debug.Log("EnemyController : Player ‚ÉHIT");
    }
    private void HandleCooldown()
    {
        if (m_isCooldownEnd) return;

        m_attackCooldownDuration += Time.deltaTime;

        if (m_attackCooldownDuration >= m_attackCooldown)
        {
            m_attackCooldownDuration = 0f;
            m_isCooldownEnd = true;
        }
    }
    public bool TryUseCooldown()
    {
        if (!m_isCooldownEnd) return false;

        ConsumeCooldown();

        return true;
    }
    public void ConsumeCooldown()
    {
        m_isCooldownEnd = false;
        m_attackCooldownDuration = 0f;
    }
    public void UseItem(Vector3 dir)
    {
        ItemRecieveData data = new ItemRecieveData
        {
            entity = this,
            baseValue = STR,
            pos = transform.position,
            dir = dir
        };

        m_itemManager.OnUseItem(m_attackItem, data);
    }
    #endregion

    #region MOVE
    public void Move(Vector3 dir, float speed)
    {
        if (dir == Vector3.zero)
        {
            Stop();
            return;
        }

        m_agent.isStopped = false;
        m_agent.speed = speed;

        m_agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;

        m_agent.avoidancePriority = 50;

        m_agent.Move(dir * speed * Time.deltaTime);
    }
    public void SetDestination(Vector3 targetPos, float speed)
    {
        m_agent.isStopped = false;

        m_agent.speed = speed;
        m_agent.acceleration = speed * 2.5f;
        m_agent.stoppingDistance = m_attackRange;

        m_agent.SetDestination(targetPos);
    }
    private void HandleRotation(float distance)
    {
        if (!m_isRotating) return;
        if (distance > m_findRange) return;

        Enemy_Rush rush = m_enemyBehaviour as Enemy_Rush;

        if (rush != null && rush.IsRunning)
        {
            Rotate(rush.CurrentDirection);
        }
        else
        {
            Vector3 dir = m_target.Value - transform.position;
            Rotate(dir);
        }
    }
    private void Rotate(Vector3 dir)
    {
        dir.y = 0f;

        if (dir == Vector3.zero) return;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
    }
    public void SetEnableRotation(bool state)
    {
        m_isRotating = state;
    }

    public void Stop()
    {
        m_agent.isStopped = true;
    }
    private void StopAll()
    {
        m_enemyBehaviour?.Stop();
        Stop();
    }
    public Vector2 GetRandomPosition(float range)
    {
        Vector2 result = new(transform.position.x, transform.position.z);

        for (var i = range; i >= 0; i -= 1)
        {
            Vector3 randomPoint = transform.position + new Vector3((UnityEngine.Random.value * 2 - 1) * range, 0, (UnityEngine.Random.value * 2 - 1) * range);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result.x = hit.position.x;
                result.y = hit.position.z;
                break;
            }
        }

        return result;
    }
    public void TeleportToPosition(Vector2 position)
    {
        Vector3 origin = transform.position;
        origin.x = position.x;
        origin.z = position.y;
        m_agent.Warp(origin);
    }
    #endregion

    #region DEAD 

    public void OnDead(bool isDropItem = true)
    {
        if (isDropItem)
        {
            ItemDrop();
        }
        //ReturnPool();
    }

    public void ItemDrop()
    {
        if (m_dropItem == null) return;
        //m_itemManager.ItemDrop(m_itemDrop, transform.position)
    }
    public void ReturnPool()
    {
        ReturnObjectToPool pool = GetComponent<ReturnObjectToPool>();
        if(pool == null)
        {
            Debug.LogWarning($"{this.name} : ReturnObjectToPool Not Found");
        }
        pool.ReturnToPool();
    }
    #endregion

    #region ENEMY

    public static EnemyController SpawnEnemy(Enum_EnemyType type, Vector3 position)
    {
        Middleman_Enemy pool = FindAnyObjectByType<Middleman_Enemy>();
        if(pool == null)
        {
            Debug.LogWarning("Middleman_Enemy Not Found");
            return null;
        }
        EnemyController enemy = pool.GetEnemy(type);
        if(enemy == null)
        {
            Debug.LogWarning($"Pool Missing : {type}");
            return null;
        }

        enemy.transform.position = position;
        enemy.gameObject.SetActive(true);

        return enemy;
    }
    #endregion
}
public interface IEnemyBehaviour
{
    void Initialized(EnemyController Controller);
    void Execute();
    void Stop();
}