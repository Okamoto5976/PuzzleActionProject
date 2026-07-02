using UnityEngine;

public class ObjectDestruction : MonoBehaviour
{
    [SerializeField] private int maxHitCount = 1;
    private int currentHitCount;
    [SerializeField]private GameObject itemPrefab;

     void Start()
    {
        currentHitCount = maxHitCount;   
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            RegisterHit();
        }
    }
    private void RegisterHit()
    {
        currentHitCount--;
        Debug.Log($"{gameObject.name}にヒット！残り耐久:{currentHitCount}");
        if (currentHitCount <= 0)
        {
            DestroyBox();
        }
    }
    private void DestroyBox()
    {
        if (itemPrefab != null)
        {
            //dropdrop
            //DropManager.Instance.SpawnItem(itemPrefab,transform.position);

            //ObjectPool
            //GameObject item=objectPool.Rent(itemPrefab);
            //item.transform.position=transform.position;

            Debug.Log($"{gameObject.name}が破壊されました!アイテムをドロップします");
        }
        Destroy(gameObject);
        Debug.Log($"{gameObject.name}が破壊されました!何も出ませんシケです");
        //gameObject.SetActive(false);//pool
    }
}
