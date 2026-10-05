using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackItem", menuName = "Scriptable Objects/Datas/AttackItem")]
public class AttackItem : Item
{
    //[SerializeField] private bool m_overrideDamage;
    [SerializeField] private DamageData m_damage = new();


    [SerializeField] private bool m_isRayCollider;

    private HitCollider m_hitCollider = new(true);

    private RayCollider m_rayCollider = new(true);
    [SerializeField] private float m_range;

    [System.Serializable]
    public class BuffModify
    {
        public float m_value;
        public StatusType m_statusType;//what status? HP, Strength
        public ModifierType m_modifierType;//what mod? Add, Multiply

        [Header("----Active Buff Setting ----")]
        public float m_duration;
        public BuffID m_buffID;


    }

    [SerializeField] private List<BuffModify> m_selfBuffList = new();

    [SerializeField] private List<BuffModify> m_targetBuffList = new();


    public override void Activation(ItemRecieveData data)
    {
        List<Collider> hits = new();

        if (m_damage.Attack < 0f)
        {
            m_damage.Attack += data.entity.STR;
        }

        if (m_damage.CriticalDamage < 0f)
        {
            m_damage.CriticalDamage += data.entity.CriticalDamage;

        }

        if(m_damage.CriticalRate < 0f)
        {
            m_damage.CriticalRate += data.entity.CriticalRate;

        }

        if (m_damage.Knockback < 0f)
        {
            m_damage.Knockback += data.entity.KnockBack;
        }

        if (m_damage.BreakRate < 0f)
        {
            m_damage.BreakRate += data.entity.BreakRate;
        }

        if (m_damage.StunDuration < 0f)
        {
            m_damage.StunDuration += data.entity.StunPower;
        }

        if (m_isRayCollider)
        {
            AttackRay collider = new()
            {
                origin = data.pos,
                direction = data.dir,
                range = m_range,
                maxPenetrate = 1,
            };

            hits = m_rayCollider.AttackCollider(m_damage, data.entity.Team, collider);

            DebugViewCollider.Instance.ViewRayCollider(collider);
        }
        else
        {
            AttackHitBox hitbox = new()
            {
                m_pos = data.pos,
                m_hitBoxOffset = data.offset,
                m_radius = m_range
            };

            hits = m_hitCollider.AttackCollider(m_damage, data.entity.Team, hitbox);

            DebugViewCollider.Instance.ViewHitCollider(hitbox);
        }

        //if (m_overrideDamage)
        //{
        //    m_damage.Attack += data.entity.STR;

        //}
        Debug.Log("Attack Item");

        if (m_se != null)
        {
            AudioManager.Instance.PlayAudio(m_se);

        }

        foreach (Collider hit in hits)
        {
            Debug.Log("Collider");


            Entity entity = hit.GetComponentInParent<Entity>();
            if (entity == null)
            {
                continue;
            }

            if (entity.Team == data.entity.Team) continue;

            foreach (var buff in m_selfBuffList)
            {
                if (buff.m_duration <= 0) continue;

                StatusModifier modifier = new StatusModifier()
                {
                    m_statType = buff.m_statusType,
                    m_value = buff.m_value,
                    m_modType = buff.m_modifierType
                };

                data.entity.AddBuff(modifier, buff.m_buffID, buff.m_duration);

            }

            foreach (var buff in m_targetBuffList)
            {
                if (buff.m_duration <= 0) continue;

                StatusModifier modifier = new StatusModifier()
                {
                    m_statType = buff.m_statusType,
                    m_value = buff.m_value,
                    m_modType = buff.m_modifierType
                };

                entity.AddBuff(modifier, buff.m_buffID, buff.m_duration);

            }

            entity.TakeDamage(m_damage);
        }


        //Debug.Log($"AttackItem‚ðŽg—p‚µ‚Ü‚µ‚½");
    }
}
