using System.Collections.Generic;
using UnityEngine;

public class Flame : TrapBase
{
    [SerializeField] private float m_Sustainability = 10f;//持続
    [SerializeField] private float m_Damageinterval = 0.5f;//間隔
    [SerializeField] private float m_Amountdamage = 5f;//量

    private TeamType m_Team;
    private float m_baseValue;

    private float m_timer;
    private float m_tickTimer;

    private List<Entity> m_target = new List<Entity>();

    protected override void EntitySetUp()
    {
        m_timer = 0f;
        m_tickTimer = 0f;
        m_target.Clear();
    }

    protected override void OnHit()
    {

    }

    public void InitFire(Entity owner, TeamType team, float daseValue)
    {
        m_owner = owner;
        m_Team = team;
        m_baseValue = daseValue;

        // Poolから再利用した時のためにリセット
        m_timer = 0f;
        m_tickTimer = 0f;
        m_target.Clear();
    }

    private void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer >= m_Sustainability)
        {
            OnReturnPool();
            return;
        }

        m_tickTimer += Time.deltaTime;

        if (m_tickTimer >= m_Damageinterval)
        {
            m_tickTimer = 0f;
            ApplyFireDamage();
        }
    }

    private void ApplyFireDamage()
    {
        for (int i = m_target.Count - 1; i >= 0; i--)
        {
            Entity target = m_target[i];

            if (target == null)
            {
                m_target.RemoveAt(i);
                continue;
            }

            if (target.Team == m_Team)
            {
                m_target.RemoveAt(i);
                continue;
            }

            DamageData damageDate = new DamageData
            {
                Attack = m_Amountdamage + m_baseValue,
                AttackDir =
                    (target.transform.position - transform.position).normalized
            };

            target.TakeDamage(damageDate);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Entity target =
            other.GetComponentInParent<Entity>();

        if (target == null)
            return;

        if (target.Team == m_Team)
            return;

        if (!m_target.Contains(target))
        {
            m_target.Add(target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity target =
            other.GetComponentInParent<Entity>();

        if (target != null && m_target.Contains(target))
        {
            m_target.Remove(target);
        }
    }
}