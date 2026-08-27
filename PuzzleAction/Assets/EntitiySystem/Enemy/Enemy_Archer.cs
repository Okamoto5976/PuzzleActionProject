using UnityEngine;

public class Enemy_Archer : MonoBehaviour, IEnemyBehaviour
{
    private EnemyController m_controller;

    public void Initialized(EnemyController controller) => m_controller = controller;
    public void Execute()
    {
        if (m_controller.Target == null) return;

        float distance = Vector3.Distance(transform.position, m_controller.Target.Value);

        if (distance <= m_controller.AttackRange)
        {
            m_controller.Stop();

            Vector3 dir = m_controller.Target.Value - transform.position;
            dir.y = 0f;
            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(dir);
            }

            if (m_controller.TryUseCooldown())
            {
                m_controller.UseItem(dir.normalized);
            }
            return;
        }

        m_controller.SetDestination(m_controller.Target.Value, m_controller.Speed);
    }

    public void Stop()=> m_controller.Stop();
}