using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyContllor : Entity
{
    public enum EnemyType
    {
        Chase,
        Rush
    }

    [Header("Type")]
    [SerializeField] private EnemyType m_type;

    [Header("Target")]
    [SerializeField] private Transform m_target;

    [Header("Range")]
    [SerializeField] private float m_findRange = 8f;
    [SerializeField] private float m_attackRange = 1.5f;

    private NavMeshAgent m_agent;
    private Enemy_Chase m_chase;
    private Enemy_Rush m_rush;

    private bool m_isPreparing;
    public Transform Target => m_target;
    public NavMeshAgent Agent => m_agent;

    protected override void Awake()
    {
        base.Awake();

        m_agent = GetComponent<NavMeshAgent>();
        m_chase = GetComponent<Enemy_Chase>();
        m_rush = GetComponent<Enemy_Rush>();

        if (m_chase != null) m_chase.Initialized(this);
        if (m_rush != null) m_rush.Initialized(this);

        m_agent.updateRotation = false;
        m_agent.updatePosition = true;
    }

    
    private void Update()
    {
        if (m_target == null) return;


        float distance = Vector3.Distance(transform.position, m_target.transform.position);

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
            Attack();
            return;
        }

   
        switch (m_type)
        {
            case EnemyType.Chase:
                m_chase.Execute();
                return;

            case EnemyType.Rush:

                if (m_rush.IsRunning)
                {
                    Move(m_rush.GetDirection());
                    return;
                }

                if (m_isPreparing)
                {
                    m_rush.UpdatePrepare(m_target.transform.position);
                    return;
                }

                StartRushPrepare();
                return;
        }
    }

    // NavMesh Move
    //-----Rush-----
    public void Move(Vector3 dir)
    {
        if (dir == Vector3.zero)
        {
            Stop();
            return;
        }

        m_agent.isStopped = false;

        m_agent.speed = DashSpeed;
        m_agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        m_agent.avoidancePriority = 50;

        m_agent.Move(dir * m_agent.speed * Time.deltaTime);

       // Rotate(dir);
    }

    private void StartRushPrepare()
    {
        if (!m_isPreparing)
        {
            m_isPreparing = true;

            m_rush.Ready();
            StartCoroutine(RushDelay());
        }
    }

    private IEnumerator RushDelay()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1f));

        m_rush.StartRush();
        m_isPreparing = false;
    }
    //-----Chase-----
    public void SetDestination(Vector3 targetPos)
    {
        m_agent.isStopped = false;
        m_agent.speed = Speed;
        m_agent.acceleration = Speed * 5;
        m_agent.SetDestination(targetPos);
        //Rotate(targetPos);
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
            Time.deltaTime * 10
        );
    }


    private void StopAll()
    {
        if (m_chase != null) m_chase.Stop();
        if (m_rush != null) m_rush.Stop();

        StopAllCoroutines();
        m_isPreparing = false;

        Stop();
    }

    private void Attack()
    {
        //Attack Process
        Debug.Log("HIT");
    }

}
