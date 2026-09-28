using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BuffItem", menuName = "Scriptable Objects/Datas/BuffItem")]

public class BuffItem : Item 
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

    [Header("Buff Reference")]
    [SerializeField] private ItemType m_buffEffectType;

    [SerializeField] private List<BuffItemClass> m_buffItemClass = new();

    //[SerializeField] private BuffType m_buffType;


    //public List<Item> m_buffData = new List<Item>();

    //passive use----------------
    private List<StatusModifier> m_modifiers = new();

    [SerializeField] private Passive m_passiveType;

    public override void Activation(ItemRecieveData data)
    {
        if (m_buffEffectType == ItemType.Active)
        {

            foreach(var buff in m_buffItemClass)
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

            //Debug.Log("buff add to entity");
        }
    }

    public override void AddPassive(PlayerController player)
    {
        Debug.LogWarning("AddPassive in item");

        if(m_passiveType == Passive.WinnerTrophy)
        {
            player.m_isWinnerTrophy = true;
        }
        else if(m_passiveType == Passive.LoserTrophy)
        {
            player.m_isLoserTrophy = true;
        }
        else if(m_passiveType == Passive.Trophy)
        {
            player.m_isTrophy = true;
        }


        if (m_buffEffectType == ItemType.Passive)
        {
            foreach (var buff in m_buffItemClass)
            {
                StatusModifier modifier = new StatusModifier()
                {
                    m_statType = buff.m_statusType,
                    m_value = buff.m_value,
                    m_modType = buff.m_modifierType
                };


                //Debug.Log(modifier.m_statType);

                //Debug.Log(modifier.m_value);


                m_modifiers.Add(modifier);

            }

            player.AddPassive(m_modifiers, m_passiveType);
            m_modifiers.Clear();
        }
    }

    public override void RemovePassive(PlayerController player)
    {
        if (m_passiveType == Passive.WinnerTrophy)
        {
            player.m_isWinnerTrophy = false;
        }
        else if (m_passiveType == Passive.LoserTrophy)
        {
            player.m_isLoserTrophy = false;
        }
        else if (m_passiveType == Passive.Trophy)
        {
            player.m_isTrophy = false;
        }

        player.RemovePassive(m_passiveType);
    }
}
