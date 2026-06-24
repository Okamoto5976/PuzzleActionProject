using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private float timeToReturn = 5f;

    private Item item;              // このドロップが持っているアイテム
    private dropPool pool;          // 戻す先のプール
    private GameObject prefab;      // どのプレハブのプールに戻すか

    // dropPool から呼ばれる初期化
    public void Setup(Item itemData, dropPool dropPool, GameObject sourcePrefab)
    {
        item = itemData;
        pool = dropPool;
        prefab = sourcePrefab;

        CancelInvoke();
        Invoke(nameof(ReturnToPool), timeToReturn);
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity entity = other.GetComponent<Entity>();
        if (entity == null) return;

        bool added = entity.ReceiveItem(item);
        if (!added) return; // インベントリがいっぱい等なら拾わない

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        CancelInvoke();

        if (pool != null)
        {
            pool.ReturnItem(gameObject, prefab);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}