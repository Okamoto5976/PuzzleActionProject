using NUnit.Framework;
using UnityEngine;
using static ItemManager;
using static UnityEngine.EventSystems.EventTrigger;
[CreateAssetMenu(fileName = "AttackItem", menuName = "Scriptable Objects/Datas/AttackItem")]
public class AttackItem : Item
{
    public enum EffectType
    {
        Null,
        Heal,
        Damage,
        Buff,
        Debuff,
        Torap
    }
    [SerializeField] private float Durability; //耐久値
    [SerializeField] private float attackRange; //攻撃範囲
    [SerializeField] private float Damage; //ダメージ
    [SerializeField] private float ObjectTime;
    public GameObject prefab; //アイテムのプレハブ
    private ItemPool itempool;
    
    public override void Activation(float value, ItemRecieveData data)
    {
        Debug.Log("Test");
        //ItemManagerからpool経由してEntityを呼ぶItemManagerでpoolを仲介にenumで種類を渡す
    //    Entity entity = itempool.Get(type); //ItemManagerからpool経由してEntityを呼ぶItemManagerでpoolを仲介にenumで種類を渡す
    //    //Playerから渡されたdataの中にある座標の位置に呼び出す
    //    GameObject obj = Instantiate(prefab, data.pos, Quaternion.LookRotation(data.dir)); //プレイヤーデータから座標と向きを呼び出す
    //    //Playerから渡されたdataのbaseValueを呼び出したEntityに渡す 
    //    entity.BaseValue = data.baseValue; //EntityにbaseValueを渡す
    //    //Destroy();
    //    Return(Entity entity);
    }
}
