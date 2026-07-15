using UnityEngine;
using System.Collections.Generic;

public class MapObjectManager : MonoBehaviour
{
  public static MapObjectManager Instance{get;private set;}

    [Header("あれ今日ビジュいいじゃん")]
    [SerializeField] private GameObject boxVisual;
    [SerializeField] private GameObject potVisual;
    // [SerializeField] private GameObject ;

    [Header("MapObject")]
    [SerializeField] private GameObject mapObjectBasePrefab;
                                        
    [Header("ドロップ")]
    [SerializeField] private List<GameObject> itemPoolList;

    private List<MapObject> m_objectPool = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnObject(Vector3 position)
    {
        MapObject mapObj = GetOrCreateObject();
        mapObj.transform.position = position;
        mapObj.gameObject.SetActive(true);

        string objectType = Random.value > 0.5f ? "Box" : "Pot";
        string size = GetRandomSize();
        int hp=GetHpBySize(size);

        GameObject itemToDrop = GetRandomItem();

        GameObject visual = objectType == "Box" ? boxVisual : potVisual;

        Debug.Log($"【生成】種類: {objectType} | サイズ: {size} (初期耐久: {hp})");

        mapObj.Initialize(hp,itemToDrop,visual);
        //mapObj.OnDestroyed += HandleObjectDestroyed;
    }

    private MapObject GetOrCreateObject()
    {
        foreach(var obj in m_objectPool)
        {
            if(!obj.gameObject.activeSelf)return obj;
        }
        GameObject newObj = Instantiate(mapObjectBasePrefab, transform);
        MapObject mapObj=newObj.GetComponent<MapObject>();
        mapObj.OnDestroyed += HandleObjectDestroyed;
        m_objectPool.Add(mapObj);
        return mapObj;
    }
    private void HandleObjectDestroyed(MapObject mapObj)
    {
        //mapObj.OnDestroyed -= HandleObjectDestroyed;

        if(mapObj.ItemPrefab!=null)
        {
            Debug.Log($"{mapObj.ItemPrefab.name}から{mapObj.ItemPrefab.name}をドロップ");

            Instantiate(mapObj.ItemPrefab,mapObj.transform.position,Quaternion.identity);
        }
        else
        {
            Debug.Log($"{mapObj.gameObject.name}は何も落とさなかったシケ");
        }
        mapObj.gameObject.SetActive(false);
    }
    private int GetHpBySize(string size)
    {
        return size switch
        {
            "Large" => 3,
            "Medium" => 2,
            "Small" => 1,
            _ => 1
        };
    }

    private string GetRandomSize()
    {
        float rand = Random.value;
        if (rand < 0.2f) return "Large";
        if (rand < 0.5f) return "Medium";
        return "Small";
    }
    private GameObject GetRandomItem()
    {
        if (itemPoolList == null || itemPoolList.Count == 0) return null;
        return itemPoolList[Random.Range(0,itemPoolList.Count)];
    }

    //テスト
    [ContextMenu("test")]
    void test()
    {
            Vector3 testPosition = new Vector3(0, 0.5f, 0);

            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                testPosition = player.transform.position + player.transform.forward * 2f;
                testPosition.y = 0.5f; // 地面の高さ調整
            }

            SpawnObject(testPosition);
            Debug.Log("【テスト】オブジェクトを生成しました！");
    }
}
