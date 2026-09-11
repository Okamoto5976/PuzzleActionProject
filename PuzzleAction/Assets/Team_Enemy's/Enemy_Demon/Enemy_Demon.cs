using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy_Demon : MonoBehaviour, IEnemyBehaviour
{
    private EnemyController m_enemyController;
    private EnemyDemonController m_enemyDemonController = new();

    [SerializeField] private float stepCooldown = 0.5f;
    [SerializeField] private float probabilityOfTakeStep = 75f;  // probability of take step
    [SerializeField] private float nextActionDurationMin = 1f;
    [SerializeField] private float nextActionDurationMax = 5f;


    public void Initialized(EnemyController enemyController)
    {
        m_enemyController = enemyController;
        //m_enemyDemonController.Initialize(enemyController, transform, preAttackDuration, postAttackDuration);
    }

    public void Execute()
    {
        m_enemyDemonController.DoDemonStates();
    }

    public void Stop() => m_enemyController.Stop();

}

