using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private GameObject m_target;

    [Header("Status")]
    [SerializeField] public int m_attackValue = 1;

    [Header("Range")]
    [SerializeField] private float m_findRange = 8f;
    [SerializeField] private float m_attackRange = 1.5f;

    private NavMeshAgent m_agent;
    private Rigidbody m_rb;

    [NonSerialized] public bool m_isFound = false;
    [NonSerialized] public bool m_isAttacking = false;

    public int AttackValue => m_attackValue;
    public bool IsFaund => m_isFound;
    public bool IsAttacking => m_isAttacking;

    private void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (m_target == null) return;

        float distance = Vector3.Distance(transform.position, m_target.transform.position);

        // 索敵
        if (distance <= m_findRange)
        {
            m_isFound = true;
            m_agent.isStopped = false;
        }
        else
        {
            m_isFound = false;
            m_isAttacking = false;
            m_agent.isStopped = true;
            return;
        }

        // 攻撃
        if (distance <= m_attackRange)
        {
            m_isAttacking = true;
            m_agent.isStopped = true;
            Attack();
        }
        else
        {
            m_isAttacking = false;
            ChaseTarget();
        }
    }

    // target 追従
    private void ChaseTarget()
    {
        if (!m_agent.pathPending)
        {
            m_agent.SetDestination(m_target.transform.position);
        }
    }

    // 攻撃（今はログのみ）
    public void Attack()
    {
        Debug.Log($"攻撃中！ ダメージ：{m_attackValue}");
    }

    // デバッグ可視化
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_findRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_attackRange);
    }
}