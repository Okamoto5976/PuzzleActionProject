using UnityEngine;
using System.Collections;

public class Enemy_Rush : MonoBehaviour, IEnemyBehaviour
{
    private EnemyController m_controller;

    private enum Enum_RushState
    {
        Prepare,
        Rush
    }
    private Enum_RushState m_state;


    private Vector3 m_targetPos;
    private Vector3 m_dir;

    private bool m_hasHit;


    public bool IsRunning => m_state == Enum_RushState.Rush;
    public Vector3 CurrentDirection => m_dir;

    public void Initialized(EnemyController controller)
    {
        m_controller = controller;
    }
    public void Execute()
    {
        float distance = Vector3.Distance(transform.position, m_controller.Target.Value);
        if (distance <= m_controller.FindRange && distance >= m_controller.AttackRange)
        //if (distance <= m_controller.FindRange)
        {
            m_controller.SetDestination(m_controller.Target.Value, m_controller.Speed);
        }
        if (distance > m_controller.FindRange) return;

        switch (m_state)
        {
            case Enum_RushState.Prepare: UpdatePrepare(); break;
            case Enum_RushState.Rush: UpdateRush(); break;
        }
    }
    // ====================Prepare
    private void UpdatePrepare()
    {
        float distance = Vector3.Distance(transform.position, m_controller.Target.Value);
        Vector3 dir = m_controller.Target.Value - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }

        // Cooldown’†
        if (!m_controller.IsCooldownReady)
        {
            if (distance <= m_controller.AttackRange)
            {
                m_controller.Stop();
            }
            return;
        }

        m_controller.ConsumeCooldown();

        m_hasHit = false;

        m_targetPos = m_controller.Target.Value;

        m_dir = m_targetPos - transform.position;
        m_dir.y = 0f;
        m_dir.Normalize();

        m_state = Enum_RushState.Rush;
    }

    // ==================Rush
    private void UpdateRush()
    {
        if (m_controller.IsStun) return;
        if (m_controller.IsKnockBack) return;

        transform.rotation = Quaternion.LookRotation(m_dir);

        m_controller.InputMove(m_dir, m_controller.Speed * 1.5f);

        float distanceToPlayer =Vector3.Distance(transform.position, m_controller.Target.Value);
        if (distanceToPlayer <= m_controller.AttackRange)
        {
            if (!m_hasHit)
            {
                m_controller.Attack();
                m_hasHit = true;
                Stop();
                return;
            }
        }

        Vector3 toTarget = m_targetPos - transform.position;
        if (Vector3.Dot(toTarget, m_dir) <=0)
        {
            Stop();
        }
    }
    public void Stop()
    {
        m_controller.Stop();
        m_state = Enum_RushState.Prepare;
    }
}