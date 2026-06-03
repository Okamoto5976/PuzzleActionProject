using System.Collections.Generic;
using UnityEngine;



public class dropPool : MonoBehaviour
{
    [System.Serializable]
    public class PoolItem
    {
        public ItemData i_data;
        public GameObject prefab;
        public int poolSize = 5;
        [Range(0f, 1f)]
        public float dropChance = 0.5f;
    }


    //ItemData itemData; // アイテムのデータクラス
    //public List<Item> PoolItem = new List<Item>();
    public List<PoolItem> itemConfigs;
    private Dictionary<GameObject, Queue<GameObject>> pools;
    //private Queue<GameObject> pool = new Queue<GameObject>(); // アイテムのプール


    // アイテムのドロップ率とドロップサイズを考慮してアイテムをドロップするクラス
    private void Awake()
    {
        pools = new Dictionary<GameObject, Queue<GameObject>>();
        foreach (var config in itemConfigs)
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>();
            for (int i = 0; i < config.poolSize; i++)
            {
                GameObject obj = Instantiate(config.prefab);
                obj.SetActive(false);
                poolQueue.Enqueue(obj);
            }
            pools[config.prefab] = poolQueue;
        }
        //float rool = Random.value * itemData.DropRate; // ランダムクラスのロード
        //初期プール生成
        //    for (int i = 0; i < itemData.DropSize; i++) // 初期プールサイズ分のアイテムを生成
        //    {


        //GameObject dropIndex = Instantiate(.ItemPrefab); // アイテムのプレハブから新しいアイテムを生成
        //dropIndex.SetActive(false); // アイテムを非アクティブにする
        //pool.Enqueue(dropIndex); // アイテムをプールに追加
        //}
    }
    // アイテムのドロップ
    public void DropItem(Vector3 position) // アイテムを取得してドロップ
    {
        List<PoolItem> candidates = new List<PoolItem>();
        foreach (var config in itemConfigs)
        {
            if (Random.value <= config.dropChance)
            {
                candidates.Add(config);
            }
        }
        if (candidates.Count == 0) return;

        PoolItem selected = candidates[Random.Range(0, candidates.Count)];

        if (pools[selected.prefab].Count > 0)
        {
            GameObject item = pools[selected.prefab].Dequeue();
            item.transform.position = position;
            item.SetActive(true);
        }
        //GameObject dropIndex; // ドロップするアイテム
        //if (pool.Count > 0) // プールにアイテムがある場合はそれを使用
        //{
        //dropIndex = pool.Dequeue(); // プールからアイテムを取り出す
        //}
        //else
        //{
        //dropIndex = Instantiate(itemData.DropPrefab); // プールにアイテムがない場合は新しく生成
        //}

        //dropIndex.transform.position = position; // アイテムの位置を設定
        //dropIndex.SetActive(true); // アイテムをアクティブにしてドロップ
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


