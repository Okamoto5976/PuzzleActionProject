using UnityEngine;
[CreateAssetMenu(fileName = "SpecialItem", menuName = "Scriptable Objects/Datas/SpecialItem")]
public class TrapItem : Item
{
    [SerializeField] private float trapValue;
    //trapType
    //{arrow,gas

    //poolからもらったobj入れる 変数
    public GameObject TrapPrefab;

    public override void Activation(float value, ItemRecieveData data)
    {
        //data posにobjを置く dataに向きも入れる
        //objにEntity(Trap)がついている　Enityに dataのbaseValueを送る（コメントにする
        //EntityTrap.SetbaseValue(data.baseValue)                Trap側でTrapの攻撃力＋baseValue
        //objのEntityにmoveDirがあるからdataのdirを入れる
        //var entity = TrapPrefab.GetComponent<Entity>();
        TrapPrefab.transform.position = data.pos;
        TrapPrefab.transform.rotation = Quaternion.LookRotation(data.dir);
        //entity.moveDir = data.dir;
    }
}
