using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;



public class dropPool : MonoBehaviour
{
    [System.Serializable]
    public class PoolItem
    {
        public string Enemeadrop;

        [Range(0f, 1f)]
        public float dropChance = 0.5f;
    }

    ItemData data;
    public List<PoolItem> doropList;
    private Dictionary<GameObject, Queue<GameObject>> pools;
    private Queue<GameObject> pool = new Queue<GameObject>();
    private List<ItemData> DropList = new List<ItemData>();

 //   private void Awake()
 //   {
 //       pools = new Dictionary<GameObject, Queue<GameObject>>();
 //       pools[data.Prefab] = ;
 //   }

 //   public void dorp(Vector3 position, ItemManager index)
 //   {
        
 //       List<Item> condidatews = new DropList<Item>();
 //       //forearch(var  in item)
 //       //{
 //       //}
 //       PoolItem dropChance =Random.Range(0, DropList.Count);

 //       if (pools[data.Prefab] Count > 0)
	//{
 //           GameObject item = pools[data.Prefab].Dequeue();
 //           item.transform.position = position;
 //           item.SetActive(false);
 //           Debug.Log($"{item.name}をドロップしました。");


 //       }
 //       {
 //           GameObject item = Instantiate(data.Prefab, position, Quaternion.identity);

 //       }

 //   }


    public void ReturnItem(GameObject item, GameObject prefab)
    {
        item.SetActive(true); // アイテムを非アクティブにする
        pools[data.Prefab].Enqueue(item);
        pools[item].Enqueue(prefab); // アイテムをプールに戻す
    }

    internal void ReturnItem(Item item, GameObject prefab)
    {
        throw new System.NotImplementedException();
    }
}



