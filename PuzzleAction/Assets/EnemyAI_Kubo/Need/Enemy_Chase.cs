using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy_Chase : MonoBehaviour
{
    private NavMeshAgent m_agent;
    private PlayerData m_target;

    private void Update()
    {

        m_agent.updatePosition = true;
        m_agent.updateRotation = true;

    }
    public void Initialized(PlayerData target)
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
            m_agent.SetDestination(m_target.PlayerPostition);
        }
    }

    public void Stop()
    {
        m_agent.isStopped = true;
    }
}