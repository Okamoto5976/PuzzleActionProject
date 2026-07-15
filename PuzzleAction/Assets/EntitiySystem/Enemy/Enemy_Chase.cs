using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy_Chase : MonoBehaviour, IEnemyBehaviour
{
    private EnemyController m_enemyController;

    public void Initialized(EnemyController enemyController) => m_enemyController = enemyController;

    public void Execute()
    {
        if (m_enemyController.Target == null) return;
        m_enemyController.SetDestination(m_enemyController.Target.Value, m_enemyController.Speed);
    }

    public void Stop() => m_enemyController.Stop();

}

