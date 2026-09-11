using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flame : TrapBase
{
    protected override void SetUp()
    {
    }
    protected override void OnHit()
    {
    }
    [SerializeField] private float m_Sustainability = 10f;//持続
    [SerializeField] private float m_Damageinterval = 0.5f;//間隔
    [SerializeField] private float m_Amountdamage = 5f;//量

    private Entity m_owner;
    private TeamType m_Team;
    private float m_baseValue;

    private float m_timer;
    private float m_tickTimer;
    private List<Entity>m_target=new List<Entity>();

    public void InitFire(Entity owner,TeamType team,float daseValue)
    {
        m_owner = owner;
        m_Team= team;
        m_baseValue = daseValue;
    }

    private void Update()
    {
        m_timer += Time.deltaTime;
        if(m_timer >= m_Sustainability)
        {
            //pool アイテムpoolでいいのか？
            OnReturnPool();
            return;
        }

        m_tickTimer += Time.deltaTime;
        if( m_tickTimer >= m_Damageinterval)
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
            DamageData damageDate = new DamageData
            {
                Attack = m_Amountdamage + m_baseValue,
                Attacker = m_owner,
                AttackDir = (target.transform.position - transform.position).normalized
            };
        target.TakeDamage(damageDate);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Entity target = other.GetComponent<Entity>();
        if (target == null) return;
        if (target.Team == m_team) return;

        if (!m_target.Contains(target))
        {
            m_target.Add(target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity target = other.GetComponent<Entity>();
        if (target != null && m_target.Contains(target))
        {
            m_target.Remove(target);
        }
    }

}
