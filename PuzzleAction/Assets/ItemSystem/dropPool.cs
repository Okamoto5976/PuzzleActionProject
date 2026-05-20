using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;



public class dropPool:MonoBehaviour
{
    [Header("ドロップ率")]
    [SerializeField] private float item_drop_rat; // ドロップ率
    [SerializeField] private int initialPoolSize = 10; // 初期プールサイズ
    [SerializeField] private GameObject itemPrefab; // ドロップアイテムのプレハブ
    //[SerializeField] private int drop_item_glade; // ドロップアイテムのグレード（例：1=一般、2=レア、3=エピックなど）
    private Queue<GameObject> pool =new Queue<GameObject>(); // アイテムのプール


    public GameObject GetItem(Vector3 position) // アイテムを取得してドロップ
    {
        GameObject obj; // ドロップするアイテム
        if (pool.Count > 0) // プールにアイテムがある場合はそれを使用
        {
            obj = pool.Dequeue(); // プールからアイテムを取り出す
        }
        else 
        {
            obj = Instantiate(itemPrefab); // プールにアイテムがない場合は新しく生成
        }
        obj.transform.position = position; // アイテムの位置を設定
        obj.SetActive(true); // アイテムをアクティブにしてドロップ
        return obj; // ドロップしたアイテムを返す
    }
    private void Awake()
    {
        // 初期プール生成
        for (int i = 0; i < initialPoolSize; i++) // 初期プールサイズ分のアイテムを生成
        {
            GameObject obj = Instantiate(itemPrefab); // アイテムのプレハブから新しいアイテムを生成
            obj.SetActive(false); // アイテムを非アクティブにする
            pool.Enqueue(obj); // アイテムをプールに追加
        }
    }
    public void ReturnItem(GameObject obj)
    {
        obj.SetActive(false); // アイテムを非アクティブにする
        pool.Enqueue(obj); // アイテムをプールに戻す
    }
}


/*    private void Awake()
    {
        // プールの生成
        pool = new ObjectPool<GameObject>(
            createFunc: CreateItem,
            actionOnGet: OnGetItem,
            actionOnRelease: OnReleaseItem,
            actionOnDestroy: OnDestroyItem,
            collectionCheck: false,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }*/

    // 新しいアイテムを生成
/*    private GameObject CreateItem()
    {
        GameObject obj = Instantiate(itemPrefab);
        obj.SetActive(false);
        // プールに戻すためのスクリプトをアタッチ
        obj.AddComponent<PooledItem>().SetPool(pool);
        return obj;
    }
*/
    // プールから取得したとき
  /*  private void OnGetItem(GameObject obj)
    {
        obj.SetActive(true);
    }

    // プールに戻すとき
    private void OnReleaseItem(GameObject obj)
    {
        obj.SetActive(false);
    }

*//*    // プールの最大数を超えて破棄されるとき
    private void OnDestroyItem(GameObject obj)
    {
        Destroy(obj);
    }*//*

    // 外部から呼び出してアイテムをドロップ
    public void DropItem(Vector3 position)
    {
        GameObject item = pool.Get();
        item.transform.position = position;
    }
}*/