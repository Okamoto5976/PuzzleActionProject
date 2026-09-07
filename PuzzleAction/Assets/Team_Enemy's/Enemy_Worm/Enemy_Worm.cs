using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy_Worm : MonoBehaviour, IEnemyBehaviour
{
    private EnemyController m_enemyController;
    private EnemyWormController m_wormController = new();
    [SerializeField] private float m_detectDistance = 5;
    [SerializeField] private float m_attackAnimationTime = 2;
    [SerializeField] private float m_attackAnimationCooldown = 1;


    public void Initialized(EnemyController enemyController)
    {
        m_enemyController = enemyController;
        m_wormController.Initialize(m_enemyController, transform, m_detectDistance, m_attackAnimationTime, m_attackAnimationCooldown);
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
