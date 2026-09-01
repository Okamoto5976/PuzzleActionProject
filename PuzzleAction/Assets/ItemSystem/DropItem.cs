using UnityEngine;

public class DropItem : MonoBehaviour
{

    //float radius = 3f; // アイテムを拾うための半径
    [SerializeField] private float m_timeToReturn = 5f; // アイテムが自動的に戻るまでの時間
    //DropPool pool; // アイテムを管理するドロッププールのクラス
    //private GameObject prefab;
    //public event Action m_event;

    private Item m_itemData;

    public Item ItemData => m_itemData;

    ////playerの座標が自身の半径３mいないに　プレイヤーが入ったら　プレイヤーにアイテムを渡す。
    //private void ItemGet(Collider other)
    //{
    //    if (Vector3.Distance(transform.position, other.transform.position) <= radius)
    //    {
    //        if (pool == null)
    //        {
    //            Debug.LogError("Pool is not assigned.");
    //            return;
    //        }
    //        if (other.CompareTag("Player"))
    //        {
    //            //Add.inventory();
    //            Return();
    //        }
    //    }
    //}


    private void Start()
    {
        Initialize();
    }


    //call entity get dropItem
    public void ItemGet()
    {
        Return();
    }

    public void Initialize(Item data = null)
    {
        Invoke(nameof(Return), m_timeToReturn); // timeToReturn秒後にReturnメソッドを呼び出す
        if (data == null) return;
        SetItemData(data);
    }

    private void SetItemData(Item data)
    {
        m_itemData = data;
    }

    private void Return()
    {

        //if (pool != null)
        //{
        //    //Poolに返す処理
        //    pool.ReturnItem(prefab);
        //}
        Debug.Log("Return");
        //return pool
    }
}
