using Unity.XR.OpenVR;
using UnityEngine;

public class Enemy_Wizard : MonoBehaviour, IEnemyBehaviour
{
    private EnemyController m_enemy;

    [Header("Move")]
    [SerializeField, Range(0, 1)] private float m_reverseRate = 0.3f;
    private bool m_isReverse;

    public void Initialized(EnemyController enemyController)
    {
        m_enemy = enemyController;
        UpdateOrbitDirection();
    }

    public void Execute()
    {
        if (m_enemy.Target == null) return;

        float distance = Vector3.Distance(transform.position, m_enemy.Target.Value);
        if (distance > m_enemy.AttackRange)
        {
            m_enemy.SetDestination(m_enemy.Target.Value, m_enemy.Speed);
        }
        else
        {
            Orbit();
            Shoot();
        }
    }

    public void Stop()
    {
        m_enemy.Stop();
    }

    private void UpdateOrbitDirection()
    {
        m_isReverse = Random.value < m_reverseRate;
    }

    private void Orbit()
    {
        Vector3 center = m_enemy.Target.Value;
        Vector3 offset = transform.position - center;
        offset.y = 0f;

        //if (offset.sqrMagnitude < 0.01f)
        //{
        //    offset = transform.right;
        //}

        Vector3 tangent = Vector3.Cross(Vector3.up, offset.normalized);

        if (m_isReverse)
        {
            tangent *= -1f;
        }

        Vector3 targetPos = center + offset.normalized * m_enemy.AttackRange + tangent * m_enemy.Speed;
        m_enemy.SetDestination(targetPos, m_enemy.Speed);
    }

    private void Shoot()
    {
        if (!m_enemy.IsCooldownReady) return;

        Vector3 dir = (m_enemy.Target.Value - transform.position) .normalized;
        m_enemy.UseItem(dir);
        m_enemy.ConsumeCooldown();

        UpdateOrbitDirection();
    }

    //private void Chase()
    //{
    //    m_enemy.SetDestination(m_enemy.Target.Value, m_enemy.Speed);
    //}
}