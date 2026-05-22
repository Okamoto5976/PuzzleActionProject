using System.Collections.Generic;
using UnityEngine;



public class dropPool:MonoBehaviour
{
    [Header("ドロップ率")]
    [SerializeField] private float item_drop_rat; // ドロップ率
    [SerializeField] private int initialPoolSize = 10; // 初期プールサイズ
    [SerializeField] private GameObject itemPrefab; // ドロップアイテムのプレハブ
    //[SerializeField] private int drop_item_glade; // ドロップアイテムのグレード（例：1=一般、2=レア、3=エピックなど）
    private Queue<GameObject> pool =new Queue<GameObject>(); // アイテムのプール
    private List <Item> dropList = new List<Item>(); // ドロップするアイテムのリスト



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


