using System;
using System.Collections.Generic;
using UnityEngine;


// 汎用オブジェクトプール
public class ItemPool : MonoBehaviour
{
    Item item;
    [System.Serializable]
    public class PoolItem
    {
        public AttackItem.EffectType Type { get; private set; }
        public ItemData prefab; // アイテムのプレハブ
        public int initialSize = 5; // 初期プールサイズ
    }

    public List<PoolItem> poolItems; // プールアイテムのリスト
    private Dictionary<AttackItem.EffectType, Queue<Entity>> poolDictionary; // プールの辞書
    private Entity prefab;

    public static object Instance { get; internal set; } // シングルトンインスタンス

    void Awake()
    {
        poolDictionary = new Dictionary<AttackItem.EffectType, Queue<Entity>>(); // プールの初期化

        foreach (var item in poolItems) // 各アイテムタイプごとにプールを作成
        {
            var queue = new Queue<Entity>(); // キューを作成
            for (int i = 0; i < item.initialSize; i++)// 初期サイズ分のオブジェクトを生成
            {
                Entity obj = Instantiate(prefab, transform, false);// オブジェクトを生成
                obj.Deactivate(); // 非アクティブにしてプールに戻す
                queue.Enqueue(obj); // キューに追加
            }
            poolDictionary[item.Type] = queue; // 辞書に追加
        }
    }

    // 取得
    public Entity Get(AttackItem.EffectType type) // アイテムタイプを指定してオブジェクトを取得
    {
        if (!poolDictionary.ContainsKey(type)) // 指定されたタイプのプールが存在しない場合
        {
            Debug.LogError($"Pool for {type} not found!"); // エラーログを出力
            return null; // nullを返す
        }

        if (poolDictionary[type].Count > 0)  // プールにオブジェクトがある場合はそれを返す
        {
            return poolDictionary[type].Dequeue(); // キューからオブジェクトを取り出して返す
        }
        else
        {
            // 必要なら追加生成  
            var prefab = poolItems.Find(p => p.Type == type).prefab; // プールアイテムリストから指定されたタイプのプレハブを見つける
            Entity obj = Instantiate(prefab, transform, Quaternion.identity); // プレハブから新しいオブジェクトを生成
            obj.Deactivate(); // 非アクティブにしてプールに戻す
            return obj; // 生成したオブジェクトを返す
        }
    }

    private Entity Instantiate(ItemData prefab, Transform transform, Quaternion identity)
    {
        throw new NotImplementedException();
    }

    // 戻す
    public void Return(Entity entity)
    {
        entity.Deactivate(); // 非アクティブにしてプールに戻す
        poolDictionary[entity.prefab].Enqueue(entity); // キューに戻す
    }

    // ここで、EntityクラスのBuffSetメソッドを呼び出すためのサンプルコードを追加します。
    internal static Entity entity(float baseValue)
    {
        throw new NotImplementedException();// これはサンプルコードであり、実際の実装に合わせて修正してください。
    }
}


