using System;
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
        public GameObject prefab;
        public int initialCount = 5;
    }
    public GameObject prefab;
    [SerializeField] public Dictionary<Item, Queue<Item>> pools;
    public List<PoolItem> ItemList;

    //ItemData data;
    private ObjectPool<GameObject> pool;
    private int maxSize;
    private int DefaultCapacity;

    private void Awake()
    {
        pool = new ObjectPool<GameObject>(
            createFunc: CreateItem,       // 生成時
            actionOnGet: ItemGet,   // Get 時
            actionOnRelease: ReturnItem, // Release 時
            collectionCheck: true,        // 重複返却などの安全チェック
            defaultCapacity:DefaultCapacity,
            maxSize: maxSize
         );
    }
    //pools = new Dictionary<string,Queue<GameObject>>();



    private GameObject CreateItem()
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        return obj;
    }

    private void ItemGet(GameObject obj)
    {
        obj.SetActive(true);
    }
    public void ReturnItem(GameObject obj)
    {
        obj.SetActive(false); // アイテムを非アクティブにする
        //pools[prefab].Enqueue(prefab);
        //pool[item].Enqueue(prefab); // アイテムをプールに戻す
    }



    public void GetItem(int id)
    {
        foreach(var item in ItemList)
        {
            var queue= new Queue<GameObject>();
            for(int i =0; i < item.initialCount; i++)
            {
                var obj = Instantiate(item.prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            //pools[] = queue;
        }
    }

    public void ItemDrop(int id , ItemRecieveData r_data )
    {
        GetItem(id);
        transform.position = r_data.pos;
        //object value = Instantiate(prefab, r_data.pos, Quaternion.Euler(r_data.dir));


    }








}




