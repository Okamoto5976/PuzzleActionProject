using System.Collections.Generic;
using UnityEngine;

public enum Passive
{
    PriceDown,
    Glasses,
    SpeedShoes,
    SlowResShoes,
    WinnerTrophy,
    LoserTrophy,
}

//public class PassiveModifier
//{
//    public Passive m_passive;
//}

public class PassiveBuffInstance
{
    public EntityStatus m_status;//Entity‚ÌStatus

    public List<StatusModifier> m_modifier;

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

        //foreach (var modifier in modifiers)
        //{
        //    EntityStatus status = m_player.GetStatus(modifier.m_statType);
        //}

        //PassiveBuffInstance instance = new PassiveBuffInstance
        //{
        //    m_status = status,
        //    m_modifier = modifiers,
        //    m_passiveType = passiveType,
        //};

        //m_passives.Add(instance);

        //status.AddModifier(modifier);




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

            //instance.m_status.RemoveModifier(instance.m_modifier);
        }
    }
}
