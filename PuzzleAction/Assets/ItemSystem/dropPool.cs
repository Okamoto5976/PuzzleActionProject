using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;

public class PoolItem
{
    public int id;
    public string name;
    public GameObject prefab;
    public int initialCount;
}
public class DropPool : MonoBehaviour
{
    public int SetCount;
    //public PoolItem poolItem;
    public GameObject prefab;
    //public PoolItem poolItem;
    public List<PoolItem> ItemList = new();
    public List <Item>Items=new();
    Dictionary<PoolItem, ObjectPool<GameObject>> pools;
    private ObjectPool<GameObject> pool;
    public int maxSize;
    public int DefaultCapacity;
    //初期設定
    private void Awake()
    {
        pool = new ObjectPool<GameObject>(
           CreateItem,       // 生成時
           ItemGet,   // Get 時
           ReturnItem, // Release 時
           collectionCheck: true,        // 重複返却などの安全チェック
           defaultCapacity: DefaultCapacity,
           maxSize: maxSize
        );
    }
    public void start()
    {
        CreateItem();
    }
    private GameObject CreateItem()
    {
        for (int i =0; i<= Items.Count; i++) {
            for (int j= 0; j<= SetCount; j++ ) {
                var obj = Instantiate(prefab);
                obj.SetActive(false);
                //return obj;
            }
        }
        return null;
    }

    private void ItemGet(GameObject obj)
    {
        obj.SetActive(true);
        return ;
    }
    public void ReturnItem(GameObject obj)
    {
        obj.SetActive(false); // アイテムを非アクティブにする
        //pool[].Enqueue(obj); // アイテムをプールに戻す
        return ;
    }

    public GameObject Get(int id)
    {
         foreach(var item in ItemList)
        {
            if (item.id == id)
            {
                
                var obj = Instantiate(item.prefab);
                obj.SetActive(false);
                return obj;
            }
            else
            {
                Debug.Log("そのidは見つかりません");
            }
        }
         return null;
  
    }
    public void ItemDrop(int id ,ItemRecieveData r_data )
    {
        GameObject obj= Get(id);
        obj.transform.position = r_data.pos;
        obj.transform.rotation=Quaternion.Euler(r_data.dir);   
        //object value = Instantiate(prefab, r_data.pos, Quaternion.Euler(r_data.dir));


    }
}
