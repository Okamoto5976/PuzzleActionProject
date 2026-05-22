using UnityEngine;
[CreateAssetMenu(fileName = "SpecialItem", menuName = "Scriptable Objects/Datas/SpecialItem")]
public class TorapItem : Item
{
    [SerializeField] private float torpvalue;
    //trapType
    //{arrow,gas

    //poolからもらったobj入れる 変数
    public GameObject TrapPrefab;

    public override void Activation(float value, ItemRecieveData data)
    {
        //data posにobjを置く
        //objにEntityがついている　Enityに dataのbaseValueを送る（コメントにする
        //objのEntityにmoveDirがあるからdataのdirを入れる
        var entity = TrapPrefab.GetComponent<Entity>();
        entity.Initialize(ItemType.Weapon, data.baseValue);
        //entity.moveDir = data.dir;
        //GameObject obj = Instantiate(TrapPrefab, data.pos, Quaternion.LookRotation(data.dir));
    }
}
