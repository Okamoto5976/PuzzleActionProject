using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.PlayerSettings;



public class DropPool : MonoBehaviour
{
     
    
    public class PoolItem
    {
        public List<Item> ItemList;
        public ItemData Prefab;
        public ItemData ItemSize;
    }

    [SerializeField] private List<GameObject>initialList = new List<GameObject>();

    ItemData data;
    private List<Item> ItemList = new List<Item>();
    private Dictionary<GameObject, Queue<GameObject>> pools;
    private Queue<GameObject> pool = new Queue<GameObject>();
    private void Awake()
    {
        // 初期リストのデータをプールへ移行
        foreach (var obj in initialList)
        {
            if (obj == null) continue;
            obj.SetActive(false);  // 非表示にしてプールへ
            pool.Enqueue(obj);
        }

    }

    public void Get(string id)
    {
        //return pool.Dequeue();


    }

   
    public void ReturnItem(GameObject item, GameObject prefab)
    {
        item.SetActive(false); // アイテムを非アクティブにする

        pools[data.Prefab].Enqueue(item);
        pools[item].Enqueue(prefab); // アイテムをプールに戻す
    }

    internal void ReturnItem(Item item, GameObject prefab)
    {
        throw new System.NotImplementedException();
    }

}




