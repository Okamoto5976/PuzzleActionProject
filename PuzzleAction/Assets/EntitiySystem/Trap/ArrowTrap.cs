using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : TrapBase
{
    [System.Serializable]
    public class BuffItemClass
    {
        public float m_value;
        public StatusType m_statusType;//what status? HP, Strength
        public ModifierType m_modifierType;//what mod? Add, Multiply

        [Header("----Active Buff Setting ----")]
        public float m_duration;
        public BuffID m_buffID;
    }

    [Header("BuffSetting")]

    [SerializeField] private List<BuffItemClass> m_buffItemClass = new();

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

    private StatusModifier SetModifier(BuffItemClass buff)
    {
        StatusModifier modifier = new StatusModifier()
        {
            m_statType = buff.m_statusType,
            m_value = buff.m_value,
            m_modType = buff.m_modifierType,
        };

        return modifier;
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

        if(m_buffItemClass.Count != 0)
        {
            foreach (var buff in m_buffItemClass)
            {
                if (buff.m_duration <= 0) continue;

                var modifier = SetModifier(buff);

                target.AddBuff(modifier, buff.m_buffID, buff.m_duration);

            }
        }

        OnHit();
    }
}
