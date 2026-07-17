using UnityEngine;

public class Enemy_Mimic : MonoBehaviour, IEnemyBehaviour
{
    private EnemyController m_enemyController;

    private float m_awakeRange = 3f;

    private bool m_isAwakened = false;

    public void Initialized(EnemyController controller) => m_enemyController = controller;
    public void Execute()
    {
        if (m_enemyController.Target == null) return;

        float distance = Vector3.Distance(transform.position,m_enemyController.Target.Value);

        // not started
        if (!m_isAwakened)
        {
            if (distance <= m_awakeRange)
            {
                m_isAwakened = true;

                Debug.Log("Mimic Awaken");
            }
            return;
        }
        if(distance <= m_enemyController.AttackRange)
        {
            m_enemyController.Stop();
            m_enemyController.TryAttack();
            return;
        }
        m_enemyController.SetDestination(m_enemyController.Target.Value,m_enemyController.EvasionSpeed);
    }

    public void Stop()
    {
        if (!m_isAwakened) return;
        m_enemyController.Stop();
    }
}