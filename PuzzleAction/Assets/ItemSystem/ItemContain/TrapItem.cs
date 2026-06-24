using UnityEngine;
[CreateAssetMenu(fileName = "SpecialItem", menuName = "Scriptable Objects/Datas/TrapItem")]
public class TrapItem : Item
{
    [SerializeField] private float trapValue;

    //poolからもらったobj入れる 変数
    public TrapBase TrapPrefab;

    [SerializeField] private Enum_TrapType m_enumTrap;

    public Enum_TrapType EnumTrap => m_enumTrap;

    public void SetTrap(TrapBase obj)
    {
        TrapPrefab = obj;
    }

    public override void Activation(float value, ItemRecieveData data)
    {
        //data posにobjを置く dataに向きも入れる
        //objにEntity(Trap)がついている　Enityに dataのbaseValueを送る（コメントにする
        //EntityTrap.SetbaseValue(data.baseValue)                Trap側でTrapの攻撃力＋baseValue
        //objのEntityにmoveDirがあるからdataのdirを入れる
        //var entity = TrapPrefab.GetComponent<Entity>();
        TrapPrefab.Init(data.entity, data.dir, (int)data.baseValue);
        TrapPrefab.gameObject.transform.position = data.pos;
        //TrapPrefab.gameObject.transform.rotation = Quaternion.LookRotation(data.dir);
        //entity.moveDir = data.dir;
    }
}
