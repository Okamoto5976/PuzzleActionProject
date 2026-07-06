using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BuffItem", menuName = "Scriptable Objects/Datas/BuffItem")]

public class BuffItem : Item 
{
    
    public enum BuffType
    {
        AttackUp, 
        DefenseUp, 
        SpeedUp,
        AttackDown, 
        DefenseDown, 
        SpeedDown, 
    }

    [Header("Buff Reference")]
    
    [SerializeField] private float m_buffDuration;
    [SerializeField] private BuffType m_buffType;
    [SerializeField] private ItemType m_buffEffectType;

    [SerializeField] private StatusType m_statusType;
    [SerializeField] private ModifierType m_modifierType;

    //public List<Item> m_buffData = new List<Item>();

    public override void Activation(float value, ItemRecieveData data)
    {
        if (m_buffEffectType == ItemType.Active)
        {

            if (m_buffDuration <= 0)
            {

                return;
            }

            //m_buffData.Add(this);
            StatusModifier modifier = new StatusModifier()
            {
                m_statType = m_statusType,
                m_value = value,
                m_modType = m_modifierType
            };

            Debug.Log("buff add to entity");
            data.entity.AddBuff(modifier, m_buffDuration);
        }
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