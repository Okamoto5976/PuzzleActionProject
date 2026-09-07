using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy_RunAway : MonoBehaviour, IEnemyBehaviour
{
    private EnemyController m_enemyController;
    private EnemyRunAwayController m_enemyRunAwayController = new();

    [SerializeField] private float m_runRange = 4f;


    public void Initialized(EnemyController enemyController)
    {
        m_enemyController = enemyController;
        m_enemyRunAwayController.Initialize(enemyController, transform, m_runRange);
    }

    public void Execute()
    {
        m_enemyRunAwayController.DoRunAwayStates();
    }

    public void Stop() => m_enemyController.Stop();

}
