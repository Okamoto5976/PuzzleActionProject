using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : Entity
{
    [Header("EnemyType")]
    [SerializeField] private Enum_EnemyType m_type;
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
    [Header("Debug")]
    [SerializeField] private Item m_item;

    private ItemManager m_itemManager;

    private NavMeshAgent m_agent;
    private IEnemyBehaviour m_enemyBehaviour;
    private Vector3 m_spawnPosition;

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

    protected override void Awake()
    {
        base.Awake();

        m_spawnPosition = transform.position;

        m_agent = GetComponent<NavMeshAgent>();
        m_itemManager = FindAnyObjectByType<ItemManager>();

        switch (m_type)
        {
            case Enum_EnemyType.Chase:
                {
                    gameObject.AddComponent<Enemy_Chase>();
                    m_hitCollider = GetComponent<HitCollider>();
                    break;
                }

            case Enum_EnemyType.Rush:
                {
                    gameObject.AddComponent<Enemy_Rush>();
                    m_hitCollider = GetComponent<HitCollider>();
                    break;
                }

            case Enum_EnemyType.Archer:
                {
                    gameObject.AddComponent<Enemy_Archer>();
                    break;
                }

            case Enum_EnemyType.Mimic:
                {
                    gameObject.AddComponent<Enemy_Mimic>();
                    m_hitCollider = GetComponent<HitCollider>();
                    break;
                }
        }

        m_enemyBehaviour = GetComponent<IEnemyBehaviour>();

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
    public void ConsumeCooldown()
    {
        m_isCooldownEnd = false;
        m_attackCooldownDuration = 0f;
    }
    public bool TryAttack()
    {
        if (!m_isCooldownEnd) return false;

        Attack();
        ConsumeCooldown();
        return true;
    }
    public bool TryUseCooldown()
    {
        if (!m_isCooldownEnd) return false;

        ConsumeCooldown();

        return true;
    }

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

    public void UseItem(Vector3 dir)
    {
        ItemRecieveData data = new ItemRecieveData
            {
                entity = this,
                baseValue = STR,
                pos = transform.position,
                dir = dir
            };

        m_itemManager.OnUseItem(m_item, data);
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
                Attacker = this
            };

        m_hitCollider.AttackCollider(damage, Team, m_attackHitBox);
        Debug.Log("EnemyController : HIT");
    }
    //common
    public void Stop()
    {
        m_agent.isStopped = true;
    }

    private void Rotate(Vector3 dir)
    {
        dir.y = 0f;

        if (dir == Vector3.zero) return;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
    }

    private void HandleRotation(float distance)
    {
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
    private void StopAll()
    {
        m_enemyBehaviour?.Stop();
        Stop();
    }
}
public interface IEnemyBehaviour
{
    void Initialized(EnemyController Controller);
    void Execute();
    void Stop();
}