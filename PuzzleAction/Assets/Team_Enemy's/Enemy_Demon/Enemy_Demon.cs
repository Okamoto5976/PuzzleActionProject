using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy_Demon : MonoBehaviour, IEnemyBehaviour
{
    private EnemyController m_enemyController;
    private EnemyDemonController m_enemyDemonController = new();
    private Rigidbody rb;

    [SerializeField] private float probabilityOfTakeStep = 75f;
    [SerializeField] private float nextActionDurationMin = 1f;
    [SerializeField] private float nextActionDurationMax = 5f;
    [Header("Action Parameter")]
    [SerializeField] private float stepPower = 300f;
    [SerializeField] private float stepTime = 0.9f;
    [SerializeField] private float waitTimeAfterStep = 0.2f;
    [SerializeField] private float zigzagRange = 12f;
    [SerializeField][Range(0, 1f)] private float stopTime = 0.5f;


    public void Initialized(EnemyController enemyController)
    {
        m_enemyController = enemyController;
        rb = GetComponent<Rigidbody>();
        m_enemyDemonController.Initialize(enemyController, transform, probabilityOfTakeStep, nextActionDurationMin, nextActionDurationMax, stepPower,
            rb, zigzagRange, stopTime, stepTime, waitTimeAfterStep);
    }

    public void Execute()
    {
        m_enemyDemonController.DoDemonStates();
    }

    public void Stop()
    {
        m_enemyController.Stop();
        rb.linearVelocity = Vector3.zero;
    }

}

