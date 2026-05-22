using System.Collections.Generic;
using UnityEngine;



public class dropPool:MonoBehaviour
{
    ItemData itemData; // アイテムのデータクラス
    ItemManager ItemList;
     private Queue<GameObject> pool =new Queue<GameObject>(); // アイテムのプール

    // アイテムのドロップ率とドロップサイズを考慮してアイテムをドロップするクラス
    private void Awake()
    {
        
        //float rool = Random.value * itemData.DropRate; // ランダムクラスのロード
        // 初期プール生成
        //for (int i = 0; i < itemData.DropSize; i++) // 初期プールサイズ分のアイテムを生成
        //{
        //    GameObject dropIndex = Instantiate(); // アイテムのプレハブから新しいアイテムを生成
        //    dropIndex.SetActive(false); // アイテムを非アクティブにする
        //    pool.Enqueue(dropIndex); // アイテムをプールに追加
        //}
    }// アイテムのドロップ
    //public GameObject DropItem(Vector3 position) // アイテムを取得してドロップ
    //{

    //    //GameObject dropIndex; // ドロップするアイテム
    //    //if (pool.Count > 0) // プールにアイテムがある場合はそれを使用
    //    //{
    //    //    dropIndex = pool.Dequeue(); // プールからアイテムを取り出す
    //    //}
    //    //else
    //    //{
    //    //    dropIndex = Instantiate(itemData.DropPrefab); // プールにアイテムがない場合は新しく生成
    //    //}

    //    //dropIndex.transform.position = position; // アイテムの位置を設定
    //    //dropIndex.SetActive(true); // アイテムをアクティブにしてドロップ
    //    //Debug.Log("アイテムをドロップしました。"); // ドロップしたことをログに出力
    //    //return dropIndex; // ドロップしたアイテムを返す
    //}
    public void ReturnItem(GameObject obj)
    {
        obj.SetActive(false); // アイテムを非アクティブにする
        pool.Enqueue(obj); // アイテムをプールに戻す
    }
}


