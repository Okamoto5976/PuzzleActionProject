using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyContllor : Entity
{
    [Header("Target")]
    [SerializeField] private Transform m_target;

    [Header("Range")]
    [SerializeField] private float m_findRange = 8f;
    [SerializeField] private float m_attackRange = 1.5f;

    [Header("Attack")]
    [SerializeField] private float m_attackCooldown = 5f;
    private float m_attackCooldownDuration;
    private bool m_isCooldownEnd = false;

    [SerializeField] private HitCollider m_hitCollider;
    [SerializeField] private AttackHitBox m_attackHitBox;

    private NavMeshAgent m_agent;
    private IEnemyBehaviour m_enemyBehaviour;

    private bool m_isPreparing;
    public Transform Target => m_target;
    public NavMeshAgent Agent => m_agent;
    public Vector3 Forward => transform.forward;

    protected override void Awake()
    {
        base.Awake();

        m_agent = GetComponent<NavMeshAgent>();
        m_enemyBehaviour = GetComponent<IEnemyBehaviour>();

        if (m_enemyBehaviour != null)
        {
            m_enemyBehaviour.Initialized(this);
        }

        m_agent.updateRotation = false;
        m_agent.updatePosition = true;
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


        float distance = Vector3.Distance(transform.position, m_target.transform.position);

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
            return;
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
            Vector3 dir = m_target.position - transform.position;
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
        DamageData damage = new DamageData
        {
            Attack = (int)STR,
            HitRate = 100f,
            CriticalRate=CriticalRate,
            CriticalDamage=CriticalDamage,
            BreakRate=BreakRate,
            Knockback=KnockBack,
            Stun=Stun,
            AttackDir=transform.forward,
            Attacker=this,
            AttackerSE=AttackSE,
            AudioSource=AudioSource,
        };

        m_hitCollider.AttackCollider(damage,Team,m_attackHitBox);
        Debug.Log("HIT");
    }
}


public interface IEnemyBehaviour
{
    void Initialized(EnemyContllor Controller);
    void Execute();
    void Stop();
}