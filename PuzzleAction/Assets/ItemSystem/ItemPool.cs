using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

// 汎用オブジェクトプール
public class ItemPool : MonoBehaviour
{
    [System.Serializable]
    public class PoolItem
    {
        public ItemType type;
        public Entity prefab;
        public int initialSize = 5;
    }
    Entity entity;

    public List<PoolItem> poolItems;
    private Dictionary<ItemType, Queue<Entity>> poolDictionary;

    public static object Instance { get; internal set; }

    void Awake()
    {
        poolDictionary = new Dictionary<ItemType, Queue<Entity>>();

        foreach (var item in poolItems)
        {
            var queue = new Queue<Entity>();
            for (int i = 0; i < item.initialSize; i++)
            {
                Entity obj = Instantiate(item.prefab, transform);
                obj.Deactivate();
                queue.Enqueue(obj);
            }
            poolDictionary[item.type] = queue;
        }
    }

    // 取得
    public Entity Get(ItemType type)
    {
        if (!poolDictionary.ContainsKey(type))
        {
            Debug.LogError($"Pool for {type} not found!");
            return null;
        }

        if (poolDictionary[type].Count > 0)
        {
            return poolDictionary[type].Dequeue();
        }
        else
        {
            // 必要なら追加生成
            var prefab = poolItems.Find(p => p.type == type).prefab;
            Entity obj = Instantiate(prefab, transform);
            obj.Deactivate();
            return obj;
        }
    }

    // 戻す
    public void Return(Entity entity)
    {
        entity.Deactivate();
        poolDictionary[entity.Type].Enqueue(entity);
    }

    //internal static Entity entity(float baseValue)
    //{
    //    throw new NotImplementedException();
    //}
}
//public class ObjectPool<T> where T : MonoBehaviour
//{
//    private readonly Queue<T> pool = new Queue<T>();
//    private readonly T prefab;
//    private readonly Transform parent;

//    public ObjectPool(T prefab, int initialSize, Transform parent = null)
//    {
//        this.prefab = prefab;
//        this.parent = parent;

//        for (int i = 0; i < initialSize; i++)
//        {
//            var obj = UnityEngine.Object.Instantiate(prefab, parent);
//            obj.gameObject.SetActive(false);
//            pool.Enqueue(obj);
//        }
//    }

//    public T Get()
//    {
//        if (pool.Count > 0)
//        {
//            return pool.Dequeue();
//        }
//        return UnityEngine.Object.Instantiate(prefab, parent);
//    }

//    public void Return(T obj)
//    {
//        obj.gameObject.SetActive(false);
//        pool.Enqueue(obj);
//    }
//}

// ItemManager が enum を受け取り、プールから取得
/*public class ItemManager : MonoBehaviour
{
    [Serializable]
    public struct ItemPoolConfig
    {
        public ItemType type;
        public Item prefab;
        public int initialSize;
    }

    [SerializeField] private List<ItemPoolConfig> poolConfigs;

    private Dictionary<ItemType, ObjectPool<Item>> pools;

    private void Awake()
    {
        pools = new Dictionary<ItemType, ObjectPool<Item>>();
        foreach (var config in poolConfigs)
        {
            pools[config.type] = new ObjectPool<Item>(config.prefab, config.initialSize, transform);
        }
    }

    // enum で種類を指定して取得
    public Item SpawnItem(ItemType type, Vector3 position)
    {
        if (!pools.ContainsKey(type))
        {
            Debug.LogError($"No pool found for item type: {type}");
            return null;
        }

        var item = pools[type].Get();
        item.transform.position = position;
        item.Initialize(type);
        return item;
    }

    // アイテムを返却
    public void ReturnItem(Item item)
    {
        if (pools.ContainsKey(item.Type))
        {
            pools[item.Type].Return(item);
        }
        else
        {
            Destroy(item.gameObject);
        }
    }
}*/
