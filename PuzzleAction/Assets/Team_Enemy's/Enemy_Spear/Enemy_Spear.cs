using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy_Spear : MonoBehaviour, IEnemyBehaviour
{
    private EnemyController m_enemyController;
    private EnemySpearController m_enemySpearController = new();

    [SerializeField] private float preAttackDuration = 0.75f;
    [SerializeField] private float postAttackDuration = 3f;


    public void Initialized(EnemyController enemyController)
    {
        m_enemyController = enemyController;
        m_enemySpearController.Initialize(enemyController, transform, preAttackDuration, postAttackDuration);
    }

    public void Execute()
    {
        m_enemySpearController.DoSpearStates();
    }

    public void Stop() => m_enemyController.Stop();

}

