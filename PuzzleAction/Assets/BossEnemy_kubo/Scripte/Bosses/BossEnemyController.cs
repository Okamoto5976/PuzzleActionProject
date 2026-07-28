using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossEnemyController : Entity
{
    [Header("Boss Type")]
    [SerializeField] private Enum_BossType m_bossType;

    [Header("Target")]
    [SerializeField] private Vector3Asset m_target;

    [Header("Range")]
    [SerializeField] private float m_findRange = 20f;
    [SerializeField] private float m_attackRange = 3f;

    [Header("Attack Control")]
    [SerializeField] private float m_attackCooldown = 4f;
    private float m_attackTimer;
    private bool m_canAttack = true;

    [Header("Item")]
    [SerializeField] private ItemManager m_itemManager;
    [SerializeField] private Item m_item;

    [Header("Key")]
    [SerializeField] private GameObject m_keyPrefab;
    [SerializeField] private Transform m_keySpawnPoint;

    [Header("Debug")]
    [SerializeField] private bool m_isAttacking = false;


    private NavMeshAgent m_agent;
    private IBossBehaviour m_bossBehaviour;

    public Vector3Asset Target => m_target;
    public NavMeshAgent Agent => m_agent;
    public float AttackRange => m_attackRange;
    public Vector3 Forward => transform.forward;
    public bool IsAttacking => m_isAttacking;

    protected override void Awake()
    {
        base.Awake();

        m_agent = GetComponent<NavMeshAgent>();

        //name descending
        switch (m_bossType)
        {
            //case Enum_BossType.Demon:
            //    gameObject.AddComponent<Boss_Demon>();
            //    break;

            //case Enum_BossType.Dragon:
            //    gameObject.AddComponent<Boss_Dragon>();
            //    break;

            case Enum_BossType.Golem:

                gameObject.AddComponent<Boss_Golem>();
                break;

        }

        m_bossBehaviour = GetComponent<IBossBehaviour>();

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

        HandleRotation(distance);

        // findRange
        if (distance > m_findRange)
        {
            StopAll();
            return;
        }

        // attackRange
        if (distance <= m_attackRange)
        {
            Stop();

            if (m_canAttack && !m_isAttacking)
            {
                m_bossBehaviour.Attack();
                m_canAttack = false;
            }
        }

        // Excute boss AI 
        if (!m_isAttacking)
        {
            m_bossBehaviour.Execute();
        }
    }

    //Movement
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

    public void SetDestination(Vector3 targetPos, float speed)
    {
        m_agent.isStopped = false;
        m_agent.speed = speed;
        m_agent.acceleration = speed * 2f;
        m_agent.stoppingDistance = m_attackRange;

        m_agent.SetDestination(targetPos);
    }

    public void UseItem(ItemRecieveData data)
    {
        if(m_item == null) return;

        if (m_itemManager == null) return;

        m_itemManager.OnUseItem(m_item, data);
    }

    public void Stop()
    {
        m_agent.isStopped = true;
    }

    private void StopAll()
    {
        Stop();

        if (m_bossBehaviour != null)
        {
            m_bossBehaviour.Stop();
        }
    }

    // Rotation
    private void HandleRotation(float distance)
    {
        if (m_target == null) return;

        Vector3 dir = m_target.Value - transform.position;
        dir.y = 0;

        if (dir == Vector3.zero) return;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            10f * Time.deltaTime
        );
    }

    // Attack
    private void HandleCooldown()
    {
        if (!m_canAttack) 
        {
            m_attackTimer += Time.deltaTime;

            if (m_attackTimer >= m_attackCooldown)
            {
                m_attackTimer = 0f;
                m_canAttack = true;
            }
        }
    }

    public void SetAttacking(bool value)
    {
        m_isAttacking = value;

        if (value)
        {
            Stop();
        }
    }

    // Boss Attack End Hook
    public void StartAttack()
    {
        Debug.Log("[BOSS] Attack Lock ON");
        m_isAttacking = true;
    }
    public void EndAttack()
    {
        Debug.Log("[BOSS] Attack Finished");
        m_isAttacking = false;
    }

    // Death / Drop
    protected virtual void Die()
    {
        DropKey();
        Destroy(gameObject);
    }

    private void DropKey()
    {
        if (m_keyPrefab == null) return;

        Vector3 pos = m_keySpawnPoint != null ? m_keySpawnPoint.position : gameObject.transform.position;

        Instantiate(m_keyPrefab, pos, Quaternion.identity);
    }


    //--------------------------------------------------------------------------------------------------------------
    // debug display
    //--------------------------------------------------------------------------------------------------------------

    #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            // Find Range
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position,m_findRange);

            // Attack Range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,m_attackRange);

            // Target Line
            if (m_target != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position,m_target.Value);
            }
        }
    #endif
}
