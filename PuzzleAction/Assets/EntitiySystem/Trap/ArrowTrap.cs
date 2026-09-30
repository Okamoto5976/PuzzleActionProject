using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : TrapBase
{
    [SerializeField] private float m_damageRate = 1f;

    [SerializeField] private LayerMask m_hitLayers;

    private bool m_isInitialized;

    private void FixedUpdate()
    {
        if (!m_isInitialized)
            return;

        OnAddForce(m_dir, m_power);

        m_isInitialized = false;
    }

    protected override void EntitySetUp()
    {
        m_rb.linearVelocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;
        m_isInitialized = true;

        CreateDamageData();
    }

    protected override void CreateDamageData()
    {
        base.CreateDamageData();
    }

    protected override void OnHit()
    {
        OnReturnPool();
    }

    protected override void OnTriggerEnter(
        Collider other)
    {
        if ((m_hitLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            OnHit();
            return;
        }

        Entity target = other.GetComponentInParent<Entity>();

        if (target == null) return;

        if (target.Team == TeamType.Nature) return;

        if (target.Team == m_team) return;

        target.TakeDamage(m_damageData);

        if(m_trapData.m_buffSetting.Count != 0)
        {
            foreach (var buff in m_trapData.m_buffSetting)
            {
                if (buff.m_duration <= 0) continue;

                var modifier = SetModifier(buff);

                target.AddBuff(modifier, buff.m_buffID, buff.m_duration);

            }
        }

        OnHit();
    }
}
