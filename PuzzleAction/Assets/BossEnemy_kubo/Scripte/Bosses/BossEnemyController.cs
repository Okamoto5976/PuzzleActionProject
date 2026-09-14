using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossEnemyController : Entity
{
    [Header("Target")]
    [SerializeField] private Vector3Asset m_target;

    [Header("Range")]
    [SerializeField] private float m_findRange = 15f;
    [SerializeField] private float m_attackRange = 3f;

    [Header("Attack")]
    [SerializeField] private float m_attackCooldown = 3f;

    [Header("Drop")]
    [SerializeField] private int m_dropCount = 3;

    [Header("Ref")]
    [SerializeField] private AttackHitBox m_attackHitBox;
    private HitCollider m_hitCollider;

    [Header("Item")]
    [SerializeField] private Item m_attackItem;
    private ItemManager m_itemManager;

    private float m_cooldownTimer;
    private bool m_isCooldownReady = true;

    private NavMeshAgent m_agent;
    private IBossBehaviour m_bossBehaviour;
    private ReturnObjectToPool m_returnPool;

    public float FindRange => m_findRange;
    public float AttackRange => m_attackRange;

    public bool IsCooldownReady => m_isCooldownReady;

    public Vector3 SpawnPosition { get; private set; }

    public NavMeshAgent Agent => m_agent;

    public Vector3Asset Target => m_target;

    #region UNITY EVENT
    protected override void Awake()
    {
        base.Awake();

        SpawnPosition = transform.position;

        m_agent = GetComponent<NavMeshAgent>();
        m_hitCollider = GetComponent<HitCollider>();
        m_bossBehaviour = GetComponent<IBossBehaviour>();
        m_returnPool = GetComponent<ReturnObjectToPool>();
        m_itemManager = FindAnyObjectByType<ItemManager>();

        m_attackHitBox.m_transform = gameObject.transform;

        if (m_attackItem == null)
        {
            Debug.LogWarning($"{name} AttackItem Missing");
        }

        if (m_itemManager == null)
        {
            Debug.LogWarning($"{name} ItemManager Missing");
        }

        if (m_bossBehaviour != null)
        {
            m_bossBehaviour.Initialize(this);
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

        Rotate();

        if (distance > m_findRange)
        {
            StopAll();
            return;
        }

        m_bossBehaviour?.Execute();
    }
    #endregion

    #region ATTACK
    public bool TryUseCooldown()
    {
        if (!m_isCooldownReady) return false;

        ConsumeCooldown();
        return true;
    }
    public void ConsumeCooldown()
    {
        m_isCooldownReady = false;
        m_cooldownTimer = 0f;
    }
    private void HandleCooldown()
    {
        if (m_isCooldownReady) return;

        m_cooldownTimer += Time.deltaTime;

        if (m_cooldownTimer >= m_attackCooldown)
        {
            m_isCooldownReady = true;
            m_cooldownTimer = 0f;
        }
    }
    public bool TryAttack()
    {
        if (!m_isCooldownReady) return false;
        Attack();
        ConsumeCooldown();
        return true;
    }
    public void Attack()
    {
        Debug.DrawLine(transform.position, m_attackHitBox.m_transform.position, Color.red, 2f);
        Debug.Log(Vector3.Distance(m_attackHitBox.m_transform.position, m_target.Value));
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
            StunDuration = Stun,
            AttackDir = transform.forward,
            Attacker = this,
            //AttackerSE = AttackSE,
            //AudioSource = AudioSource
        };

        m_hitCollider.AttackCollider(damage, Team, m_attackHitBox);
        Debug.Log("BossEnemyController : Player ‚ÉHIT");
    }
    public void UseItem(Vector3 dir)
    {
        ItemRecieveData data = new ItemRecieveData
        {
            entity = this,
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

        m_agent.Move(dir * speed * Time.deltaTime);
    }
    public void SetDestination(Vector3 pos, float speed)
    {
        m_agent.isStopped = false;

        m_agent.speed = speed;
        m_agent.acceleration = speed * 2.5f;
        m_agent.stoppingDistance = m_attackRange;

        m_agent.SetDestination(pos);
    }
    public void Stop()
    {
        m_agent.isStopped = true;
    }
    private void StopAll()
    {
        Stop();
        m_bossBehaviour?.Stop();
    }
    private void Rotate()
    { 
        Vector3 dir = m_target.Value - transform.position;
        dir.y = 0;
        if (dir == Vector3.zero) return;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
    }
    public Vector3 GetRandomPosition(float range)
    {
        Vector3 result = transform.position;

        for (float i = range; i >= 0; i--)
        {
            Vector3 randomPoint = transform.position + new Vector3((Random.value * 2 - 1) * range, 0, (Random.value * 2 - 1) * range);
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                result.x = hit.position.x;
                result.y = hit.position.z;
                break;
            }
        }
        return result;
    }
    #endregion

    #region DEAD
    public virtual void OnDead(bool isItemDrop)
    {
        GameManager.Instance.SetKey();

        if(isItemDrop)
        {
            DropItems();
        }
    }
    private void DropItems()
    {
        if (m_itemManager == null)
            return;

        for (int i = 0; i < m_dropCount; i++)
        {
            Vector3 pos = transform.position + Random.insideUnitSphere;

            pos.y = transform.position.y;

            //m_itemManager.DropItemSetData(pos, m_dropCount);
        }
    }
    #endregion

    #region BOSS
    public static EnemyController SpawnEnemy(Enum_EnemyType type, Vector3 position)
    {
        Middleman_Enemy pool = FindAnyObjectByType<Middleman_Enemy>();
        if (pool == null)
        {
            Debug.LogWarning("Middleman_Enemy Not Found");
            return null;
        }
        //get enemy flom pool
        EnemyController enemy = pool.GetEnemy(type);
        if (enemy == null)
        {
            Debug.LogWarning($"Pool Missing : {type}");
            return null;
        }

        //set enemy info
        enemy.transform.position = position;
        enemy.gameObject.SetActive(true);
        Debug.Log($"spawn : {enemy}");

        return enemy;
    }
    #endregion
}

public interface IBossBehaviour
{
    void Initialize(BossEnemyController controller);
    void Execute();
    void Stop();
}