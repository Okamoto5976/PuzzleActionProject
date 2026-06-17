using UnityEngine;

public class Enemy_Archer : MonoBehaviour, IEnemyBehaviour
{
    private EnemyController m_controller;

    private float m_coolTime = 1.5f;
    private float m_lastFireTime;

    public void Initialized(EnemyController controller)
    {
        m_controller = controller;
    }

    public void Execute()
    {
        if (m_controller.Target == null) return;

        float distance = Vector3.Distance(
            transform.position,
            m_controller.Target.Value
        );

        if (distance <= m_controller.AttackRange)
        {
            m_controller.Stop();

            Vector3 dir = (m_controller.Target.Value - transform.position);
            dir.y = 0f;

            transform.rotation = Quaternion.LookRotation(dir);

            TryShoot(dir.normalized);
            Debug.Log("Shot!!");
        }
        else
        {
            m_controller.SetDestination(
                m_controller.Target.Value,
                m_controller.Speed
            );
        }
    }

    private void TryShoot(Vector3 dir)
    {
        if (Time.time < m_lastFireTime + m_coolTime)
            return;

        m_lastFireTime = Time.time;

        m_controller.UseItem(dir);
    }

    public void Stop()
    {
        m_controller.Stop();
    }
}