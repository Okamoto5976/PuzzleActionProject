using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;



public class dropPool:MonoBehaviour
{
    [Header("ドロップ率")]
    [SerializeField] private float item_drop_rat;
    [SerializeField] private int initialPoolSize = 10;
    [SerializeField] private GameObject itemPrefab;
    //[SerializeField] private int drop_item_glade;
    private Queue<GameObject> pool =new Queue<GameObject>();

   
    public GameObject GetItem(Vector3 position)
    {
        GameObject obj;
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(itemPrefab);
        }
        obj.transform.position = position;
        obj.SetActive(true);
        return obj;
    }
    private void Awake()
    {
        // 初期プール生成
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(itemPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }
    public void ReturnItem(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
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