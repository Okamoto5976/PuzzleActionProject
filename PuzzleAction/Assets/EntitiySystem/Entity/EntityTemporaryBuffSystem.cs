using UnityEngine;
using System.Collections.Generic;

public class TemporaryBuffInstance
{
    public float m_duration;

    public EntityStatus m_status;//Entity‚ÌStatus

    public StatusModifier m_modifier;

    public BuffID m_buffID;
}

public class EntityTemporaryBuffSystem : MonoBehaviour
{
    private Entity m_Entity;

    private List<TemporaryBuffInstance> m_buffs = new();

    private void Awake()
    {
        m_Entity = GetComponent<Entity>();
    }

    public void AddBuff(StatusModifier modifier, BuffID buffID, float duration)
    {
        TemporaryBuffInstance existing = m_buffs.Find(x => x.m_buffID == buffID);
        
        if(existing != null)
        {
            if(existing.m_modifier.m_value < modifier.m_value)
            {
                existing.m_modifier.m_value = modifier.m_value;
            }
            
            if(existing.m_duration < duration)
            {
                existing.m_duration = duration;

            }


            return;
        }
        

        EntityStatus status = m_Entity.GetStatus(modifier.m_statType);

        TemporaryBuffInstance instance = new TemporaryBuffInstance
        {
            m_duration = duration,
            m_status = status,
            m_modifier = modifier,
            m_buffID = buffID,
        };

        m_buffs.Add(instance);

        status.AddModifier(modifier);
    }

    private void Update()
    {
        if (m_buffs == null) return;

        for(int i = m_buffs.Count - 1; i >= 0; i--)
        {
            var buff = m_buffs[i];

            buff.m_duration -= Time.deltaTime;

            if(buff.m_duration <= 0)
            {
                RemoveBuff(buff);

                m_buffs.RemoveAt(i);
            }
        }
    }

    public void RemoveBuff(TemporaryBuffInstance buff)
    {
        buff.m_status.RemoveModifier(buff.m_modifier);
    }
}

