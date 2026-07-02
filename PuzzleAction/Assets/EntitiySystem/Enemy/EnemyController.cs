using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : Entity
{

    [Header("EnemyType")]
    [SerializeField] private Enum_EnemyType m_type;   

    [Header("Target")]
    //Debug
    [SerializeField] private Vector3Asset m_target;

    [Header("Range")]
    [SerializeField] private float m_findRange = 8f;
    [SerializeField] private float m_attackRange = 1.5f;

    [Header("Attack")]
    [SerializeField] private float m_attackCooldown = 5f;
    private float m_attackCooldownDuration;
    private bool m_isCooldownEnd = false;

    [Header("ref")]
    [SerializeField] private HitCollider m_hitCollider;
    [SerializeField] private AttackHitBox m_attackHitBox;

    [Header("Debug")]
    //[SerializeField] private ArrowTrap m_arrowPrefab;
    private ItemManager m_itemManager;
    [SerializeField] private Item m_item;

    private NavMeshAgent m_agent;
    private IEnemyBehaviour m_enemyBehaviour;

    //private bool m_isPreparing;
    public Vector3Asset Target => m_target;
    public NavMeshAgent Agent => m_agent;
    public float AttackRange => m_attackRange;
    public Vector3 Forward => transform.forward;

    protected override void Awake()
    {
        base.Awake();

        m_agent = GetComponent<NavMeshAgent>();
        m_itemManager = FindAnyObjectByType<ItemManager>();
        switch (m_type)
        {
            case Enum_EnemyType.Chase:
                gameObject.AddComponent<Enemy_Chase>();
                break;

            case Enum_EnemyType.Rush:
                gameObject.AddComponent<Enemy_Rush>();
                break;

            case Enum_EnemyType.Archer:
                gameObject.AddComponent<Enemy_Archer>();
                break;
        }

        m_enemyBehaviour = GetComponent<IEnemyBehaviour>();

        if (m_enemyBehaviour != null)
        {
            m_enemyBehaviour.Initialized(this);
        }

        m_agent.updateRotation = false;
        m_agent.updatePosition = true;

        //m_agent.stoppingDistance = m_attackRange;
    }   

    
    protected override void Update()
    {
        base.Update();

        if (m_target == null) return;

        //attackCooldown---------------------
        if (m_attackCooldownDuration > m_attackCooldown)
        {
            m_isCooldownEnd = true;
            m_attackCooldownDuration = 0f;
        }

        if (!m_isCooldownEnd)
        {
            m_attackCooldownDuration += Time.deltaTime;
        }
        //------------------------------------


        float distance = Vector3.Distance(transform.position, m_target.Value);

        HandleRotation(distance);

        // out findRange
        if (distance > m_findRange)
        {
            StopAll();
            return;
        }

        // in attackRange
        if (distance <= m_attackRange)
        {
            StopAll();
            if(m_isCooldownEnd)
            {
                Attack();
                m_isCooldownEnd = false;
            }
            //return;
        }

        m_enemyBehaviour.Execute();
    }

    // NavMesh Move
    public void Move(Vector3 dir, float speed)
    {
        //Rotate(dir);

        if (dir == Vector3.zero)
        {
            Stop();
            return;
        }

        m_agent.isStopped = false;

        m_agent.speed = speed;

        m_agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        m_agent.avoidancePriority = 50;

        m_agent.Move(dir * m_agent.speed * Time.deltaTime);

    }

    public void SetDestination(Vector3 targetPos, float speed)
    {
        //targetPos.y = 0;
        //Rotate(targetPos);
        m_agent.isStopped = false;
        m_agent.speed = speed;
        m_agent.acceleration = m_agent.speed * 2.5f;
        m_agent.stoppingDistance = m_attackRange;

        m_agent.SetDestination(targetPos);
    }

    public void UseItem(Vector3 dir)
    {
        //ArrowTrap arrow = Instantiate(
        //    m_arrowPrefab
        //);
        ItemRecieveData data = new ItemRecieveData
        {
            entity = this,
            baseValue = STR,
            pos = transform.position,
            dir = dir
        };

        m_itemManager.OnUseItem(m_item ,data);

        //arrow.Init(this, dir, 5);
        //arrow.transform.position = transform.position;
    }
    //-----common-----
    public void Stop()
    {
        m_agent.isStopped = true;
    }

    private void Rotate(Vector3 dir)
    {
        dir.y = 0f;
        if (dir == Vector3.zero) return;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            10 * Time.deltaTime
        );
    }

    private void HandleRotation(float distance)
    {
        if (distance > m_findRange) return;

        var rush = m_enemyBehaviour as Enemy_Rush;

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
        m_enemyBehaviour.Stop();
        Stop();
    }

    public void Attack()
    {
        //HitCollider���g����
        //Attack Process
        //DamageData damage = new DamageData
        //{
        //    Attack = (int)STR,
        //    HitRate = 100f,
        //    CriticalRate = CriticalRate,
        //    CriticalDamage = CriticalDamage,
        //    BreakRate = BreakRate,
        //    Knockback = KnockBack,
        //    Stun = Stun,
        //    AttackDir = transform.forward,
        //    Attacker = this,
        //    AttackerSE = AttackSE,
        //    AudioSource = AudioSource,
        //};

        //m_hitCollider.AttackCollider(damage, Team, m_attackHitBox);
        Debug.Log("HIT");
    }
}


public interface IEnemyBehaviour
{
    void Initialized(EnemyController Controller);
    void Execute();
    void Stop();
}