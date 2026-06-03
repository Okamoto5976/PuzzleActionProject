using System.Collections.Generic;
using UnityEngine;



public class dropPool : MonoBehaviour
{
    [System.Serializable]
    public class PoolItem
    {
        public string Enemeadrop; // 敵のデータクラス 
        public List<Item> ItemdropList; // アイテムのデータクラス
        public GameObject prefab;
        public int poolSize = 5;
        [Range(0f, 1f)]
        public float dropChance = 0.5f;
    }


    //ItemData itemData; // アイテムのデータクラス
    //public List<Item> PoolItem = new List<Item>();
    public List<PoolItem> itemConfigs;
    private Dictionary<GameObject, Queue<GameObject>> pools;
    private Queue<GameObject> pool = new Queue<GameObject>(); // アイテムのプール
    private PoolItem config;


    // アイテムのドロップ率とドロップサイズを考慮してアイテムをドロップするクラス
    private void Awake()
    {
        pools = new Dictionary<GameObject, Queue<GameObject>>();
        foreach (var config in itemConfigs)
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>(); //
            for (int i = 0; i < config.poolSize; i++)
            {
                GameObject obj = Instantiate(config.prefab);   // アイテムのプレハブから新しいアイテムを生成
                obj.SetActive(false); // アイテムを非アクティブにする
                poolQueue.Enqueue(obj); // プールにアイテムを追加
            }
            pools[config.prefab] = poolQueue; // プールを辞書に追加
        }
     
    }
    // アイテムのドロップ
    public void DropItem(Vector3 position) // アイテムを取得してドロップ
    {
        List<PoolItem> candidates = new List<PoolItem>();

        foreach (var config in itemConfigs)
        {
            if (config.poolSize <= 0) continue; // プールサイズが0以下の場合はスキップ

        }
        if (Random.value <= config.dropChance)
        {
            candidates.Add(config);
        }
   
        if (candidates.Count == 0) return;

        PoolItem selected = candidates[Random.Range(0, candidates.Count)];

        if (pools[selected.prefab].Count > 0) // プールにアイテムがある場合
        {
            GameObject item = pools[selected.prefab].Dequeue(); // プールからアイテムを取り出す
            item.transform.position = position; // アイテムの位置を設定
            item.SetActive(true); // アイテムをアクティブにしてドロップ
        }
        else
        {
            GameObject item = Instantiate(selected.prefab, position, Quaternion.identity); // プールにアイテムがない場合は新しく生成
        }
 
            Debug.Log("アイテムをドロップしました。"); // ドロップしたことをログに出力
          //return ; // ドロップしたアイテムを返す
    }
    public void ReturnItem(GameObject item,GameObject prefab)
    {
        item.SetActive(false); // アイテムを非アクティブにする
        pools[prefab].Enqueue(item);
        pools[item].Enqueue(prefab); // アイテムをプールに戻す
    }

    internal void ReturnItem(Item item, GameObject prefab)
    {
        throw new System.NotImplementedException();
    }
}


