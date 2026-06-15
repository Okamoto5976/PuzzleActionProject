using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.Progress;
//using static UnityEditor.PlayerSettings;




public class DropPool : MonoBehaviour
{
    public class PoolItem
    {
        public string id;
    //    public GameObject prefab;
    //    public int initialCount = 5;
    }
    public GameObject prefab;
    [SerializeField] public Dictionary<string, Queue<object>> pools;
    //public List<PoolItem> ItemList;

    //ItemData data;
    private ObjectPool<GameObject> pool;
    private void Awake()
    {
        pool = new ObjectPool<GameObject>(
            createFunc: CreateItem,       // 生成時
            actionOnGet: OnGetFromPool,   // Get 時
            actionOnRelease: OnReleasedToPool, // Release 時
            collectionCheck: true,        // 重複返却などの安全チェック
            defaultCapacity: 10,          // 初期数
            maxSize: 50                   // 最大数
         );
    }
    //pools = new Dictionary<string,Queue<GameObject>>();
    //foreach(var item in ItemList)
    //{


    //    //    var queue= new Queue<GameObject>();
    //    //    for(int i =0; i < item.initialCount; i++)
    //    //    {
    //    //        var obj = Instantiate(item.prefab);
    //    //        obj.SetActive(false);
    //    //        queue.Enqueue(obj);
    //    //    }
    //    //    pools[item.id] = queue;
    //    //}


    private GameObject CreateItem()
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        return obj;
    }

    private void OnGetFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReleasedToPool(GameObject obj)
    {
        obj.SetActive(false);
    }


    //public void GetItem(string id)
    //{

    //    foreach (var id in pool)
    //    {

    //        GameObject item = pool.Get();
    //        item.transform.position = position;
    //        return item;
    //    }

        
    //}
    //public void Drop(string id , ItemRecieveData r_data)
    //{

    //    Instantiate(prefab, r_data.pos, r_data.dir);
    //    //Debug.Log($"{item.name}をドロップしました。");
    //}

    //public GameObject Get()
    //{
    //    GameObject obj = Instantiate(PoolItem.prefab);
    //    obj.SetActive(false);
    //    return obj;

    //}



    public void ReturnItem(string id, GameObject obj)
    {
        obj.SetActive(false); // アイテムを非アクティブにする

        pools[id].Enqueue(obj);
        //pool[item].Enqueue(prefab); // アイテムをプールに戻す
    }



}




