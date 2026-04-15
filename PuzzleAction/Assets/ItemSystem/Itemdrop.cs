using UnityEngine;



public class ItemPool :MonoBehaviour
{ 
/*
    [Header("ドロップ率")]
    [SerializeField] private float item_drop_rat;
    [SerializeField] private int drop_item_name;
    [SerializeField] private int drop_item_glade;
    [SerializeField] private int initalPoolSize = 10;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int initialPoolSize = 10;
    private Queue<GameObject> pool =new Queue<GameObject>();
    public GameObject GetItem(Vector3 position)
    {
        GameObject obj;
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(itemPrefab);
        }
        obj.transform.position = position;
        obj.SetActive(true);
        return obj;
    }
    private void Awake()
    {
        // 初期プール生成
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(itemPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }
    public void RetrunItem(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
public class TreasureChest : MonoBehaviour
{
    [SerializeField] private ItemPool itemPool; // プール参照
    [SerializeField] private int dropCount = 3; // ドロップ数

    private bool isOpened = false;

    public void OpenChest()
    {
        if (isOpened) return;
        isOpened = true;

        // ランダム位置にアイテムをドロップ
        for (int i = 0; i < dropCount; i++)
        {
            Vector3 dropPos = transform.position + new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f));
            itemPool.GetItem(dropPos);
        }
    }
}
public class DropItem :MonoBehaviour
{
    private ItemPool pool;
    private float lifeTime = 5f;

    public void Init(ItemPool poolRef)
    {
        poolRef = poolRef;
        CancelInvoke();
        Invoke(nameof(RetrunToPool),lifeTime);
    }
    private void RetrunToPool()
    {
        pool.RetrunItem(gameObject);
    }*/
}