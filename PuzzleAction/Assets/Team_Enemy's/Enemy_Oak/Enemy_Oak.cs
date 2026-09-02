using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Enemy_Oak : MonoBehaviour, IEnemyBehaviour
{
    private EnemyController m_enemyController;

    public void Initialized(EnemyController enemyController) => m_enemyController = enemyController;

    public void Execute()
    {
        if (m_enemyController.Target == null) return;
        float distance = Vector3.Distance(transform.position, m_enemyController.Target.Value);
        if (distance <= m_enemyController.AttackRange)
        {
            m_enemyController.Stop();

            Vector3 dir = m_enemyController.Target.Value - transform.position;
            dir.y = 0f;
            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(dir);
            }

            if (m_enemyController.TryUseCooldown())
            {
                m_enemyController.UseItem(dir.normalized);
            }
            return;
        }
        m_enemyController.SetDestination(m_enemyController.Target.Value, m_enemyController.Speed);
    }

    public void Stop() => m_enemyController.Stop();
}
