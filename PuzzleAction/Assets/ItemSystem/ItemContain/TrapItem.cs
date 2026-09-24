using UnityEngine;
[CreateAssetMenu(fileName = "SpecialItem", menuName = "Scriptable Objects/Datas/TrapItem")]
public class TrapItem : Item
{
    //poolからもらったobj入れる 変数
    public TrapBase TrapPrefab;

    [SerializeField] private Enum_TrapType m_enumTrap;

    [SerializeField] private bool m_isSetGround;
    [SerializeField] private FloatRunTime m_groundPos;

    public Enum_TrapType EnumTrap => m_enumTrap;

    public void SetTrap(TrapBase obj)
    {
        TrapPrefab = obj;
    }

    public override void Activation(ItemRecieveData data)
    {
        if (TrapPrefab == null)
        {
            Debug.Log("null!!");
            return;
        }


        //Trap Area use item
        if(data.entity == null)
        {
            TrapPrefab.TrapInit(data);
            TrapPrefab.gameObject.SetActive(false);

            return;
        }

        //if (data.power > 0f)
        //{
        //    TrapPrefab.PullInit(data.entity, data.dir, data.power);
        //    //TrapPrefab.gameObject.transform.position = data.pos;
        //    TrapPrefab.gameObject.SetActive(true);
        //    return;
        //}
        
        //data posにobjを置く dataに向きも入れる
        //objにEntity(Trap)がついている　Enityに dataのbaseValueを送る（コメントにする
        //EntityTrap.SetbaseValue(data.baseValue)                Trap側でTrapの攻撃力＋baseValue
        //objのEntityにmoveDirがあるからdataのdirを入れる
        //var entity = TrapPrefab.GetComponent<Entity>();
        
        TrapPrefab.Init(data);
        //TrapPrefab.gameObject.transform.position = data.pos;
        TrapPrefab.gameObject.SetActive(true);
        //TrapPrefab.gameObject.transform.rotation = Quaternion.LookRotation(data.dir);
        //entity.moveDir = data.dir;
    }
}
