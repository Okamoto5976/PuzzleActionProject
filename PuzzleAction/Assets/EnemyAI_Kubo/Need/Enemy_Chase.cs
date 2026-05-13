using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy_Chase : MonoBehaviour
{
    private NavMeshAgent m_agent;
    private GameObject m_target;

    public void Initialized(GameObject target)
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_target = target;
    }

    public void Move()
    {
        if (m_target == null) return;

        if (!m_agent.pathPending)
        {
            m_agent.isStopped = false;
            m_agent.SetDestination(m_target.transform.position);
        }
    }

    public void Stop()
    {
        m_agent.isStopped = true;
    }
}