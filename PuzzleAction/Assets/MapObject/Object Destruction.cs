using UnityEngine;

//creatmapからもらった情報を元にMapObject生成
//生成するときにItem情報や見た目付与
public class ObjectDestruction : MonoBehaviour
{
    //creat map 
    //item pool or manager 

    //big middole small 
    // mitame

    //それぞれのHPやらなんやらの設定
    [SerializeField] private int maxHitCount = 1;
    private int currentHitCount;



    [SerializeField]private GameObject itemPrefab; //item pool 

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
            //spawn
            //item.transform.position=transform.position;

            Debug.Log($"{gameObject.name}が破壊されました!アイテムをドロップします");
        }

        Destroy(gameObject);
        Debug.Log($"{gameObject.name}が破壊されました!何も出ませんシケです");
        //gameObject.SetActive(false);//pool
    }
}
