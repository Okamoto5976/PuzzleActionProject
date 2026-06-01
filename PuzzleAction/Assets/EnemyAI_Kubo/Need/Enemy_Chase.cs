using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy_Chase : MonoBehaviour
{
    private EnemyContllor m_enemyController;

    public void Initialized(EnemyContllor enemyController)
    {
        m_enemyController = enemyController;
    }


    public void Execute()
    {
        if (m_enemyController.Target == null) return;

        var agent = m_enemyController.Agent;
        if(!agent.pathPending)
            m_enemyController.SetDestination(m_enemyController.Target.position);
    }

    public void Stop()
    {
        m_enemyController.Stop();
    }

}

