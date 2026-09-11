using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy_Worm : MonoBehaviour, IEnemyBehaviour
{
    private EnemyController m_enemyController;
    [SerializeField] private EnemyWormController m_wormController = new();


    public void Initialized(EnemyController enemyController)
    {
        m_enemyController = enemyController;
        m_wormController.Initialize(m_enemyController, transform);
    }

    /// <summary>
    /// Do Behaviour
    /// </summary>
    public void Execute()
    {
        m_wormController.DoWormState();
    }

    public void Stop() => m_enemyController.Stop();
}
