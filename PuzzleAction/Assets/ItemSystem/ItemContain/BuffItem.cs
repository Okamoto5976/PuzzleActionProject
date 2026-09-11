using Mono.Cecil;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BuffItem", menuName = "Scriptable Objects/Datas/BuffItem")]

public class BuffItem : Item 
{

    //public enum BuffType
    //{
    //    AttackUp, 
    //    DefenseUp, 
    //    SpeedUp,
    //    AttackDown, 
    //    DefenseDown, 
    //    SpeedDown, 
    //}

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

        if(m_buffEffectType == ItemType.Passive)
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
        player.RemovePassive(m_passiveType);
    }
}

//entity.BuffSet(buffType, value, buffDuration); //Entity��BuffSet��buffType��value��buffDuration��n��
//switch (m_buffType)
//{
//    case BuffType.AttackUp:
//        Buffdata.Add(this);
//        StatusModifier modifier = new StatusModifier()
//        {
//            m_statType = StatusType.Strength,
//            m_value = value,
//            m_modType = ModifierType.Add
//        };

//        data.entity.AddBuff(modifier, m_buffDuration);

//        break;
//    case BuffType.DefenseUp:
//        ////�h��͏㏸�̏���
//        ////data.entity.BuffSet(value);
//        //Buffdata.Add(this); //Buffdata�ɂ���BuffItem��ǉ�
//        //statusModifier = new()
//        //{
//        //    m_statType = StatusType.Defense,
//        //    m_value = value,
//        //    m_modType = ModifierType.Add
//        //};
//        break;
//    case BuffType.SpeedUp:
//        ////���x�㏸�̏���
//        ////data.entity.BuffSet(value);
//        //Buffdata.Add(this); //Buffdata�ɂ���BuffItem��ǉ�
//        //statusModifier = new()
//        //{
//        //    m_statType = StatusType.Speed,
//        //    m_value = value,
//        //    m_modType = ModifierType.Add
//        //};
//        break;
//    case BuffType.AttackDown:
//        ////�U���͌����̏���
//        ////data.entity.BuffSet(value);
//        //Buffdata.Add(this);
//        //statusModifier = new()
//        //{
//        //    m_statType = StatusType.Strength,
//        //    m_value = value,
//        //    m_modType = ModifierType.Add
//        //};
//        break;
//    case BuffType.DefenseDown:
//        ////�h��͌����̏���
//        ////data.entity.BuffSet(value);
//        //Buffdata.Add(this);
//        //statusModifier = new()
//        //{
//        //    m_statType = StatusType.Defense,
//        //    m_value = value,
//        //    m_modType = ModifierType.Add
//        //};
//        break;
//    case BuffType.SpeedDown:
//        ////���x�����̏���
//        ////data.entity.BuffSet(value);
//        //Buffdata.Add(this);
//        //statusModifier = new()
//        //{
//        //    m_statType = StatusType.Speed,
//        //    m_value = value,
//        //    m_modType = ModifierType.Add
//        //};
//        break;
//    default:
//        break;
//}
//data.entity.AddBuff(statusModifier, buffDuration);

//while (m_buffDuration > 0) //buffDuration��0�ɂȂ�܂Ń��[�v
//{
//    m_buffDuration = Time.deltaTime; //buffDuration�����炷
//                                     //�o�t�̌��ʂ��ێ����鏈��
//    if (m_buffDuration <= 0) //buffDuration��0�ȉ��ɂȂ����烋�[�v�𔲂���
//    {
//        //    data.entity.BaseValueReset(value); //Entity��BaseValue�����ɖ߂�����
//        Buffdata.Remove(this); //Buffdata���炱��BuffItem���폜
//    }

//}