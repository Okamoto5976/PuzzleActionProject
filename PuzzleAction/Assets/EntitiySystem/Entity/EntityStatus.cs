using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModifierType
{
    Add,
    Multiply
}

public class StatusModifier
{
    public StatusType m_statType;

    public float m_value;
    public ModifierType m_modType;

    public object m_source;
}

public class EntityStatus
{
    private float m_baseValue;

    private List<StatusModifier> m_modifiers = new();

    public float Value
    {
        get
        {
            float add = 0;
            float multiply = 1;

            foreach(var modifier in m_modifiers)
            {
                switch(modifier.m_modType)
                {
                    case ModifierType.Add:
                        add += modifier.m_value;
                        break;
                    case ModifierType.Multiply:
                        multiply += modifier.m_value;
                        break;
                }
            }

            return (m_baseValue + add) * multiply;
        }
    }

    public EntityStatus(float baseValue)
    {
        m_baseValue = baseValue;
    }

    public void AddModifier(StatusModifier modifier)
    {
        m_modifiers.Add(modifier);
    }

    public void RemoveModifier(StatusModifier modifier)
    {
        m_modifiers.Remove(modifier);
    }

    public void RemoveModifierFromSource(object source)
    {
        m_modifiers.RemoveAll(x => x.m_source == source);
    }
}


