using UnityEngine;
using System.Collections.Generic;

public class BuffInstance
{
    public float m_duration;

    public EntityStatus m_status;//Entity‚ÌStatus

    public StatusModifier m_modifier;
}

public class EntityBuffSystem : MonoBehaviour
{
    private Entity m_Entity;

    private List<BuffInstance> m_buffs;

    private void Awake()
    {
        m_Entity = GetComponent<Entity>();
    }

    public void AddBuff(StatusModifier modifier, float duration)
    {
        //EntityStatus status = m_Entity.GetStatus(modifier.m_statType);

        BuffInstance instance = new BuffInstance
        {
            m_duration = duration,
            //m_status = status,
            m_modifier = modifier,
        };

        m_buffs.Add(instance);

        //status.AddModifier(modifier)
    }

    private void Update()
    {
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

    public void RemoveBuff(BuffInstance buff)
    {
        //buff.m_status.RemoveModifier(buff.m_modifier);
    }
}
