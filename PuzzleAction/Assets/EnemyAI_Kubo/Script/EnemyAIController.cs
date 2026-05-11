using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAIController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private GameObject m_target;  //UŒ‚‘ÎÛ

    [Header("Status")]
    [SerializeField] public int m_attackValue = 1; // Debug—p

    [Header("Range")]
    [SerializeField] private float m_findRange = 8f;      //õ“G”ÍˆÍ
    [SerializeField] private float m_attackRange = 1.5f;  //UŒ‚‰Â”\”ÍˆÍ

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

        float distance = Vector3.Distance(transform.position, m_target.transform.position);  //Player‚ÆEnemy‚Æ‚Ì‹——£ŒvZ

        // õ“G”ÍˆÍ“à‚É“ü‚Á‚½‚ç”­Œ©
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

        // UŒ‚”ÍˆÍ“à‚È‚çUŒ‚
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

    // target ’Ç]
    private void ChaseTarget()
    {
        if (!m_agent.pathPending)
        {
            m_agent.SetDestination(m_target.transform.position);
        }
    }

    // UŒ‚i¡‚ÍƒƒO‚Ì‚İj
    public void Attack()
    {
        //ÀÛ‚ÌUŒ‚ˆ—’Ç‰Á
        Debug.Log($"UŒ‚’†I ƒ_ƒ[ƒWF{m_attackValue}");
    }

    // ƒfƒoƒbƒO‰Â‹‰»
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_findRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_attackRange);
    }
}