using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    Item item; // アイテムのクラス
    float radius = 3f; // アイテムを拾うための半径
    float timeToReturn = 5f; // アイテムが自動的に戻るまでの時間
    dropPool pool; // アイテムを管理するドロッププールのクラス
    private GameObject prefab;

    //public event Action m_event;
    void Update()
    {
        if(Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= radius)
        {
            AddToInventory();
        }
    }

    void AddToInventory()
    {
        //InventoryManager .Instance.AddItem(item);
        Destroy(gameObject);
    }
    //playerの座標か
    //自身の半径３mいないに　プレイヤーが入ったら　プレイヤーにアイテムを渡す。
    private void ItemGet(Collider other)
    {
        if (Vector3.Distance(transform.position, other.transform.position) <= radius)
        {
            if (pool == null)
            {
                Debug.LogError("Pool is not assigned.");
                return;
            }
            if (other.CompareTag("Player"))
            {
                //Add.itemData;
                Return();
            }
        }
    }

    public void Initialize()
    {
        Invoke(nameof(Return), timeToReturn); // timeToReturn秒後にReturnメソッドを呼び出す
    }
    private void Return()
    {

        if (pool != null)
        {
            //Poolに返す処理
            pool.ReturnItem(item, prefab);
        }
    }
}
