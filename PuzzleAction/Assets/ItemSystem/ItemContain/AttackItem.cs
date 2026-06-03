using UnityEngine;

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
    [SerializeField] private EffectType type;
    ItemPool poolitem;
   

    public Entity ItemPrefab { get; private set; }
    public object prefab { get; private set; }

    public override void Activation(float value, ItemRecieveData data)
    {
        Debug.Log("Test");
        //ItemManagerからpool経由してEntityを呼ぶItemManagerでpoolを仲介にenumで種類を渡す
        Entity entity = poolitem.Get(type); //pool経由してEntityを呼ぶpoolを仲介にenumで種類を渡す
        //Playerから渡されたdataの中にある座標の位置に呼び出す
        GameObject obj = Object.Instantiate((GameObject)prefab, data.pos, Quaternion.LookRotation(data.dir)); // プレイヤーデータから座標と向きを呼び出す
        //Playerから渡されたdataのbaseValueを呼び出したEntityに渡す 
        entity.BaseValue = data.baseValue; //EntityにbaseValueを渡す
        poolitem.Return(ItemPrefab);
    }
}
