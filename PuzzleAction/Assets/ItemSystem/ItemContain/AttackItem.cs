using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AttackItem", menuName = "Scriptable Objects/Datas/AttackItem")]
public class AttackItem : Item
{
    [SerializeField] private bool m_overrideDamage;
    [SerializeField] private DamageData m_damage = new();

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
        Collider[] hits = Physics.OverlapSphere(
            data.pos + data.offset,
            data.power
            );

        if(m_overrideDamage)
        {
            m_damage.Attack += data.entity.STR;
            m_damage.BreakRate += data.entity.BreakRate;
            m_damage.CriticalRate += data.entity.CriticalRate;
            m_damage.CriticalDamage += data.entity.CriticalDamage;
        }

        

        foreach (Collider hit in hits)
        {
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
