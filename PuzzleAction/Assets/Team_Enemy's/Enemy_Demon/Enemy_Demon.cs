using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy_Demon : MonoBehaviour, IEnemyBehaviour
{
    private EnemyController m_enemyController;
    private EnemyDemonController m_enemyDemonController = new();

    [SerializeField] private float probabilityOfTakeStep = 75f;
    [SerializeField] private float nextActionDurationMin = 1f;
    [SerializeField] private float nextActionDurationMax = 5f;
    [Header("Action Parameter")]
    [SerializeField] private float stepPower = 300f;
    [SerializeField] private float zigzagRange = 10f;
    [SerializeField][Range(0, 0.5f)] private float stopTime = 0.5f;


    public void Initialized(EnemyController enemyController)
    {
        m_enemyController = enemyController;
        Rigidbody rb = GetComponent<Rigidbody>();
        m_enemyDemonController.Initialize(enemyController, transform, probabilityOfTakeStep, nextActionDurationMin, nextActionDurationMax, stepPower,
            rb, zigzagRange, stopTime);
    }

    public void Execute()
    {
        m_enemyDemonController.DoDemonStates();
    }

    public void Stop() => m_enemyController.Stop();

}

