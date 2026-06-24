using Mono.Cecil;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BuffItem", menuName = "Scriptable Objects/Datas/BuffItem")]
public class BuffItem : Item 
{
    //Entity entity; //Entity��錾
    public enum BuffType
    {
        AttackUp, //�U���͏㏸
        DefenseUp, //�h��͏㏸
        SpeedUp, //���x�㏸
        AttackDown, //�U���͌���
        DefenseDown, //�h��͌���
        SpeedDown, //���x����
        // ���̃o�t�̎�ނ�ǉ�
    }
    public enum BUffEffectType 
    {
        active, //�g�p�����Ƃ��Ɍ��ʂ���������^�C�v
        passive, //��Ɍ��ʂ��������Ă���^�C�v
    }
    public enum BuffTimeType
    {
        Temporary, //�ꎞ�I�ȃo�t
        Permanent  //�i���I�ȃo�t
    }
    
    [SerializeField] private float buffDuration; //�o�t���ʎ���
    [SerializeField] private BuffType buffType; //�o�t�̎��
    [SerializeField] private BUffEffectType buffEffectType; //�o�t�̌��ʃ^�C�v
    //Entity entity;
    public List <Item> Buffdata = new List<Item>();
    public override void Activation(float value, ItemRecieveData data)
    {
        //�ȉ��͌��ʎ��ԃA�C�e���̘b
        //�󂯂Ƃ���Entity�ɗ^���鏈��
        //data.Entity.BuffSet���Ă����̂�p�ӂ���Ă���
        //������buffType��m_value��^����΂悢
        //modifier�ɂ܂Ƃ߂�im_value��buffType������j
        //���ʎ��Ԃ̂�����̂�duration���K�v
        //modifiers = new EventModifiers(buffType, value, buffDuration); //EventModifiers��buffType��value��buffDuration��n��
        //Entity entity = data.entity; //Entity��data����擾
        //StatusModifier statusModifier;
        //�o�t����
        if (buffEffectType == BUffEffectType.active)
        {

            if (buffDuration <= 0) //buffDuration��0�ȉ��̏ꍇ�͏����𔲂���
            {
                Debug.LogWarning("�o�t�̌��ʎ��Ԃ��ݒ肳��Ă��܂���");
                
                return;
            }
            //entity.BuffSet(buffType, value, buffDuration); //Entity��BuffSet��buffType��value��buffDuration��n��
            switch (buffType)
            {
                case BuffType.AttackUp:
                    ////�U���͏㏸�̏���
                    ////data.entity.BuffSet(value); //�U���͂��㏸�����鏈��
                    //Buffdata.Add(this); //Buffdata�ɂ���BuffItem��ǉ�
                    //statusModifier = new()    // ���ʂ̏ڍׂ�ݒ�
                    //{
                    //    m_statType = StatusType.Strength,
                    //    m_value = value,
                    //    m_modType = ModifierType.Add
                    //};
                    break;
                case BuffType.DefenseUp:
                    ////�h��͏㏸�̏���
                    ////data.entity.BuffSet(value);
                    //Buffdata.Add(this); //Buffdata�ɂ���BuffItem��ǉ�
                    //statusModifier = new()
                    //{
                    //    m_statType = StatusType.Defense,
                    //    m_value = value,
                    //    m_modType = ModifierType.Add
                    //};
                    break;
                case BuffType.SpeedUp:
                    ////���x�㏸�̏���
                    ////data.entity.BuffSet(value);
                    //Buffdata.Add(this); //Buffdata�ɂ���BuffItem��ǉ�
                    //statusModifier = new()
                    //{
                    //    m_statType = StatusType.Speed,
                    //    m_value = value,
                    //    m_modType = ModifierType.Add
                    //};
                    break;
                case BuffType.AttackDown:
                    ////�U���͌����̏���
                    ////data.entity.BuffSet(value);
                    //Buffdata.Add(this);
                    //statusModifier = new()
                    //{
                    //    m_statType = StatusType.Strength,
                    //    m_value = value,
                    //    m_modType = ModifierType.Add
                    //};
                    break;
                case BuffType.DefenseDown:
                    ////�h��͌����̏���
                    ////data.entity.BuffSet(value);
                    //Buffdata.Add(this);
                    //statusModifier = new()
                    //{
                    //    m_statType = StatusType.Defense,
                    //    m_value = value,
                    //    m_modType = ModifierType.Add
                    //};
                    break;
                case BuffType.SpeedDown:
                    ////���x�����̏���
                    ////data.entity.BuffSet(value);
                    //Buffdata.Add(this);
                    //statusModifier = new()
                    //{
                    //    m_statType = StatusType.Speed,
                    //    m_value = value,
                    //    m_modType = ModifierType.Add
                    //};
                    break;
                default:
                    //���̑��̃o�t�̏���
                    Debug.LogWarning("����`�̃o�t�^�C�v�ł�");
                    break;
            }
            //data.entity.AddBuff(statusModifier, buffDuration);

            while (buffDuration > 0) //buffDuration��0�ɂȂ�܂Ń��[�v
            {
                buffDuration = Time.deltaTime; //buffDuration�����炷
                                               //�o�t�̌��ʂ��ێ����鏈��
                if ( buffDuration <= 0) //buffDuration��0�ȉ��ɂȂ����烋�[�v�𔲂���
                {
                //    data.entity.BaseValueReset(value); //Entity��BaseValue�����ɖ߂�����
                    Buffdata.Remove(this); //Buffdata���炱��BuffItem���폜
                }

            }
            //if (HP == 0)
            //{
            //    //HP��0�ɂȂ����Ƃ��̏���
            //    data.Entity.BuffSet();
            //    BaseValueReset(entity); //Entity��BaseValue�����ɖ߂�����
            //}

        }
        //else if(buffEffectType == BUffEffectType.passive)
        //{
        //    //�p�b�V�u�̏���
        //     entity = data.entity; //Entity��data����擾
        //    entity.BuffSet(buffType, value, buffDuration); //Entity��BuffSet��buffType��value��buffDuration��n��
        //     switch (buffType)
        //    {
        //        case BuffType.AttackUp:
        //            //�U���͏㏸�̏���

        //            break;
        //        case BuffType.DefenseUp:
        //            //�h��͏㏸�̏���

        //            break;
        //        case BuffType.SpeedUp:
        //            //���x�㏸�̏���
        //            break;
        //        case BuffType.AttackDown:
        //            //�U���͌����̏���
        //            break;
        //        case BuffType.DefenseDown:
        //            //�h��͌����̏���
        //            break;
        //        case BuffType.SpeedDown:
        //            //���x�����̏���
        //            break;
        //        default:
        //            //���̑��̃o�t�̏���
        //            Debug.LogWarning("����`�̃o�t�^�C�v�ł�");
        //        break;
        //    }

    }
        //�ȉ��̓p�b�V�u�̘b
        //�p�b�V�u����Entity�̃o�t�̒ǉ��Ɖ������s��
       

        //�p�b�V�u�A�C�e���͌��


        //Debug.Log($"�g�p����{buffDuration}�b�ԁA{buffType}��{value}�㏸����");
       
    //}

    //private void BaseValueReset(Entity entity)
    //{
    //    throw new NotImplementedException();
    //}
}

