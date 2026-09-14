using UnityEngine;
using System.Collections;

public class BossEnemy_Golem : MonoBehaviour, IBossBehaviour
{
    [SerializeField]
    private GolemController m_golem;

    private BossEnemyController m_controller;

    private bool m_isStomping;

    public void Initialize(BossEnemyController controller)
    {
        m_controller = controller;
    }

    public void Execute()
    {
        if (m_controller.Target == null) return;

        float distance = Vector3.Distance(transform.position, m_controller.Target.Value);

        if (distance <= m_golem.ShortRange)
        {
            CloseAttack();
            return;
        }
        if (distance <= m_golem.LongRange)
        {
            LongAttack();
            return;
        }

        m_controller.SetDestination(m_controller.Target.Value, m_controller.Speed);
    }

    private void CloseAttack()
    {
        if (m_isStomping) return;

        if (!m_controller.TryUseCooldown()) return;

        StartCoroutine(StompCoroutine());
    }

    private void LongAttack()
    {
        m_controller.Stop();

        if (!m_controller.TryUseCooldown()) return;

        Vector3 dir = (m_controller.Target.Value - transform.position).normalized;

        m_controller.UseItem(dir);
    }

    public void Stop()
    {
        m_controller.Stop();
    }

    //=====================
    private IEnumerator StompCoroutine()
    {
        m_isStomping = true;

        m_controller.Stop();

        Vector3 startPos = transform.position;

        Vector3 jumpPos = startPos + Vector3.up * m_golem.JumpHeight;

        float timer = 0f;

        // Jump
        while (timer < m_golem.JumpDuration)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, jumpPos, timer / m_golem.JumpDuration); 
            yield return null;
        }

        timer = 0f;

        // Fall
        while (timer < m_golem.JumpDuration)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(jumpPos, startPos, timer / m_golem.JumpDuration);
            yield return null;
        }

        CreateShockWave();

        m_isStomping = false;
    }

    private void CreateShockWave()
    {
        if (m_golem.ShockWavePrefab == null) return;

        DamageData damage = 
            new DamageData
            {
                Attack = (int)m_controller.STR,
                CriticalRate = m_controller.CriticalRate,
                CriticalDamage = m_controller.CriticalDamage,
                BreakRate = m_controller.BreakRate,
                Knockback = m_controller.KnockBack,
                Stun = m_controller.Stun,
                AttackDir = transform.forward,
                Attacker = m_controller
            };

        StompShockWave shockWave = Instantiate( m_golem.ShockWavePrefab, transform.position,Quaternion.identity);

        shockWave.Initialize(damage, m_controller.Team, m_golem.ShortRange, m_golem.ShockWaveLifeTime);
    }
}