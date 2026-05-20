using UnityEngine;
[CreateAssetMenu(fileName = "BuffItem", menuName = "Scriptable Objects/Datas/BuffItem")]
public class BuffItem : Item 
{
    public enum BuffType//後　仮なので消す
    {
        AttackUp,
        DefenseUp,
        SpeedUp,
        AttackDown,
        DefenseDown,
        SpeedDown,
        // 他のバフの種類を追加
    }

    [SerializeField] private float buffDuration; //バフ効果時間
    [SerializeField] private BuffType buffType; //バフの種類
    //[SerializeField] private ItemData itemName;
    public override void Activation(float value, ItemRecieveData data)
    {
        //バフ処理
        //以下は効果時間アイテムの話
        //受けとったEntityに与える処理
        //data.Entity.BuffSetっていうのを用意されている
        Entity entity = data.entity;
        if (entity != null) {
            entity.BuffSet(buffType, value, buffDuration);
        }



        //そこにbuffTypeとm_valueを与えればよい
        //modifierにまとめる（m_valueとbuffTypeを入れる）
        //効果時間のあるものはdurationが必要
        //yield return new WaitForSeconds(buffDuration);
        //以下はパッシブの話
        //パッシブ側でEntityのバフの追加と解除を行う

        //パッシブアイテムは後回し


        Debug.Log($"使用して{buffDuration}秒間、{buffType}が{value}上昇した");
       
    }
}

