using System;
using UnityEngine;
[CreateAssetMenu(fileName = "BuffItem", menuName = "Scriptable Objects/Datas/BuffItem")]
public class BuffItem : Item 
{
    //Entity entity; //Entityを宣言
    public enum BuffType
    {
        AttackUp, //攻撃力上昇
        DefenseUp, //防御力上昇
        SpeedUp, //速度上昇
        AttackDown, //攻撃力減少
        DefenseDown, //防御力減少
        SpeedDown, //速度減少
        // 他のバフの種類を追加
    }
    public enum BUffEffectType 
    {
        active, //使用したときに効果が発動するタイプ
        passive, //常に効果が発動しているタイプ
    }
    public enum BuffTimeType
    {
        Temporary, //一時的なバフ
        Permanent  //永続的なバフ
    }
    
    [SerializeField] private float buffDuration; //バフ効果時間
    [SerializeField] private BuffType buffType; //バフの種類
    [SerializeField] private BUffEffectType buffEffectType; //バフの効果タイプ


    public override void Activation(float value, ItemRecieveData data)
    {
        //以下は効果時間アイテムの話
        //受けとったEntityに与える処理
        //data.Entity.BuffSetっていうのを用意されている
        //そこにbuffTypeとm_valueを与えればよい
        //modifierにまとめる（m_valueとbuffTypeを入れる）
        //効果時間のあるものはdurationが必要
        //modifiers = new EventModifiers(buffType, value, buffDuration); //EventModifiersにbuffTypeとvalueとbuffDurationを渡す
        //Entity entity = data.entity; //Entityをdataから取得
        //バフ処理
        if (buffEffectType == BUffEffectType.active)
        {

            if (buffDuration <= 0) //buffDurationが0以下の場合は処理を抜ける
            {
                Debug.LogWarning("バフの効果時間が設定されていません");
                
                return;
            }
            //entity.BuffSet(buffType, value, buffDuration); //EntityのBuffSetにbuffTypeとvalueとbuffDurationを渡す
            switch (buffType)
            {
                case BuffType.AttackUp:
                    //攻撃力上昇の処理
                    //attack = entity.BaseValue + value; //攻撃力を上昇させる処理
                    break;
                case BuffType.DefenseUp:
                    //防御力上昇の処理

                    break;
                case BuffType.SpeedUp:
                    //速度上昇の処理
                    break;
                case BuffType.AttackDown:
                    //攻撃力減少の処理
                    break;
                case BuffType.DefenseDown:
                    //防御力減少の処理
                    break;
                case BuffType.SpeedDown:
                    //速度減少の処理
                    break;
                default:
                    //その他のバフの処理
                    Debug.LogWarning("未定義のバフタイプです");
                    break;
            }
            while (buffDuration > 0) //buffDurationが0になるまでループ
            {
                buffDuration = Time.deltaTime; //buffDurationを減らす
                                               //バフの効果を維持する処理
                if (buffDuration <= 0) //buffDurationが0以下になったらループを抜ける
                {
                    //BaseValueReset(entity); //EntityのBaseValueを元に戻す処理
                }

            }
            //if (HP == 0)
            //{
            //    //HPが0になったときの処理
            //    BaseValueReset(entity); //EntityのBaseValueを元に戻す処理
            //}

        }
        //else if(buffEffectType == BUffEffectType.passive)
        //{
        //    //パッシブの処理
        //     entity = data.entity; //Entityをdataから取得
        //    entity.BuffSet(buffType, value, buffDuration); //EntityのBuffSetにbuffTypeとvalueとbuffDurationを渡す
        //     switch (buffType)
        //    {
        //        case BuffType.AttackUp:
        //            //攻撃力上昇の処理

        //            break;
        //        case BuffType.DefenseUp:
        //            //防御力上昇の処理

        //            break;
        //        case BuffType.SpeedUp:
        //            //速度上昇の処理
        //            break;
        //        case BuffType.AttackDown:
        //            //攻撃力減少の処理
        //            break;
        //        case BuffType.DefenseDown:
        //            //防御力減少の処理
        //            break;
        //        case BuffType.SpeedDown:
        //            //速度減少の処理
        //            break;
        //        default:
        //            //その他のバフの処理
        //            Debug.LogWarning("未定義のバフタイプです");
        //        break;
        //    }

    }
        //以下はパッシブの話
        //パッシブ側でEntityのバフの追加と解除を行う
       

        //パッシブアイテムは後回し


        //Debug.Log($"使用して{buffDuration}秒間、{buffType}が{value}上昇した");
       
    //}

    //private void BaseValueReset(Entity entity)
    //{
    //    throw new NotImplementedException();
    //}
}

