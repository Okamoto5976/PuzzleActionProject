using System.Collections.Generic;
using UnityEngine;

public enum Passive
{
    None,
    PriceDown,
    Glasses,
    SpeedShoes,
    SlowResShoes,
    WinnerTrophy,
    LoserTrophy,
}

public class PassiveStatus
{
    public EntityStatus m_status;

    public StatusModifier m_modifier;
}

public class PassiveBuffInstance
{
    public List<PassiveStatus> m_modifier = new();

    public Passive m_passiveType;
}

public class EntityPassiveBuffSystem : MonoBehaviour
{
    private PlayerController m_player;

    private List<PassiveBuffInstance> m_passives = new();

    private void Awake()
    {
        m_player = GetComponent<PlayerController>();
    }


    //Item effect in modifiers list when add passive
    //if you want delete passive, passiveType found from list
    public void AddPassive(List<StatusModifier> modifiers, Passive passiveType)
    {
        //PassiveBuffInstance existing = m_buffs.Find(x => x.m_buffID == passiveID);

        //if (existing != null)
        //{
        //    if (existing.m_modifier.m_value < modifier.m_value)
        //    {
        //        existing.m_modifier.m_value = modifier.m_value;
        //    }

        //    if (existing.m_duration < duration)
        //    {
        //        existing.m_duration = duration;

        //    }


        //    return;
        //}

        PassiveBuffInstance instance = new();
        instance.m_passiveType = passiveType;


        foreach (var modifier in modifiers)
        {
            EntityStatus status = m_player.GetStatus(modifier.m_statType);

            PassiveStatus passiveStatus = new();

            passiveStatus.m_status = status;
            passiveStatus.m_modifier = modifier;

            instance.m_modifier.Add(passiveStatus);
        }

        

        m_passives.Add(instance);

        foreach(var modifier in instance.m_modifier)
        {
            modifier.m_status.AddModifier(modifier.m_modifier);
        }

    }

    //private void Update()
    //{
    //    if (m_buffs == null) return;

    //    for (int i = m_buffs.Count - 1; i >= 0; i--)
    //    {
    //        var buff = m_buffs[i];

    //        buff.m_duration -= Time.deltaTime;

    //        if (buff.m_duration <= 0)
    //        {
    //            RemoveBuff(buff);

    //            m_buffs.RemoveAt(i);
    //        }
    //    }
    //}

    public void RemoveBuff(Passive passivetype)
    {
        //buff.m_status.RemoveModifier(buff.m_modifier);
        
        PassiveBuffInstance instance = m_passives.Find(x => x != null && x.m_passiveType == passivetype);

        if (instance != null)
        {
            m_passives.Remove(instance);

            foreach(var modifier in instance.m_modifier)
            {
                modifier.m_status.RemoveModifier(modifier.m_modifier);
            }
        }
    }
}
