using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    ItemData data;
    Item item; // アイテムのクラス
    float radius = 3f; // アイテムを拾うための半径
    float timeToReturn = 5f; // アイテムが自動的に戻るまでの時間
    DropPool pool; // アイテムを管理するドロッププールのクラス
    private GameObject prefab;
    [SerializeField]public GameObject prefabPrefab; 
    //public event Action m_event;
    
    ////playerの座標が自身の半径３mいないに　プレイヤーが入ったら　プレイヤーにアイテムを渡す。
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
                //Add.inventory();
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
            //pool.ReturnItem(item, prefab);
        }
    }
}
